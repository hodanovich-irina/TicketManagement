import React from 'react';

const AddEventButton = ({children, ...props}) => {
    return (
        <button type="button" className="btn btn-dark"
        data-bs-toggle="modal"
        data-bs-target="#exampleModal" {...props}>
            {children}
            {props.but_name}
        </button>
    )
}
export default AddEventButton;