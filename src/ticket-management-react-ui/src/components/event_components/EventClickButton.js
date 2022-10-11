import React from 'react';

const EventClickButton = ({children, ...props}) => {
    return(
        <button type="button" className="btn btn-primary float-start">
            {children}
            {props.text}
        </button>
    )
}
export default EventClickButton;