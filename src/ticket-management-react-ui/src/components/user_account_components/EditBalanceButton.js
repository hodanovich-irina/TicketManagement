import React from 'react';

const EditBalanceButton = ({children, ...props}) => {
    return(
        <button data-bs-toggle="modal" data-bs-target="#balanceModal"
        {...props} type="button" className="btn btn-dark col-md-auto">
            {children}
            {props.text}
        </button>
    )
}
export default EditBalanceButton;