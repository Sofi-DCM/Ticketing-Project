import { UserDataService } from "../services/userService.js";
import { Toast } from "./toast.js";
import { ReservationTimerService } from "../services/reservationService.js";

function initGlobalTimer() {
    const superContainer = document.querySelector(".timer-container");
    const container = document.getElementById("timerContainer");
    const cartTableBody = document.getElementById("tableBody");

    function update() {
        if (UserDataService.getData() == null) {
            localStorage.removeItem('activeReservations');
            if (superContainer) superContainer.style.display = "none";
            return;
        }
        const reservations = JSON.parse(localStorage.getItem('activeReservations') || "[]");
        const now = Date.now();

        const valid = reservations.filter(t => t.endTime > now);

        if (valid.length !== reservations.length) {
            ReservationTimerService.UpdateReservations(valid);

            const expired = reservations.filter(t => t.endTime <= now);
            for (const t of expired) {
                Toast.show(`Expiró reserva de asiento ${t.name}`, "info");
            }
            window.dispatchEvent(new CustomEvent('reservationExpired'));
        }
        
        if (!container && !superContainer && !cartTableBody) return; 

        if( container && superContainer) {
            if (valid.length === 0) {
                container.innerHTML = "";
                superContainer.style.display = "none";
                return;
            }

            superContainer.style.display = "flex";
            container.innerHTML = valid.map(t => {
                const diff = Math.round((t.endTime - now) / 1000);
                const m = Math.floor(diff / 60);
                const s = diff % 60;
                return `<div class="badge d-flex timer-custom m-1"><em>Asiento ${t.name} -</em><strong> ${m}:${s < 10 ? '0' : ''}${s}</strong></div>`;
            }).join('');
        }
        else if(cartTableBody){
            const timerCells = cartTableBody.querySelectorAll("#timer");
            let i = 0;
            for(const c of timerCells){
                const t = valid[i];
                const diff = Math.round((t.endTime - now) / 1000);
                const m = Math.floor(diff / 60);
                const s = diff % 60;
                c.innerHTML = `${m} : ${s}`;
                i += 1;
            }
        }
        
    }
    setInterval(update, 1000);
    update();
}

document.addEventListener('DOMContentLoaded', initGlobalTimer);