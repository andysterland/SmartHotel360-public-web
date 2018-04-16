import * as React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import SearchInfo from './SearchInfo';
import Filters from './Filters';
import BestRoomsAll from './BestRoomsAll';
import BestRoom from './BestRoom';

type SearchRoomsProps =
    RouteComponentProps<{}>;

export default class BestRooms extends React.Component<SearchRoomsProps, {}> {
    public render() {
        return <div className='sh-search_rooms'>
            <SearchInfo />
            <Filters />
            <BestRoomsAll component={BestRoom} isLinked={true} title='Best Rooms' modifier='full' />
        </div>;
    }
}