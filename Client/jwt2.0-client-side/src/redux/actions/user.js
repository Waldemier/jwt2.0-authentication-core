export const addRole = role => ({
    type: 'SET_ROLE',
    payload: role
})

export const removeRole = () => ({
    type: 'REMOVE_ROLE',
})