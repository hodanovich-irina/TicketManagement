import React,{Component} from "react";
import Cookies from 'js-cookie'

export class LanguageSelector extends Component {
    constructor(props){
        
        super(props);
        let lang = Cookies.get("language")

        this.state={
            language: lang === undefined ? "en" : lang
        }
        this.handleChangeLanguage = this.handleChangeLanguage.bind(this);
    }
    handleChangeLanguage(e){
        e.preventDefault()
        let lang = e.target.value;
        Cookies.set("language",lang);
        this.setState(prState => ({
            language: lang
        }));

        window.location.reload();
    }
    render() {
      return (
        <div className="user-info-block me-3">
            <select className="form-select mb-2 me-5 m-2" value={Cookies.get("language")} onChange={this.handleChangeLanguage}>
              <option value="en">English</option>
              <option value="ru">русский</option>
              <option value="be-BY">белорусский</option>
            </select>
        </div>
      );
  }
}