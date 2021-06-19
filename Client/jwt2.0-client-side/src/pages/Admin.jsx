import React from 'react';
import axios from 'axios';
import { useDispatch, useSelector } from 'react-redux';
import getCookie from '../Helpers/getCookie';

import { addWeaters } from "../redux/actions/weather";
import { removeRole } from "../redux/actions/user";

import refreshTokens from '../Helpers/refreshTokens.cs'

import { useHistory } from "react-router-dom";

const Admin = () => {
    let history = useHistory();
    const dispatch = useDispatch();
    const { weathers, role } = useSelector(store => {
        return {
            weathers: store.weathers.items,
            role: store.users.role
        }
    });

    React.useEffect(() => {
        console.log("USE EFFECT ADMIN") // shows every time that the weathers are changes
    }, [weathers])

    const getWeatherHandler = () => {
        axios.get("https://localhost:5001/api/weatherforecast", {
            headers: {
                "Authorization": `Bearer ${getCookie("access_token")}`
            },
            withCredentials: true
        })
            .then(res => {
                dispatch(addWeaters(res.data))
            })
            .catch(err => {
                if(err.response.status == 401) { // if unauthorized => refresh
                    const success = refreshTokens();
                    if(success) {
                       return getWeatherHandler();
                    }
                    console.log("Refresh token was expired.")

                    dispatch(removeRole());
                    history.push("/login");
                }
                else {
                    console.error(err)
                }
            })
    }
    return role === "Admin" ?
                (
                    <div>
                        <h1>Admin page</h1>
                        {weathers && weathers.map((weather, index) => {
                            return <h1 key={index + Math.random(100)}>{weather.summary}</h1>
                        })}
                        <button onClick={getWeatherHandler}>Weather</button>
                    </div>
                )
            :
            (
                <div>
                    <h1 style={{color: "red"}}>Access denied</h1>
                </div>
            )

};

export default Admin;