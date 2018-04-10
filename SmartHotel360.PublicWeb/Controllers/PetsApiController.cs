using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using SmartHotel360.PublicWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHotel360.PublicWeb.Controllers
{

    public class PetUploadRequest
    {
        public string Base64 { get; set; }
        public string Name { get; set; }
    }

    [Route("api/pets")]
    public class PetsApiController : Controller
    {
        private readonly SettingsService _settingsSvc;
        private readonly string dbName = "pets";
        private readonly string colName = "checks";
        public PetsApiController(SettingsService settingsSvc)
        {
            _settingsSvc = settingsSvc;
        }

        [HttpPost]
        public async Task<IActionResult> UploadPetImageAsync([FromBody] PetUploadRequest petRequest)
        {

            if (string.IsNullOrEmpty(petRequest?.Base64))
            {
                return BadRequest();
            }

            DocumentClient client = await InitializeClient();

            return PerformUploadPetImage(petRequest, client);
        }

        private IActionResult PerformUploadPetImage(PetUploadRequest petRequest, DocumentClient client)
        {
            var tokens = petRequest.Base64.Split(',');
            var ctype = tokens[0].Replace("data:", "");
            var base64 = tokens[1];
            var content = Convert.FromBase64String(base64);

            // Upload photo to storage...
            var blobUri = UploadPetToStorage(content);

            // Then create a Document in CosmosDb to notify our Function
            var identifier = UploadDocument(blobUri, petRequest.Name ?? "Bob", client);

            return Ok(identifier);
        }

        private async Task<DocumentClient> InitializeClient()
        {
            var endpoint = new Uri(_settingsSvc.GlobalSettings.Pets_Config.CosmosUri);
            var auth = _settingsSvc.GlobalSettings.Pets_Config.CosmosKey;
            var client = new DocumentClient(endpoint, auth);
            await client.CreateDatabaseIfNotExistsAsync(new Database() { Id = dbName });
            await client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(dbName),
                new DocumentCollection { Id = colName });
            return client;
        }

        private Guid UploadDocument(Uri uri, string petName, DocumentClient client)
        {
            var identifier = Guid.NewGuid();

            Document d = client.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(dbName, colName),
                new PetDocument
                {
                    Id = identifier,
                    IsApproved = null,
                    PetName = petName,
                    MediaUrl = uri.ToString(),
                    Created = DateTime.UtcNow
                }).Result;

            return identifier;
        }

        private Uri UploadPetToStorage(byte[] content)
        {
            var storageName = _settingsSvc.GlobalSettings.Pets_Config.BlobName;
            var auth = _settingsSvc.GlobalSettings.Pets_Config.BlobKey;
            var uploader = new PhotoUploader(storageName, auth);
            var blob = uploader.UploadPetPhoto(content).Result;
            return blob.Uri;
        }

        [HttpGet]
        public IActionResult GetUploadState(Guid identifier)
        {

            var endpoint = new Uri(_settingsSvc.GlobalSettings.Pets_Config.CosmosUri);
            var auth = _settingsSvc.GlobalSettings.Pets_Config.CosmosKey;
            var client = new DocumentClient(endpoint, auth);

            var collectionUri = UriFactory.CreateDocumentCollectionUri(dbName, colName);
            var query = client.CreateDocumentQuery<PetDocument>(collectionUri, new FeedOptions() { MaxItemCount = 1 });

            var docs = query.Where(x => x.Id == identifier).Where(x => x.IsApproved.HasValue).ToList();

            var doc = docs.FirstOrDefault();

            return Ok(new
            {
                Approved = doc?.IsApproved ?? false,
                Message = doc?.Message ?? ""
            });
        }
    }
}
