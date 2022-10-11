import React from 'react';

const PurchaseHistoryButton = ({children,...props}) =>{
    return(
        <button type="button" className="btn btn-dark col-md-auto">
            {children}
            {props.text}
        </button>)
}
export default PurchaseHistoryButton;