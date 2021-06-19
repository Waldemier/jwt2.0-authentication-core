import React from 'react';
import { useSelector } from "react-redux";

const Manager = () => {
    const { role } = useSelector(store => {
        return {
            role: store.users.role
        }
    })

    return role === "Manager" ?
        (
            <div>
                <h1>Manager page</h1>
            </div>
         )
        :
        (
            <div>
                <h1 style={{color: "red"}}>Access denied</h1>
            </div>
        )
};

export default Manager;