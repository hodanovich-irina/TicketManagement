import React from 'react';

const PurchaseHistoryTableBody = ({children, ...props}) => {
    return (
            <tr>
                <td>{String(props.dateOfPurchase).substring(0,10)}</td>
                <td>{props.price}</td>
                <td>{props.row}</td>
                <td>{props.number}</td>
                <td>{props.eventName}</td>
                <td>{String(props.dateStart).substring(0,10)} - {String(props.dateEnd).substring(0,10)}</td>
                <td>
                    {children}
                </td>
            </tr>
    )
}
export default PurchaseHistoryTableBody;