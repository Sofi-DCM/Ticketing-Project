import { CONFIG } from "../configURLs/config.js";
import { baseFetch } from "../tools/baseFetch.js";

export const ReservationService = {
    async CreateReservation(userId, seatId) {
        const body = { userId: userId, seatId: seatId };
        return await baseFetch(
            CONFIG.ROUTES.RESERVATION.CREATE,
            "POST",
            body
        );
    }
};