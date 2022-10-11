import React from 'react';
const PresentationRedirect = ({children, ...props}) => {
    return(
        <>
        <button {...props} type="button" className="btn btn-light float-right ms-1 rounded">
            {children}
            {props.text}
        </button></>
    )
}
export default PresentationRedirect;