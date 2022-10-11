import React from 'react';
import { useParams } from 'react-router-dom';
import { PurchaseHistory } from './PurchaseHistory';

function GetParamsForPurchaseHistory() {
    const {id} = useParams();
    return(<PurchaseHistory getUserId={id}/>);
}

export default GetParamsForPurchaseHistory;