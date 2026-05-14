import { Toast } from "../tools/toast.js";
import { UserService, UserDataService } from "../services/userService.js";
import { ReservationService } from "../services/reservationService.js";

window.addEventListener('reservationExpired', async () => {
    console.log("Cambio en reservas detectado desde esta pestaña. Actualizando mapa...");
})

class Payment {
    constructor(){
        this.paymentContainer = document.getElementById("paymentContainer");
        this.tableContainer = document.getElementById("tableContainer");
        this.tableBody = document.getElementById("tableBody");
        this.payButton = this.paymentContainer.querySelector("#payButton");
        this.init();
    }

    init(){
        if (UserDataService.getData() == null) {
            if (document.referrer) {
                window.location.href = document.referrer; //va directo a la pagina anterior
            } else {
                window.location.href = "index.html"; // Backup por si no hay historial
            }
        }
        if(!this.tableBody || !this.paymentContainer) Toast.show("error al cargar recursos", "error");
        this.initListeners();
        this.renderReservations();
    }
    initListeners(){
        this.payButton.addEventListener('click', () => {
            this.handlePayment();
        });
        window.addEventListener('reservationExpired', async () => {
            this.renderReservations();
        })
    }

    renderReservations(){
        const reservations = JSON.parse(localStorage.getItem('activeReservations') || "[]");
        this.tableBody.innerHTML = '';
        if(reservations.length){
            console.log(reservations);
            for(const r of reservations){
                this.tableBody.innerHTML += this.getReservationHtml(r);
            }
        } 
        else{
            Toast.show("Carrito vacio", "info");
            this.tableBody.innerHTML = '<tr><td colspan="4" style="text-align:center;">No hay reservas</td></tr>';
        }
    }

    getReservationHtml(r){
        return `
            <tr>
                <td>${r.eventName}</td>
                <td>${r.sector}</td>
                <td style="text-align: center;">${r.name}</td>
                <td id="timer" style="color: var(--color-strong-pink);text-align:center;"></td>
            </tr> 
        `
    }

    async handlePayment(){
        try{
            const user = UserDataService.getData();
            console.log(user.id);
            const reservations = JSON.parse(localStorage.getItem('activeReservations') || "[]");
            if(reservations.length){
                const reservationsIds = reservations.map( r => r.reservationId);
                for (const r of reservationsIds){
                    await ReservationService.PayReservation(r, user.id);
                }
                Toast.show("Pagos realizados");
            }
        }
        catch(error){
            Toast.show(error.message, "error");
        }

    }
}

new Payment();