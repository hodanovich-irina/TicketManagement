import React from 'react';
import { presentationMethods, variables } from '../../configures/Variables';
import PresentationRedirect from '../PresentationRedirect';
import Cookies from 'js-cookie'
import { LanguageSelector } from '../localization_components/LanguageSelector';
import LocalizedStrings from 'react-localization';
import HeaderLocalizeData from '../localization_components/HeaderLocalizeData';
import { render } from '@testing-library/react';
import { NavLink } from 'react-router-dom';

let localizeStr = new LocalizedStrings(HeaderLocalizeData)

const Header = () => {
    render(localizeStr.setLanguage(Cookies.get("language")))
    return(
        <header>
            <nav className="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
                <div className="container">
                    <a className="navbar-brand">{localizeStr.TicketManagement}</a>
                    <ul className="navbar-nav flex-grow-1">
                        <li className="nav-item">
                        <NavLink className="nav-link text-dark" to="/Event">
                            {localizeStr.Events}
                        </NavLink>
                        </li>
                    </ul>
                    <LanguageSelector/>
                    {Cookies.get("userToReact")===undefined ?
                    <><PresentationRedirect text = {localizeStr.Enter} onClick = {() => window.location.assign(variables.PresentationUrl + presentationMethods.Login)}/>
                    <PresentationRedirect text = {localizeStr.Registration} onClick = {() => window.location.assign(variables.PresentationUrl + presentationMethods.Register)}/></>:
                    <>{Cookies.get("userToReact")}                    
                    <PresentationRedirect text = {localizeStr.Exit} onClick = {() => window.location.assign(variables.PresentationUrl + presentationMethods.Login)}/>
                    </>}
                </div>
            </nav>
        </header>
    )
}
export default Header;
