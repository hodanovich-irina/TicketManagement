import React from 'react';
import EventImg from './EventImg';

const EventModel = ({children, ...props}) => {
    return(
        <div className="d-inline-block">
            <div className="modal-content h-100 w-100">
                <EventImg imageURL = {props.imageURL}/>
                <div className="modal-body" >
                    <dl className="dl-horizontal">
                        {children}
                    </dl>
                </div>
            </div>
        </div>
    )
}
export default EventModel;