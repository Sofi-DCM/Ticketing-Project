import { CONFIG } from "../configURLs/config.js";
import { baseFetch } from "../tools/baseFetch.js";

export const UserIdService = {
    saveId: (id) => localStorage.setItem('userId', id),
    getId: () => {
        const id = localStorage.getItem('userId');
        return id ? id : null; 
    }
};

export const UserService = {

    async CreateUser(createUserCommand) {
        const url = CONFIG.ROUTES.USER.REGISTER;
        return await baseFetch(url, 'POST', createUserCommand);
    },
    async ValidateUserCredentials(name, password) {
        const url = CONFIG.ROUTES.USER.VALIDATE(name, password);
        return await baseFetch(url, 'GET');
    },
    async GetUserById(id) {
        const url = CONFIG.ROUTES.USER.BY_ID(id);
        return await baseFetch(url, 'GET');
    }
};