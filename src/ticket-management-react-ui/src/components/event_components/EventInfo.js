import React from 'react';
import LocalizedStrings from 'react-localization';
import { render } from '@testing-library/react';
import EventLocalizerData from '../localization_components/EventLocalizerData';
import Cookies from 'js-cookie'

let localizeStr = new LocalizedStrings(EventLocalizerData)
const EventInfo = ({children, ...props}) =>{
    render(localizeStr.setLanguage(Cookies.get("language")))
    return(
        <div>
            <dt>{localizeStr.Name}:</dt>
            <dd>{props.name}</dd>
            <dt>{localizeStr.Description}:</dt>
            <dd>{props.description}</dd>
            <dt>{localizeStr.ShowTime}:</dt>
            <dd>{String(props.hours).length  < 2 ? <>0{props.hours}</>:<>{props.hours}</>}: 
            {String(props.minutes).length  < 2 ? <>0{props.minutes}</>:<>{props.minutes}</>} 
            </dd>
            <dt>{localizeStr.ShowPeriod}:</dt>
            <dd>{String(props.dateStart).substring(0,10)} — {String(props.dateEnd).substring(0,10)}</dd>
            <dd>
                {children}
            </dd>
        </div>
    )
}
export default EventInfo;