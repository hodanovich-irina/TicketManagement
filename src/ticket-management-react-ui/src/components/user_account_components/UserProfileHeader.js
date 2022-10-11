import React from 'react';

const UserProfileHeader = ({children, ...props}) => {
    return(<div className="text-center">
    <h1 className="display-4">{props.text}</h1>
    <h6>{props.email}</h6>
    {children}
    </div>)
}
export default UserProfileHeader;