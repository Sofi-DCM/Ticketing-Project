
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
            GET_CATALOG:  (page, size, sort) => 
                `${BASE_API}/events?pageNumber=${page}&pageSize=${size}&sortBy=${sort}`,    // GET GetActiveEvents
            POST:  `${BASE_API}/events`,
        },

        //Rutas del SectorController
        SECTOR: {
            GET_BY_EVENT_ID: (eventId) =>
                `${BASE_API}/sectors/${eventId}`
        },

        // Rutas del SeatController
        SEAT: {
            GET_BY_SECTOR: (sectorId) =>
                `${BASE_API}/seats/${sectorId}`
        },

        // Rutas del ReservationController
        RESERVATION: {
            CREATE: `${BASE_API}/reservations`
        }
    }
};