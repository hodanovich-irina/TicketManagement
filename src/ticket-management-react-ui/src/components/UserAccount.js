import React,{Component} from "react";
import Cookies from 'js-cookie'
import { roleConstants, userMethods, variables } from "../configures/Variables";
import UserProfileHeader from "./user_account_components/UserProfileHeader";
import BuyButton from "./event_seat_components/BuyButton";
import UserProfile from "./user_account_components/UserProfile";
import AddEditModalWindow from "./event_components/AddEditModalWindow";
import EventInputField from "./event_components/EventInputField";
import EditUserButton from "./user_account_components/EditUserButton";
import EditBalanceModalWindow from "./user_account_components/EditBalanceModalWindow";
import EditBalanceButton from "./user_account_components/EditBalanceButton";
import EditPasswordModalWindow from "./user_account_components/EditPasswordModalWindow";
import EditPasswordButton from "./user_account_components/EditPasswordButton";
import GetEventSeatButton from "./event_area_components/GetEventSeatButton";
import LocalizedStrings from 'react-localization';
import UserAccountLocalizerData from "./localization_components/UserAccountLocalizerData";

let localizeStr = new LocalizedStrings(UserAccountLocalizerData)

export class UserAccount extends Component{
    constructor(props){
        super(props);

        this.state={
            user:{},
            modalTitle:"",
            id:"",            
            email:"",
            surname:"",
            name :"",
            patronymic: "",
            year:"",
            balance : 0.0,
            language:"",
            timeZoneId: "",
            newPassword: "",
            oldPassword:"",
            roles:[],
        }
    }
    
    refreshList(){
        fetch(variables.UserApiUrl+userMethods.GetCurrentUser,
            {
                headers:{ 'Authorization' : Cookies.get("tokenToReact")}
            }).then(variables.UserApiUrl+userMethods.GetCurrentUser)
        .then(response=>response.json())
        .then(data=>{
            this.setState({user:data});
        });

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

    changeEmail=(userEmail)=>{
        this.setState({email:userEmail.target.value});
    }
    changeName=(userName)=>{
        this.setState({name:userName.target.value});
    }

    changeSurname=(userSurname)=>{
        this.setState({surname:userSurname.target.value});
    }

    changePatronymic=(userPatronymic)=>{
        this.setState({patronymic:userPatronymic.target.value});
    }

    changeYear=(userYear)=>{
        this.setState({year:userYear.target.value});
    }

    changeBalance=(userBalance)=>{
        this.setState({balance: userBalance.target.value});
    }

    changeOldPassword=(userOldPassword)=>{
        this.setState({oldPassword: userOldPassword.target.value});
    }

    changeNewPassword=(userNewPassword)=>{
        this.setState({newPassword: userNewPassword.target.value});
    }

    editClick(user){
        this.setState({
            modalTitle:localizeStr.EditProfile,
            id:user.id,
            name:user.name,
            email:user.email,
            surname:user.surname,
            patronymic: user.patronymic,
            year:user.year,
            language:user.language,
            timeZoneId: user.timeZoneId,
            balance: user.balance,
            newPassword: "",
            oldPassword: "",
        });
    }
    
    updateClick(){
        fetch(variables.UserApiUrl+userMethods.Update,{
            method:'PUT',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json',
                'Authorization' : Cookies.get("tokenToReact"),
            },
            body:JSON.stringify({
                id:this.state.id,
                name:this.state.name,
                email:this.state.email,
                surname:this.state.surname,
                patronymic: this.state.patronymic,
                year:this.state.year,
                language:this.state.language,
                timeZoneId: this.state.timeZoneId,
                balance: this.state.balance,
            })
        })
        .then(res=>res.status === 200)
        .then((result)=>{
            this.refreshList();
        },(error)=>{
            alert(localizeStr.Failed);
        })
    }

    updateBalanceClick(){
        fetch(variables.UserApiUrl+userMethods.UpdateBalance,{
            method:'PUT',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json',
                'Authorization' : Cookies.get("tokenToReact"),
            },
            body:JSON.stringify({
                id:this.state.id,
                name:this.state.name,
                email:this.state.email,
                surname:this.state.surname,
                patronymic: this.state.patronymic,
                year:this.state.year,
                language:this.state.language,
                timeZoneId: this.state.timeZoneId,
                balance: this.state.balance,
            })
        })
        .then(res=>res.status === 200)
        .then((result)=>{
            this.refreshList();
        },(error)=>{
            alert(localizeStr.Failed);
        })
    }

    changePasswordClick(){
        fetch(variables.UserApiUrl+userMethods.ChangePassword,{
            method:'POST',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json',
                'Authorization' : Cookies.get("tokenToReact"),
            },
            body:JSON.stringify({
                id:this.state.id,   
                newPassword : this.state.newPassword,
                oldPassword: this.state.oldPassword,             
                name:this.state.name,
                email:this.state.email,
                surname:this.state.surname,
                patronymic: this.state.patronymic,
                year:this.state.year,
                language:this.state.language,
                timeZoneId: this.state.timeZoneId,
                balance: this.state.balance,
                user: this.state.user,
            })
        })
        .then(res=>res.status === 200)
        .then((result)=>{
            this.refreshList();
        },(error)=>{
            alert(localizeStr.Failed);
        })
    }

    render(){
        const{
            user,
            name,
            email,
            surname,
            patronymic,
            year,
            balance,
            newPassword,
            oldPassword,
            roles
        }=this.state;
        localizeStr.setLanguage(Cookies.get("language"))
        return(
            roles.find(role=>role===roleConstants.User) !== undefined ?
            <div className='container pb-3'> 
                <UserProfileHeader email = {user.email} text = {localizeStr.PersonalArea}><BuyButton text = {localizeStr.Events} onClick = {() => window.location.assign(variables.EventUIUrl)}/></UserProfileHeader>
                <UserProfile id = {user.id} email = {user.email} surname = {user.surname} name = {user.name }
                patronymic = {user.patronymic} year = {user.year} balance = {user.balance} language = {user.language}
                timeZoneId = {user.timeZoneId}>
                    <EditUserButton text ={localizeStr.EditProfile} onClick={()=>this.editClick(user)}  id = {user.id} email = {user.email} surname = {user.surname} name = {user.name }
                patronymic = {user.patronymic} year = {user.year} balance = {user.balance} language = {user.language}
                timeZoneId = {user.timeZoneId}></EditUserButton>
                    <EditBalanceButton text ={localizeStr.TopUpBalance} onClick={()=>this.editClick(user)}  id = {user.id} email = {user.email} surname = {user.surname} name = {user.name }
                patronymic = {user.patronymic} year = {user.year} balance = {user.balance} language = {user.language}
                timeZoneId = {user.timeZoneId}></EditBalanceButton>
                    <GetEventSeatButton text = {localizeStr.PurchaseHistory} onClick = {() => window.location.assign(variables.TicketUIUrl + (user.id))}/> 
                    <EditPasswordButton text ={localizeStr.ChangePassword} onClick={()=>this.editClick(user)}  id = {user.id} email = {user.email} surname = {user.surname} name = {user.name }
                patronymic = {user.patronymic} year = {user.year} balance = {user.balance} language = {user.language} oldPassword = {user.oldPassword} newPassword ={user.newPassword}
                timeZoneId = {user.timeZoneId}></EditPasswordButton>
                </UserProfile>
                <AddEditModalWindow modalTitle = {this.modalTitle}>
                    <EventInputField text = {localizeStr.Email}>
                        <input type="text" className="form-control"
                        value={email} onChange={this.changeEmail}/>
                    </EventInputField>
                    <EventInputField text = {localizeStr.Year}>
                        <input type="number" className="form-control"
                        value={year} onChange={this.changeYear}/>
                    </EventInputField>
                    <EventInputField text = {localizeStr.Surname}>
                        <input type="text" className="form-control"
                        value={surname} onChange={this.changeSurname}/>
                    </EventInputField>
                    <EventInputField text = {localizeStr.Name}>
                        <input type="text" className="form-control"
                        value={name} onChange={this.changeName}/>
                    </EventInputField>
                    <EventInputField text = {localizeStr.Patronymic}>
                        <input type="text" className="form-control"
                        value={patronymic} onChange={this.changePatronymic}/>
                    </EventInputField>
                    <button type="button"
                    className="btn btn-dark float-start"
                    onClick={()=>this.updateClick()}>
                        {localizeStr.Save}
                    </button>
                </AddEditModalWindow>
                <EditBalanceModalWindow>
                    <EventInputField text = {localizeStr.Balance}>
                        <input type="number" className="form-control"
                        value={balance} onChange={this.changeBalance}/>
                    </EventInputField>
                    <button type="button"
                    className="btn btn-dark float-start"
                    onClick={()=>this.updateBalanceClick()}>
                        {localizeStr.TopUpBalance}
                    </button>
                </EditBalanceModalWindow>
                <EditPasswordModalWindow>
                <EventInputField text = {localizeStr.OldPassword}>
                        <input type="text" className="form-control"
                        value = {oldPassword} onChange={this.changeOldPassword}/>
                    </EventInputField>
                    <EventInputField text = {localizeStr.NewPassword}>
                        <input type="text" className="form-control"
                        value={newPassword} onChange={this.changeNewPassword}/>
                    </EventInputField>
                    <button type="button"
                    className="btn btn-dark float-start"
                    onClick={()=>this.changePasswordClick()}>
                        {localizeStr.Save}
                    </button>
                </EditPasswordModalWindow>
            </div>:null
        )
    }
}