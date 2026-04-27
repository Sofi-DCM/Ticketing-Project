import { CONFIG } from "../configURLs/config.js";
import { baseFetch } from "../tools/baseFetch.js";

export const EventDataService = {
    saveData: (pageNumber, sortBy) => {
        if(!pageNumber || !sortBy) return;

        const data = {
            pageNumber : pageNumber,
            sortBy : sortBy
        };
        const jsonString = JSON.stringify(data);
        sessionStorage.setItem('page_data', jsonString);
    },
    getData: () => {
        const data = sessionStorage.getItem('page_data');
        return data ? JSON.parse(data) : null;
    },
    clearData: () => {
        sessionStorage.removeItem('page_data');
    } 
}

export const EventService = {
    async GetActiveEvents(page, size, sortBy){
        const url = CONFIG.ROUTES.EVENT.GET_CATALOG(page,size,sortBy);
        return await baseFetch(url, 'GET');
    }
}