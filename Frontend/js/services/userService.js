import { CONFIG } from "../configURLs/config.js";
import { baseFetch } from "../tools/baseFetch.js";

const _secretKey = "+Vr!scCGX%@D_mh*H8rf8M%a2=l*Q$b$";

export const UserIdService = {
    // NOTA: debido a que el proyecto es academico y requiere un repositorio publico para correccion 
    // se guarda aqui el secretKey con un nombre obvio para encontrarlo facilmente
    // En un entorno real de produccion se usarian JWT o al menos , usando local storage, no se subiria el key al repositorio
    // sino que se usaria una variable de entorno (.env)

    saveId: (id) => {
        if(!id) return;
        // Encriptar
        const encryptedId = CryptoJS.AES.encrypt(id.toString(), _secretKey).toString();
        localStorage.setItem('u_data', encryptedId)
    },
    getId: () => {
        const encryptedId = localStorage.getItem('u_data')
        if (!encryptedId) return null;
        
        try {
            // Intentar desencriptar
            const bytes = CryptoJS.AES.decrypt(encryptedId, _secretKey);
            const originalId = bytes.toString(CryptoJS.enc.Utf8);
            
            // Retornar el ID plano (o null si la clave falló)
            return originalId || null;
        } catch (error) {
            console.error("Error al recuperar el ID:", error);
            return null;
        }
    },
    clearId: () => {
        localStorage.removeItem('u_data');
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