import React,{Component} from 'react';
import { variables, ticketMethods, presentationMethods } from '../configures/Variables';
import PurchaseHistoryTable from './purchase_history_components/PurchaseHistoryTable';
import PurchaseHistoryTableBody from './purchase_history_components/PurchaseHistoryTableBody';
import RefuseTicketButton from "./purchase_history_components/RefuseTicketButton";
import Cookies from 'js-cookie'
import LocalizedStrings from 'react-localization';
import PurchaseHistoryLocalizeData from './localization_components/PurchaseHistoryLocalizeData';

let localizeStr = new LocalizedStrings(PurchaseHistoryLocalizeData)
export class PurchaseHistory extends Component {

    constructor(props){
        super(props);

        this.state={
            tickets:[],
            events:[],
            dateOfPurchase: new Date(),
            modalTitle:"",
            price:0.0,
            number:0,
            id:0,
            row: 0,
            eventName :"",
            dateStart: new Date(),
            dateEnd: new Date(),
            userid:"",
        }
    }

    refreshList(){
        console.log(this.props.getUserId)
        fetch(variables.TicketApiUrl+ticketMethods.GetUserTicketInfo + this.props.getUserId)
        .then(response=>response.json())
        .then(data=>{
            this.setState({tickets:data});
        });
    }

    componentDidMount(){
        this.refreshList();
    }

    render(){
        const{
            tickets,
        }=this.state;
        localizeStr.setLanguage(Cookies.get("language"))

        return(
            <div className='container pb-3'>
                {tickets.map(ticket=>
                <PurchaseHistoryTable>
                    <PurchaseHistoryTableBody key = {ticket.id} id = {ticket.id} dateOfPurchase = {ticket.dateOfPurchase} row = {ticket.row} 
                        price = {ticket.price} number = {ticket.number} eventName = {ticket.eventName} dateStart={ticket.dateStart} dateEnd = {ticket.dateEnd}>
                          <RefuseTicketButton text = {localizeStr.Refuse} onClick = {() => window.location.assign(variables.PresentationUrl + presentationMethods.DelteTicket + (ticket.id))}/>
                    </PurchaseHistoryTableBody>
                    
                </PurchaseHistoryTable> 
                )}
            </div>
        )
    }
}