import React from 'react';
import Cookies from 'js-cookie'
import LocalizedStrings from 'react-localization';
import { render } from '@testing-library/react';
import PurchaseHistoryLocalizeData from '../localization_components/PurchaseHistoryLocalizeData';

let localizeStr = new LocalizedStrings(PurchaseHistoryLocalizeData)

const PurchaseHistoryTableHeader = () => {
    render(localizeStr.setLanguage(Cookies.get("language")))
    return (
        <thead>
            <tr>
                <th>
                    {localizeStr.DateOfPurchase}
                </th>
                <th>
                    {localizeStr.Price}
                </th>
                <th>
                    {localizeStr.Row}
                </th>
                <th>
                    {localizeStr.Place}
                </th>
                <th>
                    {localizeStr.Event}
                </th>
                <th>
                    {localizeStr.EventPeriod}
                </th>
                <th>
                </th>
            </tr>
        </thead>
    )
}
export default PurchaseHistoryTableHeader;