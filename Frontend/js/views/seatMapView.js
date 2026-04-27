import { SeatService } from "../services/seatService.js";
import { Toast } from "../tools/toast.js";

class SeatMapView {
    constructor() {
        this.container = document.getElementById("seatMap");

        // fijo para probar.
        this.sectorId = 1;

        if (!this.container) {
            console.error("No se encontró el contenedor #seatMap");
            return;
        }

        this.init();
    }

    async init() {
        await this.loadSeats();
    }

    async loadSeats() {
        try {
            const seats = await SeatService.GetSeatsBySector(this.sectorId);
            this.renderSeats(seats);
        } catch (error) {
            console.error(error);
            Toast.show("No se pudieron cargar los asientos", "error");
        }
    }

    renderSeats(seats) {
        this.container.innerHTML = "";

        if (!seats || seats.length === 0) {
            this.container.innerHTML = "<p>No hay asientos para mostrar.</p>";
            return;
        }

        seats.forEach(seat => {
            const button = document.createElement("button");

            button.textContent = `${seat.rowIdentifier}${seat.seatNumber}`;

            button.className = `seat seat-${seat.status.toLowerCase()}`;

            button.title = `Asiento ${seat.rowIdentifier}${seat.seatNumber} - ${seat.status}`;

            if (seat.status !== "Available") {
                button.disabled = true;
            }

            button.addEventListener("click", () => {
                this.selectSeat(seat, button);
            });

            this.container.appendChild(button);
        });
    }

    selectSeat(seat, button) {
        document.querySelectorAll(".seat-selected")
            .forEach(btn => btn.classList.remove("seat-selected"));

        button.classList.add("seat-selected");

        Toast.show(`Seleccionaste el asiento ${seat.rowIdentifier}${seat.seatNumber}`, "success");

        console.log("Asiento seleccionado:", seat);
    }
}

const app = new SeatMapView();