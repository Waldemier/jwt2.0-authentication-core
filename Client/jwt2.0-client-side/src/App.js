import React from 'react';
import './App.scss';
import { Header } from './components';
import { Route } from 'react-router-dom';
import { Login, Register, User, Admin, Manager } from './pages';
import { useSelector } from 'react-redux';
import { useHistory } from "react-router-dom";
import NotFound from "./pages/NotFound";

function App() {

    let history = useHistory();
    const {role} = useSelector(({users}) => users);

    React.useEffect(() => {
        if (!role) {
            history.push("/login");
        }
    }, [])

    React.useEffect(() => {
        if (role) {
            history.push(`/${role}`);
        }
    })

    return (
        <div className="App">
            <Header/>
            {!role ? <Route path="/login" component={Login}/> : <Route path="/login" component={NotFound}/>}
            {!role ? <Route path="/register" component={Register}/> : <Route path="/register" component={NotFound}/>}
            {role ? <Route path="/user" component={User}/> : <Route path="/user" component={NotFound}/>}
            {role ? <Route path="/manager" component={Manager}/> : <Route path="/manager" component={NotFound}/>}
            {role ? <Route path="/admin" component={Admin}/> : <Route path="/admin" component={NotFound}/>}
        </div>
    );
}

export default App;
