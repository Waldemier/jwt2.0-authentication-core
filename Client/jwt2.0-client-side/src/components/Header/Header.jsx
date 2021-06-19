import React from 'react';
import { Link } from 'react-router-dom';
import './header.scss';
import { useSelector, useDispatch } from "react-redux";
import { useHistory } from "react-router-dom";
import axios from "axios";

import getCookie from '../../Helpers/getCookie';

import { removeRole } from '../../redux/actions/user';

function HeaderBase() {
    let history = useHistory();
    const { role } = useSelector(({ users }) => users);
    const dispatch = useDispatch();

    const logoutHandler = () => {
        axios.post("https://localhost:5001/api/auth/logout", null, {
            headers: {
                "Authorization": `Bearer ${getCookie("access_token")}`
            },
            withCredentials: true
        })
            .then(_ => console.log("User exit was successful."))
            .catch(err => console.error(err));

        dispatch(removeRole());
        history.push("/login");
    }

    return (
        <div className="header">
            <div className="header__nav">
                <ul className="header__nav__ul">
                    { role && <li><Link to="/admin">Admin</Link></li> }
                    { role && <li><Link to="/manager">Manager</Link></li> }
                    { role && <li><Link to="/user">User</Link></li> }
                </ul>
            </div>
            <div className="header__nav" style={{"margin-left": 55}}>
                <ul className="header_nav_auth__ul">
                    { role === null && <li><Link to="/login">Login</Link></li>}
                    { role === null && <li><Link to="/register">Register</Link></li>}
                    { role !== null && <li><button type="primary" shape="round" size="large" onClick={logoutHandler}>Logout</button></li>}
                </ul>
            </div>
        </div>
    )
}

export default HeaderBase
