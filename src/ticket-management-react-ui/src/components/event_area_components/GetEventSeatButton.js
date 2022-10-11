import React from 'react';

const GetEventSeatButton = ({children, ...props}) => {
    return(
        <button {...props} type="button" className="btn btn-dark inline-block">
            {children}
            {props.text}
        </button>
    )
}
export default GetEventSeatButton;