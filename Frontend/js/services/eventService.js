import { CONFIG } from "../configURLs/config.js";
import { baseFetch } from "../tools/baseFetch.js";

export const EventService = {
    async GetActiveEvents(page, size, sortBy){
        const url = CONFIG.ROUTES.EVENT.GET_CATALOG(page,size,sortBy);
        return await baseFetch(url, 'GET');
    }
}