
const BASE_API = 'https://localhost:7001/api/v1';

export const CONFIG = {
    BASE_URL: BASE_API,

    ROUTES: {
        // Rutas del UserController
        USER: {
            REGISTER:    `${BASE_API}/users`,           // POST CreateUser
            VALIDATE:   (name,password) =>  `${BASE_API}/users/validate?name=${encodeURIComponent(name)}&password=${encodeURIComponent(password)}`,  // GET ValidateUserCredentials
            BY_ID:  (id) => `${BASE_API}/users/${id}`    // GET GetUserById
        },

        // Rutas del EventController
        EVENT: {
            GET_CATALOG:  (page = 1, size = 10, sort = 'Newest') => 
                `${BASE_API}/events/active?
                            PageNumber=${page}&
                            PageSize=${size}&
                            SortBy=${sort}`,    // GET GetActiveEvents
        },

        // Rutas del ReservationController
        RESERVATION: {
        }
    }
};