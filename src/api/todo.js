const axios = require('axios');

axios.defaults.baseURL = "/api";

let api = {
    getTodosByUser: (userid) => {
        const apiUrl = `/GetTodosForUser?userId=${userid}`;
        return axios.get(apiUrl);
    }
};

export { api };