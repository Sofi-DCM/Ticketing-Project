import { UserDataService } from "../services/userService.js";

//Modulo global, cargado en todas las paginas html, se despierta si hay algo en el localstorage
function initGlobalTimer() {
    const superContainer = document.querySelector(".timer-container");
    const container = document.getElementById("timerContainer");
    if (!container) return;

    function update() {
        if(UserDataService.getData() == null){
            localStorage.removeItem('activeReservations');
            superContainer.style.display = "none";
            return;
        }
        const reservations = JSON.parse(localStorage.getItem('activeReservations') || "[]");
        const now = Date.now();
        
        const valid = reservations.filter(t => t.endTime > now);
        
        if (valid.length !== reservations.length) {
            localStorage.setItem('activeReservations', JSON.stringify(valid));
            window.dispatchEvent(new CustomEvent('reservationExpired'));
            console.log("algo expiró");
        }

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

    setInterval(update, 1000);
    update();
}

document.addEventListener('DOMContentLoaded', initGlobalTimer);
