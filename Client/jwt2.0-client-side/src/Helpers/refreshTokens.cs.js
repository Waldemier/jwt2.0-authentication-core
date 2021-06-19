import axios from 'axios';

export default async () => {
    let success = false;
    await axios.post("https://localhost:5001/api/auth/refresh", null, { withCredentials: true }) // sends request with the cookies
        .then(_ => {
            console.log("Tokens successfully updated", _)
            success = true;
        })
        .catch(error => console.error(error))
    return success;
}