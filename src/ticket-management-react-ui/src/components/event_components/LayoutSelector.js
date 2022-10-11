import React,{Component} from "react";
import { variables, venueMethods } from "../../configures/Variables";
import Cookies from 'js-cookie'
import LocalizedStrings from 'react-localization';
import EventLocalizerData from "../localization_components/EventLocalizerData";

let localizeStr = new LocalizedStrings(EventLocalizerData)
export class LayoutSelector extends Component {

    constructor(props){
        super(props);

        this.state={
            layouts:[],
            name:"",
            id:0,
        };
    }

    refreshList(){
        fetch(variables.VenueApiUrl+venueMethods.GetAll)
        .then(response=>response.json())
        .then(data=>{
            this.setState({layouts:data});
        });
    }

    componentDidMount(){
        this.refreshList();
    }
  
    render() {
        
        const{
            layouts,
        }=this.state;
        localizeStr.setLanguage(Cookies.get("language"))

      return (
        <div className="input-group mb-3">
            <label className="input-group-text">{localizeStr.Layout}: </label>    
            <select className="form-select" value={this.props.layoutId} onChange={this.props.handleChangeLayoutId}>
            <option></option>
            {layouts.map(layout=>
              <option value={layout.id}>{layout.name}</option>)}
            </select>
        </div>
      );
  }
}