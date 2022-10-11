import React from 'react';
import LocalizedStrings from 'react-localization';
import { render } from '@testing-library/react';
import EventLocalizerData from '../localization_components/EventLocalizerData';
import Cookies from 'js-cookie'

let localizeStr = new LocalizedStrings(EventLocalizerData)
const EventModelWindow = ({children}) => {
    render(localizeStr.setLanguage(Cookies.get("language")))

    return (
        <div className="text-center">
            <h1 className ="display-4">{localizeStr.Events}</h1>
            <p>{localizeStr.EventMessage}</p>
            <div className='container pb-3'>
                {children}
            </div>
        </div>
    )
}
export default EventModelWindow;