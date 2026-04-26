import { CONFIG } from "../configURLs/config.js";
import { baseFetch } from "../tools/baseFetch.js";

export const SeatService = {
    async GetSeatsBySector(sectorId) {
        const url = CONFIG.ROUTES.SEAT.GET_BY_SECTOR(sectorId);
        return await baseFetch(url, "GET");
    }
};