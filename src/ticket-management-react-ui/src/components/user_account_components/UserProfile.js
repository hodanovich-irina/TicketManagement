import React from 'react';
import Cookies from 'js-cookie'
import LocalizedStrings from 'react-localization';
import { render } from '@testing-library/react';
import UserAccountLocalizerData from '../localization_components/UserAccountLocalizerData';

let localizeStr = new LocalizedStrings(UserAccountLocalizerData)

const UserProfile = ({children, ...props}) => {
    render(localizeStr.setLanguage(Cookies.get("language")))
    return(
        <div className="modal-content h-100 w-100 ">
        <div className="modal-header">
            <h4>{props.surname} {props.name} {props.patronymic}</h4>
        </div>
        <div className="modal-body">
            <dl className="dl-horizontal">
                <dt>{localizeStr.Year}:</dt>
                <dd>{props.year}</dd>

                <dt>{localizeStr.Balance}:</dt>
                <dd>{props.balance} $</dd>

                <dt>{localizeStr.Language}:</dt>
                <dd>{props.language}</dd>

                <dt>{localizeStr.TimeZone}:</dt>
                <dd>{props.timeZoneId}</dd>

                <dt>{localizeStr.Actions}:</dt>
                <dd>
                    {children}
                </dd>
            </dl>
        </div>
        </div>)
}
export default UserProfile;