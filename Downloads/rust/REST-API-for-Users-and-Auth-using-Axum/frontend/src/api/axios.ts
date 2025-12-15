import axios from 'axios';

const BASE_URL = 'http://localhost:8080/api';

export const api = axios.create({
    baseURL: BASE_URL,
    withCredentials: true,
});

api.defaults.headers.common['Content-Type'] = 'application/json';

export const setAuthToken = (token: string | null) => {
    if (token) {
        api.defaults.headers.common['Authorization'] = `Bearer ${token}`;
    } else {
        delete api.defaults.headers.common['Authorization'];
    }
};
