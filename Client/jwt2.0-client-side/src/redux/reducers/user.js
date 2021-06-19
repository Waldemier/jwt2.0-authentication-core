const initialState = {
    role: null
}

export default (state = initialState, action) => {
    switch (action.type)
    {
        case 'SET_ROLE':
            return {
                ...state,
                role: action.payload
            }
        case 'REMOVE_ROLE':
            return {
                ...state,
                role: null
            }
        default:
            return state;
    }

}