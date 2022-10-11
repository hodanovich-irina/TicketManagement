import React from 'react';

const EditPasswordButton = ({children, ...props}) => {
    return(
        <button data-bs-toggle="modal" data-bs-target="#passwordModal"
        {...props} type="button" className="btn btn-dark col-md-auto">
            {children}
            {props.text}
        </button>
    )
}
export default EditPasswordButton;