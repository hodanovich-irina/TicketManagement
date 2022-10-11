import React from 'react';
import EventAreaTableHeader from './EventAreaTableHeader';

const EventAreaTable = ({children}) => {
    return (
        <table className="table bg-white">
            <EventAreaTableHeader/>
            <tbody>
                {children}
            </tbody>
        </table>
    )
}
export default EventAreaTable;