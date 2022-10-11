import React from 'react';

const EventAreaTableBody = ({children, ...props}) => {
    return (
            <tr>
                <td>{props.coordX}</td>
                <td>{props.coordY}</td>
                <td>{props.description}</td>
                <td>{props.name}</td>
                <td>{props.price}</td>
                <td>
                    {children}
                </td>
            </tr>
    )
}
export default EventAreaTableBody;