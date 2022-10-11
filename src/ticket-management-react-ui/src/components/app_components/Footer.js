import { render } from '@testing-library/react';
import React from 'react';
import Cookies from 'js-cookie'
import LocalizedStrings from 'react-localization';
import FooterLocalizeData from '../localization_components/FooterLocalizeData';

let localizeStr = new LocalizedStrings(FooterLocalizeData)
const Footer = () => {
    render(localizeStr.setLanguage(Cookies.get("language")) )
    return(
        <footer className="navbar fixed-bottom navbar-light bg-white">
        <div className="container">
            &copy; 2022 - {localizeStr.TicketManagement}
        </div>
        </footer>
    )
}
export default Footer;
