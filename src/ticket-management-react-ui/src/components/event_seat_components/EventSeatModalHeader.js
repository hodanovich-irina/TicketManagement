import React from 'react';

const EventSeatModalHeader = (props) => {
    return(<div className="modal-header">
        <h5 className="modal-title" id="exampleModalLabel">{props.text}</h5>
        <button type="button" className="close" data-dismiss="modal" aria-label="Close">
            <span aria-hidden="true">x</span>
        </button>
        </div>)
}
export default EventSeatModalHeader;