import React,{Component} from 'react';
import Cookies from 'js-cookie'
import { variables, eventMethods, userMethods, roleConstants } from '../configures/Variables';
import AddEditModalWindow from './event_components/AddEditModalWindow';
import AddEventButton from './event_components/AddEventButton';
import DeleteEventButton from './event_components/DeleteEventButton';
import EditEventButton from './event_components/EditEventButton';
import EventInfo from './event_components/EventInfo';
import EventInputField from './event_components/EventInputField';
import EventModel from './event_components/EventModel';
import EventModelWindow from './event_components/EventModelWindow';
import GetEventAreaButton from './event_components/GetEventAreaButton';
import { LayoutSelector } from './event_components/LayoutSelector';
import LocalizedStrings from 'react-localization';
import EventLocalizerData from './localization_components/EventLocalizerData';

let localizeStr = new LocalizedStrings(EventLocalizerData)

export class Event extends Component {

    constructor(props){
        super(props);

        this.state={
            events:[],
            modalTitle:"",
            name:"",
            id:0,
            description:"",
            dateStart:new Date(),
            dateEnd:new Date(),
            imageURL:"",
            layoutId:0,
            hours: 0,
            minutes: 0,
            baseAreaPrice: 0,
            roles:[],
        }
    }

    refreshList(){
        fetch(variables.EventApiUrl+eventMethods.GetAll)
        .then(response=>response.json())
        .then(data=>{
            this.setState({events:data});
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

    changeName=(eventName)=>{
        this.setState({name:eventName.target.value});
    }

    changeDescription=(eventDescription)=>{
        this.setState({description:eventDescription.target.value});
    }

    changeDateStart=(eventDateStart)=>{
        this.setState({dateStart:eventDateStart.target.value});
    }

    changeDateEnd=(eventDateEnd)=>{
        this.setState({dateEnd:eventDateEnd.target.value});
    }
    
    changeImageURL=(eventImageURL)=>{
        this.setState({imageURL:eventImageURL.target.value});
    }
    
    changeLayoutId=(eventLayoutId)=>{
        this.setState({layoutId:eventLayoutId.target.value});
    }

    changeHours=(eventHours)=>{
        this.setState({hours:eventHours.target.value});
    }

    changeMinutes=(eventMinutes)=>{
        this.setState({minutes:eventMinutes.target.value});
    }
    
    changeBaseAreaPrice=(eventBaseAreaPrice)=>{
        this.setState({baseAreaPrice:eventBaseAreaPrice.target.value});
    }
    addClick(){
        this.setState({
            modalTitle: localizeStr.AddEvent,
            id:0,
            name:"",
            description:"",
            dateStart:new Date(),
            dateEnd:new Date(),
            imageURL:"",
            layoutId:0,
            baseAreaPrice: 1.0,
            hours: 0,
            minutes:0,
        });
    }

    editClick(evt){
        this.setState({
            modalTitle:localizeStr.Change,
            id:evt.id,
            name:evt.name,
            description:evt.description,
            dateStart:evt.dateStart,
            dateEnd:evt.dateEnd,
            imageURL:evt.imageURL,
            layoutId: evt.layoutId,
            hours : evt.hours,
            minutes: evt.minutes,
            baseAreaPrice: evt.baseAreaPrice
        });
    }
    
    handleChangeLayoutId = (event) => {
        this.setState({layoutId: event.target.value});
    }

    createClick(){
        fetch(variables.EventApiUrl+eventMethods.Add,{
            method:'POST',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json',
                'Authorization' : Cookies.get("tokenToReact"),
            },
            body:JSON.stringify({
                name:this.state.name,
                description:this.state.description,    
                dateStart:this.state.dateStart,
                dateEnd:this.state.dateEnd,
                imageURL:this.state.imageURL,
                layoutId:this.state.layoutId,
                hours: this.state.hours,
                minutes: this.state.minutes,
                baseAreaPrice: this.state.baseAreaPrice
            })
        })
        .then(res=>res.status === 200)
        .then((result)=>{
            this.refreshList();
        },(error)=>{
            alert(localizeStr.Failed);
        })
    }


    updateClick(){
        fetch(variables.EventApiUrl+eventMethods.Update,{
            method:'PUT',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json',
                'Authorization' : Cookies.get("tokenToReact"),
            },
            body:JSON.stringify({
                id:this.state.id,
                name:this.state.name,
                description:this.state.description,    
                dateStart:this.state.dateStart,
                dateEnd:this.state.dateEnd,
                imageURL:this.state.imageURL,
                layoutId:this.state.layoutId,
                hours: this.state.hours,
                minutes: this.state.minutes,
                baseAreaPrice: this.state.baseAreaPrice
            })
        })
        .then(res=>res.json())
        .then((result)=>{
            this.refreshList();
        },(error)=>{
            alert(localizeStr.Failed);
        })
    }

    
    deleteClick(id){
        if(window.confirm(localizeStr.DelMes)){
        fetch(variables.EventApiUrl+eventMethods.Delete+id,{
            method: 'DELETE',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json',
                'Authorization' : Cookies.get("tokenToReact"),
            }
        })
        .then(res=>res.json())
        .then((result)=>{
            this.refreshList();
        },(error)=>{
            alert(localizeStr.Failed);
        })
    }
    }

    render(){
        const{
            events,
            modalTitle,
            id,
            name,
            description,
            dateStart,
            dateEnd,
            imageURL,
            baseAreaPrice,
            roles,
            hours,
            minutes
        }=this.state;
        localizeStr.setLanguage(Cookies.get("language"))

        return(
            <div className='container pb-3'>
                <EventModelWindow>
                { roles.find(role=>role===roleConstants.EventManager) !== undefined ?
                <AddEventButton but_name = {localizeStr.AddEvent} onClick={()=>this.addClick()}/>:null}
                </EventModelWindow>
                {events.map(evt=>
                    <EventModel key = {evt.id} imageURL = {evt.imageURL}>
                        <EventInfo id = {evt.id} name = {evt.name} description = {evt.description} 
                        dateStart = {evt.dateStart} dateEnd = {evt.dateEnd} layoutId = {evt.layoutId}
                        hours = {evt.hours} minutes = {evt.minutes}>
                        {roles.find(role=>role===roleConstants.EventManager) !== undefined ?
                            <><EditEventButton onClick={()=>this.editClick(evt)}/>
                            <DeleteEventButton onClick={()=>this.deleteClick(evt.id)}/></>: null}
                            {roles.find(role=>role===roleConstants.EventManager || role === roleConstants.User) !== undefined ?
                            <GetEventAreaButton onClick = {() => window.location.assign(variables.EventAreaUIUrl + (evt.id) +
                                eventMethods.SplitSumbol + (evt.name))}/>:null}                       
                        </EventInfo>
                    </EventModel>)}
                <AddEditModalWindow modalTitle = {modalTitle}>
                    <EventInputField text = {localizeStr.Name}>
                        <input type="text" className="form-control"
                        value={name} onChange={this.changeName}/>
                    </EventInputField>
                    <EventInputField text = {localizeStr.Description}>
                        <input type="text" className="form-control"
                        value={description} onChange={this.changeDescription}/>
                    </EventInputField>
                    <EventInputField text = {localizeStr.DateStart}>
                        <input type="datetime-local" className="form-control"
                        value={dateStart} onChange={this.changeDateStart}/>
                    </EventInputField>
                    <EventInputField text = {localizeStr.DateEnd}>
                        <input type="datetime-local" className="form-control"
                        value={dateEnd} onChange={this.changeDateEnd}/>
                    </EventInputField>
                    <EventInputField text = {localizeStr.URL}>
                        <input type="text" className="form-control" 
                        value={imageURL} onChange={this.changeImageURL}/>
                    </EventInputField>
                    <EventInputField text = {localizeStr.Price}>
                        <input type="number" className="form-control" 
                        value={baseAreaPrice} onChange={this.changeBaseAreaPrice}/>
                    </EventInputField>
                    <EventInputField text = {localizeStr.ShowTimeHours}>
                        <input type="number" className="form-control" 
                        value={hours} onChange={this.changeHours}/>
                    </EventInputField>
                    <EventInputField text = {localizeStr.ShowTimeMinutes}>
                        <input type="number" className="form-control" 
                        value={minutes} onChange={this.changeMinutes}/>
                    </EventInputField>
                    {id===0?
                    <>
                    <LayoutSelector layoutId = {this.state.layoutId} handleChangeLayoutId = {this.handleChangeLayoutId}/>
                    <button type="button"
                    className="btn btn-dark float-start"
                    onClick={()=>this.createClick()}>
                        {localizeStr.Save}
                    </button></>:null}
                    {id!==0?
                    <button type="button"
                    className="btn btn-dark float-start"
                    onClick={()=>this.updateClick()}>
                        {localizeStr.Save}
                    </button>:null}
                        
                </AddEditModalWindow>
            </div>)}
}