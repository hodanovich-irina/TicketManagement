import React from 'react';

const RefuseTicketButton = ({children, ...props}) => {
    return(
        <button {...props} type="button" className="btn btn-outline-danger text-dark">
            {children}
            {props.text}
        </button>
    )
}
export default RefuseTicketButton;