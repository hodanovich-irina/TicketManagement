import React from 'react';
import { useParams } from 'react-router-dom';
import { EventArea } from './EventArea';

function GetParamsForEventArea() {
    const {id} = useParams();
    const {name} = useParams();
    return(<EventArea getId={id} getName={name}/>);
}

export default GetParamsForEventArea;