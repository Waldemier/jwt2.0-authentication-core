import { combineReducers } from 'redux';
import user from './user';
import weather from './weather';

const rootReducer = combineReducers({
    users: user,
    weathers: weather
});

export default rootReducer;