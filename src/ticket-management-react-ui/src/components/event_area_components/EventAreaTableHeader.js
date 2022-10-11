import React from 'react';
import Cookies from 'js-cookie'
import LocalizedStrings from 'react-localization';
import { render } from '@testing-library/react';
import EventAreaLocalizerData from '../localization_components/EventAreaLocalizerData';

let localizeStr = new LocalizedStrings(EventAreaLocalizerData)

const EventAreaTableHeader = () => {
    render(localizeStr.setLanguage(Cookies.get("language")))
    return (
        <thead>
            <tr>
                <th>
                    {localizeStr.CoordinateX}
                </th>
                <th>
                    {localizeStr.CoordinateY}
                </th>
                <th>
                    {localizeStr.Description}
                </th>
                <th>
                    {localizeStr.Event}
                </th>
                <th>
                    {localizeStr.TicketPrice}
                </th>
                <th>
                </th>
            </tr>
        </thead>
    )
}
export default EventAreaTableHeader;