import './App.css';
import {Event} from './components/Event';
import {UserAccount} from './components/UserAccount';
import {BrowserRouter, Route, Routes, NavLink} from 'react-router-dom';
import Footer from './components/app_components/Footer';
import Header from './components/app_components/Header';
import NavBar from './components/app_components/NavBar';
import GetParamsForEventArea from './components/GetParamsForEventArea';
import GetParamsForEventSeat from './components/GetParamsForEventSeat';
import GetParamsForPurchaseHistory from './components/GetParamsForPurchaseHistory';
import { UserRoles } from './components/user_account_components/UserRoles';

function App() {
  return (
    <BrowserRouter>
    <div className="App">
      <Header/>
      <UserRoles/>
      <Routes>
        <Route path ='/' element={<Event/>}/>
        <Route path ='/Event' element={<Event/>}/>
        <Route path ='/UserAccount' element={<UserAccount/>}/>
        <Route path ='/EventArea/:id/:name' element={<GetParamsForEventArea/>}/>
        <Route path ='/EventSeat/:id' element={<GetParamsForEventSeat/>}/>
        <Route path ='/PurchaseHistory/:id' element={<GetParamsForPurchaseHistory/>}/>
      </Routes>
    </div>
    <Footer/>
    </BrowserRouter>
  );
}

export default App;
