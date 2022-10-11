import React from 'react';
import PurchaseHistoryTableHeader from './PurchaseHistoryTableHeader';

const PurchaseHistoryTable = ({children}) => {
    return (
        <table className="table bg-white">
            <PurchaseHistoryTableHeader/>
            <tbody>
                {children}
            </tbody>
        </table>
    )
}
export default PurchaseHistoryTable;