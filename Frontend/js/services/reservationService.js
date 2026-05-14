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
    },
    async PayReservation(reservationId, userId) {
        const body = {userId};
        const url = CONFIG.ROUTES.RESERVATION.PAY(reservationId);
        return await baseFetch(url, "POST", body);
    }
};