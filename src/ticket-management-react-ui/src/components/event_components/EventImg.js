import React from 'react';

const EventImg = (props) =>{
    return(
        <div className="modal-header">
            <h6><img height="450" width="300" src={props.imageURL} /></h6>
        </div>
    ) 
} 
export default EventImg;