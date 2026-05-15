import { CONFIG } from "../configURLs/config.js";
import { baseFetch } from "../tools/baseFetch.js";
import CryptoJS from "https://cdn.jsdelivr.net/npm/crypto-js@4.1.1/+esm";

const _secretKey = "+Vr!scCGX%@D_mh*H8rf8M%a2=l*Q$b$";

export const UserDataService = {
    // NOTA: debido a que el proyecto es academico y requiere un repositorio publico para correccion 
    // se guarda aqui el secretKey con un nombre obvio para encontrarlo facilmente
    // En un entorno real de produccion se usarian JWT o al menos , usando local storage, no se subiria el key al repositorio
    // sino que se usaria una variable de entorno (.env)

    saveData: (id,name) => {
        if(!id || !name) return;
        // Encriptar
        const userData = {
            id : id,
            name : name
        }
        const jsonString = JSON.stringify(userData);
        const encryptedData = CryptoJS.AES.encrypt(jsonString, _secretKey).toString();
        sessionStorage.setItem('u_data', encryptedData)
    },
    getData: () => {
        const encryptedData= sessionStorage.getItem('u_data')
        if (!encryptedData) return null;
        
        try {
            // Intentar desencriptar
            const bytes = CryptoJS.AES.decrypt(encryptedData, _secretKey);
            const jsonString = bytes.toString(CryptoJS.enc.Utf8);
            
            // Retornar el json de datos (o null si la clave falló)
            return JSON.parse(jsonString) || null;
        } catch (error) {
            console.error("Error al recuperar datos de sesion:", error);
            return null;
        }
    },
    clearData: () => {
        sessionStorage.removeItem('u_data');
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
    },
    async GetUserReservationsById(id) {
        const url = CONFIG.ROUTES.USER.GET_RESERVATIONS(id);
        return await baseFetch(url, 'GET'); 
    }
};