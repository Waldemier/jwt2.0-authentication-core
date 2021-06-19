import React from 'react';
import {useSelector} from "react-redux";

const User = () => {

    const { role } = useSelector(store => {
        return {
            role: store.users.role
        }
    })

    return role === "User" ?
        (
            <div>
                <h1>User page</h1>
            </div>
        )
        :
        (
            <div>
                <h1 style={{color: "red"}}>Access denied</h1>
            </div>
        )
};

export default User;