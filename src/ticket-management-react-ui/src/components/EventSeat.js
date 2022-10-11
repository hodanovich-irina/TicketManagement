import React,{Component} from 'react';
import { variables, eventMethods, presentationMethods, userMethods, roleConstants } from '../configures/Variables';
import BookedEventSeatButton from './event_seat_components/BookedEventSeatButton';
import BuyButton from './event_seat_components/BuyButton';
import EventSeatButton from './event_seat_components/EventSeatButton';
import Cookies from 'js-cookie'
import LocalizedStrings from 'react-localization';
import EventSeatLocalizerData from './localization_components/EventSeatLocalizerData';

let localizeStr = new LocalizedStrings(EventSeatLocalizerData)
export class EventSeat extends Component {

    constructor(props){
        super(props);

        this.state={
            eventSeats:[],
            modalTitle:"",
            row:0,
            number:0,
            id:0,
            eventAreaId: 0,
            state: 0,
            roles:[]
        }
    }

    refreshList(){
        console.log(this.props.getAreaId)
        fetch(variables.EventSeatApiUrl+eventMethods.GetByParentId + this.props.getAreaId)
        .then(response=>response.json())
        .then(data=>{
            this.setState({eventSeats:data});
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

    render(){
        const{
            eventSeats,
            roles
        }=this.state;
        localizeStr.setLanguage(Cookies.get("language"))

        return(
            <div className='inline pb-5'>
                {eventSeats.map(eventSeat=> eventSeat.state===0?
                <EventSeatButton key = {eventSeat.id} id = {eventSeat.id} row = {eventSeat.row} number = {eventSeat.number}>
                    {roles.find(role=>role===roleConstants.User) !== undefined ?<BuyButton text ={localizeStr.BuyTicket} 
                    onClick = {() => window.location.assign(variables.PresentationUrl + presentationMethods.BuyTicket + (eventSeat.id))}/>:null}
                </EventSeatButton>:
                 <BookedEventSeatButton key = {eventSeat.id} id = {eventSeat.id}
                 row = {eventSeat.row} number = {eventSeat.number}/>
                 )}
            </div>
        )
    }
}