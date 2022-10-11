import React from 'react';
import Cookies from 'js-cookie';
import LocalizedStrings from 'react-localization';
import { render } from '@testing-library/react';
import EventSeatLocalizerData from '../localization_components/EventSeatLocalizerData';

let localizeStr = new LocalizedStrings(EventSeatLocalizerData)

const EventSeatButton = ({children, ...props}) =>{
    render(localizeStr.setLanguage(Cookies.get("language")))
    return(
        <div className="d-inline p-3">
            {children}
            <button type="button" className="btn btn-outline-success " data-toggle="modal" data-target="#modal">
            <h6>{localizeStr.Row}: {props.row} {localizeStr.Place}: {props.number}</h6>
        </button></div>)
}
export default EventSeatButton;