import React from 'react';

const EventInputField = ({children, ...props}) => {
    return(
            <div className="input-group mb-3">
                <span className="input-group-text">{props.text}</span>
                {children}
            </div>
    )
}
export default EventInputField;