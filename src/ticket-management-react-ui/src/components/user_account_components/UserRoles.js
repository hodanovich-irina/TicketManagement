import { Component } from "react";
import { presentationMethods, userMethods, variables } from "../../configures/Variables";
import Cookies from 'js-cookie'
import NavBar from '../app_components/NavBar';
import { roleConstants } from '../../configures/Variables';
import { NavLink} from 'react-router-dom';
import PresentationRedirect from "../PresentationRedirect";
import LocalizedStrings from 'react-localization';
import HeaderLocalizeData from "../localization_components/HeaderLocalizeData";

let localizeStr = new LocalizedStrings(HeaderLocalizeData)

export class UserRoles extends Component{
    constructor(props){
        super(props);

        this.state={
            roles:[],
        }
    }
    
    refreshList(){
        fetch(variables.UserApiUrl+userMethods.GetCurrentUserRoles,
            {
                headers:{ 'Authorization' : Cookies.get("tokenToReact")}
            }).then(variables.UserApiUrl+userMethods.GetCurrentUserRoles)
        .then(response=>response.json())
        .then(data=>{
            this.setState({roles:data});
        });
    }

    componentDidMount(){
        this.refreshList();
    }

    render(){
        const{
            roles,
        }=this.state;
        localizeStr.setLanguage(Cookies.get("language"))
        
        return(
        roles.find(role=>role) !== undefined ? 
        <NavBar>
            {roles.find(role=>role===roleConstants.User) !== undefined ?
                <NavLink className="nav-link text-dark btn-light rounded"  to="/UserAccount">
                {localizeStr.PersonalArea}
                </NavLink> : null}
            {roles.find(role=>role===roleConstants.Admin) !== undefined ?
                <><PresentationRedirect text = {localizeStr.Users} onClick = {() => window.location.assign(variables.PresentationUrl + presentationMethods.User)}/>
                <PresentationRedirect text = {localizeStr.UserRoles} onClick = {() => window.location.assign(variables.PresentationUrl + presentationMethods.Role)}/>
                </> : null}
            {roles.find(role=>role===roleConstants.EventManager) !== undefined ?
                <><PresentationRedirect text = {localizeStr.Tickets} onClick = {() => window.location.assign(variables.PresentationUrl + presentationMethods.Ticket)}/>
                <PresentationRedirect text = {localizeStr.ImportFile} onClick = {() => window.location.assign(variables.PresentationUrl + presentationMethods.ThirdPartyImport)}/>
                </> : null}
            {roles.find(role=>role===roleConstants.VenueManager) !== undefined ?
                <><PresentationRedirect text = {localizeStr.Venues} onClick = {() => window.location.assign(variables.PresentationUrl + presentationMethods.Venue)}/>
                <PresentationRedirect text = {localizeStr.Users} onClick = {() => window.location.assign(variables.PresentationUrl + presentationMethods.User)}/>
                <PresentationRedirect text = {localizeStr.UserRoles} onClick = {() => window.location.assign(variables.PresentationUrl + presentationMethods.Role)}/>
                </> : null}
        </NavBar> : null
        )
    }
}