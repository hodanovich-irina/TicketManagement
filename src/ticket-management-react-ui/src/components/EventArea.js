import React,{Component} from 'react';
import { variables, eventMethods } from '../configures/Variables';
import EventAreaTableBody from './event_area_components/EventAreaTableBody';
import EventAreaTable from './event_area_components/EventAreaTable';
import GetEventSeatButton from './event_area_components/GetEventSeatButton';
import Cookies from 'js-cookie'
import LocalizedStrings from 'react-localization';
import EventAreaLocalizerData from './localization_components/EventAreaLocalizerData';

let localizeStr = new LocalizedStrings(EventAreaLocalizerData)
export class EventArea extends Component {

    constructor(props){
        super(props);

        this.state={
            eventAreas:[],
            modalTitle:"",
            coordX:0,
            coordY:0,
            id:0,
            description:"",
            eventId: 0,
            price: 0.0
        }
    }

    refreshList(){
        console.log(this.props.getId)
        fetch(variables.EventAreaApiUrl+eventMethods.GetByParentId + this.props.getId)
        .then(response=>response.json())
        .then(data=>{
            this.setState({eventAreas:data});
        });
    }

    componentDidMount(){
        this.refreshList();
    }

    render(){
        const{
            eventAreas,
        }=this.state;
        localizeStr.setLanguage(Cookies.get("language"))

        return(
            <div className='container pb-3'>
                <EventAreaTable>
                    {eventAreas.map(eventArea=>
                    <EventAreaTableBody key = {eventArea.id} id = {eventArea.id} coordX = {eventArea.coordX} coordY = {eventArea.coordY} 
                        description = {eventArea.description} eventId = {eventArea.eventId} price = {eventArea.price} name={this.props.getName}>
                    <GetEventSeatButton text = {localizeStr.Seats} onClick = {() => window.location.assign(variables.EventSeatUIUrl + (eventArea.id))}/> 
                    </EventAreaTableBody>)}
                </EventAreaTable>
            </div>
        )
    }
}