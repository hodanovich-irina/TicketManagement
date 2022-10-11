import React from 'react';
import Cookies from 'js-cookie';
import LocalizedStrings from 'react-localization';
import { render } from '@testing-library/react';
import EventSeatLocalizerData from '../localization_components/EventSeatLocalizerData';

let localizeStr = new LocalizedStrings(EventSeatLocalizerData)

const BookedEventSeatButton = ({...props}) =>{
    render(localizeStr.setLanguage(Cookies.get("language")))
    return(
        <div className="d-inline p-2"><button type="button" className="btn btn-danger">
            <h6>{localizeStr.Row}: {props.row} {localizeStr.Place}: {props.number}</h6>
        </button></div>)
}
export default BookedEventSeatButton;