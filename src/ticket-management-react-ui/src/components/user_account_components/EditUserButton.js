import React from 'react';

const EditUserButton = ({children, ...props}) => {
    return(
        <button data-bs-toggle="modal" data-bs-target="#exampleModal"
        {...props} type="button" className="btn btn-dark col-md-auto">
            {children}
            {props.text}
        </button>
    )
}
export default EditUserButton;