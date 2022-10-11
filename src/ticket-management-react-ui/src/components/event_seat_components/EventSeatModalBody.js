import React from 'react';

const EventSeatModalBody = ({...props}) => {
    return(<div className="modal-body">
    <p className="mb-1">
        <dl className="dl-horizontal">
            <dt>Cобытие:</dt>
            <dd>{props.event}</dd>

            <dt>Зона:</dt>
            <dt>Координата X:</dt>
            <dd>{props.coordX}</dd>

            <dt>Координата Y:</dt>
            <dd>{props.coordY}</dd>

        </dl>
    </p>
</div>)
}
export default EventSeatModalBody;