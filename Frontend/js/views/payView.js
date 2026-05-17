import { Toast } from "../tools/toast.js";
import { UserService, UserDataService } from "../services/userService.js";
import { ReservationService, ReservationTimerService } from "../services/reservationService.js";
import { lockButton } from "../tools/buttonLock.js";

window.addEventListener('reservationExpired', async () => {
    console.log("Cambio en reservas detectado desde esta pestaña. Actualizando mapa...");
})

class Payment {
    constructor(){
        this.paymentContainer = document.getElementById("paymentContainer");
        this.tableContainer = document.getElementById("tableContainer");
        this.tableBody = document.getElementById("tableBody");
        this.payButton = this.paymentContainer.querySelector("#payButton");
        this.totalAmount = document.getElementById("totalAmount");
        this.init();
    }

    init(){
        if (UserDataService.getData() == null) {
            if (document.referrer) {
                window.location.href = document.referrer; 
            } else {
                window.location.href = "../../index.html"; 
            }
        }
        if(!this.tableBody || !this.paymentContainer) Toast.show("error al cargar recursos", "error");
        this.initListeners();
        this.renderReservations();
    }
    initListeners(){
        this.payButton.addEventListener('click', (e) => {
            lockButton(e.currentTarget, async () => {
                this.handlePayment();
            }
        );
            
        });
        this.onlyNumbers("cardNumber");
        this.onlyNumbers("cardDni");
        this.onlyNumbers("cardSecurityCode");
        this.formatExpirationDate();
        window.addEventListener('reservationExpired', async () => {
            this.renderReservations();
        })
    }
    onlyNumbers(inputId) {
        const input = document.getElementById(inputId);
        if (!input) 
        {
            console.error(`No existe el input ${inputId}`);
            return;
        }

        input.addEventListener("input", (e) => {
            const originalValue = e.target.value;
            e.target.value = originalValue.replace(/[^0-9]/g, "");
        });
    }

    formatExpirationDate() {
        const input = document.getElementById("cardExpiration");
        input.addEventListener("input", () => {
            let value = input.value.replace(/\D/g, "");
            if (value.length >= 3) 
            {
                value = value.slice(0, 2) + "/" + value.slice(2, 4);
            }
            input.value = value;
        });
    }

    initCancelButtonListeners(){
        this.tableBody.querySelectorAll('.button-delete-event').forEach(button => {
            button.addEventListener('click', async (e) => {
                await lockButton(e.currentTarget, async () =>
                {
                    const reservationId = button.id; // Es mejor usar la referencia directa al botón
                    const tr = button.closest('tr');
                    const seatCell = tr.querySelector('.seatName');
                    const seatName = seatCell.innerHTML;
                    
                    await this.handleCancelReservation(reservationId, seatName);
                });

            })
        });
    }

    renderReservations(){
        const reservations = ReservationTimerService.GetReservations();
        this.tableBody.innerHTML = '';
        if(reservations.length){
            console.log(reservations);
            for(const r of reservations){
                this.tableBody.innerHTML += this.getReservationHtml(r);
            }
            this.initCancelButtonListeners();
        } 
        else{
            this.tableBody.innerHTML =
                '<tr><td colspan="6" style="text-align:center;">No hay reservas</td></tr>';
        }
        this.calculateTotal();
    }
    calculateTotal() {
        const reservations = ReservationTimerService.GetReservations();
        const total = reservations.reduce((sum, reservation) => {
            const price = Number(
                reservation.sectorPrice
                    .toString()
                    .replace(/\./g, "")
                    .replace(",", ".")
            );
            return sum + price;
        }, 0);

        this.totalAmount.textContent =
            `$${total.toLocaleString("es-AR")}`;
    }

    validatePaymentForm() {
        const cardNumber = document.getElementById("cardNumber").value.trim();
        const cardHolder = document.getElementById("cardHolder").value.trim();
        const cardDni = document.getElementById("cardDni").value.trim();
        const cardExpiration = document.getElementById("cardExpiration").value.trim();
        const cardSecurityCode = document.getElementById("cardSecurityCode").value.trim();
        const termsCheckbox = document.getElementById("termsCheckbox");

        if (!/^\d{13,19}$/.test(cardNumber)) {
            Toast.show("El número de tarjeta debe tener entre 13 y 19 dígitos", "error");
            return false;
        }

        if (cardHolder.length < 3) {
            Toast.show("Ingresá el nombre completo del titular", "error");
            return false;
        }

        if (!/^\d{7,8}$/.test(cardDni)) {
            Toast.show("El DNI debe tener 7 u 8 dígitos", "error");
            return false;
        }

        if (!/^(0[1-9]|1[0-2])\/\d{2}$/.test(cardExpiration)) {
            Toast.show("La fecha de vencimiento debe tener formato MM/AA", "error");
            return false;
        }

        if (!/^\d{3,4}$/.test(cardSecurityCode)) {
            Toast.show("El código de seguridad debe tener 3 o 4 dígitos", "error");
            return false;
        }

        if (!termsCheckbox.checked) {
            Toast.show("Debés aceptar los términos y condiciones", "error");
            return false;
        }
        return true;
    }
//agregar listeners a botones de liberar reservas
    getReservationHtml(r){
        return `
            <tr>
                <td class="cell-text">${r.eventName}</td>
                <td class="cell-text">${r.sectorName}</td>
                <td style="text-align: center;">${r.sectorPrice}</td>
                <td class="seatName" style="text-align: center;">${r.seatName}</td>
                <td id="timer" style="color: var(--color-strong-pink);text-align:center;"></td>
                <td style="text-align:center;"><button id="${r.reservationId}" class="btn rounded-pill button-delete-event">-</button></td>
            </tr> 
        `
    }

    async handlePayment(){
        if (!this.validatePaymentForm()) {
            return;
        }
        try{
            const user = UserDataService.getData();
            const reservations = ReservationTimerService.GetReservations();
            if(!reservations.length)
            {
                Toast.show("Nada para pagar", "info");
                return;
            }
            let paidCount = 0;
            let failedCount = 0;
            for (const r of reservations)
            {
                try
                {
                    await ReservationService.PayReservation(r.reservationId, user.id);
                    ReservationTimerService.DeleteReservation(r.reservationId);
                    paidCount++;
                }catch (error) {
                    failedCount++;
                    console.error(error);
                    if (
                        error.status === 400 ||
                        error.status === 404 ||
                        error.status === 409
                    ) {
                        Toast.show(`La reserva ${r.seatName} expiró o ya fue pagada`,"error");
                        ReservationTimerService.DeleteReservation(r.reservationId);
                    }
                    else {
                        Toast.show(error.message, "error");
                    }

                }
            }
            this.renderReservations();
            if(paidCount > 0)
            {
                Toast.show(`${paidCount} asiento(s) pagado(s) correctamente`, "success");
            }
            if (failedCount > 0) 
            {
                Toast.show(`${failedCount} pago(s) fallaron`,"error");
            }
            window.dispatchEvent(new Event("reservationExpired"));
        }
        catch(error){
            Toast.show(error.message, "error");
        }
    }

    async handleCancelReservation(reservationId, seatName){
        try{
            const userData = UserDataService.getData();
            await ReservationService.CancelReservation(reservationId, userData.id);
            ReservationTimerService.DeleteReservation(reservationId);
            Toast.show(`Reserva asiento ${seatName} cancelada`);
            this.renderReservations();
        }
        catch(error){
            Toast.show(error.message, "error");
        }
    }
}
new Payment();