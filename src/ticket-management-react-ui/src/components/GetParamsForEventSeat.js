import React from 'react';
import { useParams } from 'react-router-dom';
import { EventSeat } from './EventSeat';

function GetParamsForEventSeat() {
    const {id} = useParams();
    return(<EventSeat getAreaId={id}/>);
}

export default GetParamsForEventSeat;