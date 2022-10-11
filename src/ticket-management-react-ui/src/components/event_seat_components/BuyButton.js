import React from 'react';

const BuyButton = ({children,...props}) =>{
    return(
        <button {...props} type="button" className="btn btn-dark col-md-auto">
            {children}
            {props.text}
        </button>)
}
export default BuyButton;