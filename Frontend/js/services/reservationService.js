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
        const body = userId;
        const url = CONFIG.ROUTES.RESERVATION.PAY(reservationId);
        return await baseFetch(url, "POST", body);
    },
    async CancelReservation(reservationId, userId){
        const body = userId;
        const url = CONFIG.ROUTES.RESERVATION.CANCEL(reservationId);
        return await baseFetch(url, 'PATCH', body);
    }
};

export const ReservationTimerService = {
    UpdateReservations(reservations){
        if(!reservations) return;
        sessionStorage.setItem('activeReservations', JSON.stringify(reservations));
    },
    GetReservations() {
        return  JSON.parse(sessionStorage.getItem('activeReservations') || "[]");
    },
    ClearReservations(){
        sessionStorage.removeItem('activeReservations');
    },
    DeleteReservation(reservationId){
        const reservations = this.GetReservations();
        const valid = reservations.filter(t => t.reservationId != reservationId);
        this.UpdateReservations(valid);
    }
}