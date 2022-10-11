import React from 'react';

const NavBar = ({children}) => {
    return(
        <nav className="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
            <div className="container">
                <ul className="navbar-nav">
                    <li className="btn-group nav-item">
                        {children}
                    </li>
                </ul>
            </div>
        </nav>
    )
}
export default NavBar;
