import { SeatService } from "../services/seatService.js";
import { EventService } from "../services/eventService.js";
import { ReservationService } from "../services/reservationService.js";
import { Toast } from "../tools/toast.js";
import { UserDataService } from "../services/userService.js";
import { initUserButtonModule } from "../modules/userButtonModule.js";

class SeatMapView {

    constructor() {
        this.container = document.getElementById("seatMap");
        this.containerUser = document.getElementById("userContainer");
        const params = new URLSearchParams(window.location.search);
        this.eventId = params.get("eventId") || 1;
        this.initBackButton();
        initUserButtonModule(this.containerUser, false);
        //this.renderUserButton();
        this.init();
    }

    async init() {
        try {
            await this.loadEventData();
        }catch (error) {
            console.error(error);
            Toast.show("No se pudo cargar el evento", "error");
        }
    }

    async loadEventData() {
        const eventsResponse = await EventService.GetActiveEvents(1, 50, "Newest");
        const events = eventsResponse.events || [];
        const event = events.find(e => Number(e.id) === Number(this.eventId));
        if (event) {
            this.renderEventInfo(event);
        }
        const sectors = await EventService.GetSectorsByEventId(this.eventId);
        await this.renderSectors(sectors);
    }

    renderEventInfo(event) {
        document.getElementById("eventTitle").textContent = event.name || event.Name;
        document.getElementById("eventDate").textContent = `Fecha: ${new Date(event.eventDate || event.EventDate).toLocaleDateString()}`;
        document.getElementById("eventVenue").textContent = `Lugar: ${event.venue || event.Venue}`;
    }

    async renderSectors(sectors) {
        console.log("RENDER TOTAL DE SECTORES");
        this.container.innerHTML = "";
        for (const sector of sectors) {
            const firstRowSeats = await SeatService.GetSeatsBySector(sector.id, true);
            const sectorWrapper = document.createElement("div");
            sectorWrapper.className = "sector-container";
            const title = document.createElement("h2");
            title.className = "sector-title";
            const price = sector.price || sector.Price || 0;
            title.textContent = `Sector: ${sector.name}  -  Precio: $${Number(price).toLocaleString("es-AR")}`;
            const scrollWrapper = document.createElement("div");
            scrollWrapper.className = "sector-scroll";
            const content = document.createElement("div");
            this.renderSeatRow(content, firstRowSeats, sector, () => expanded);
            scrollWrapper.appendChild(content);
            sectorWrapper.appendChild(title);
            sectorWrapper.appendChild(scrollWrapper);
            const button = document.createElement("button");
            button.className = "show-more-btn";
            button.textContent = "Ver más";
            let expanded = false;
                button.addEventListener("click", async () => {
                    content.innerHTML = "";
                    if (!expanded) {
                        const updatedSeats = await SeatService.GetSeatsBySector(sector.id, false);
                        const groupedRows = this.groupSeatsByRow(updatedSeats);
                        const rows = Object.keys(groupedRows).sort((a, b) => {
                        return this.rowIdentifierToNumber(a) - this.rowIdentifierToNumber(b);
                    });
                    rows.forEach(rowKey => {
                        this.renderSeatRow(content, groupedRows[rowKey], sector, () => expanded);
                    });
                    button.textContent = "Ver menos";
                    expanded = true;
                } else {
                    const updatedFirstRowSeats = await SeatService.GetSeatsBySector(sector.id, true);
                    this.renderSeatRow(content, updatedFirstRowSeats, sector, () => expanded);
                    button.textContent = "Ver más";
                    expanded = false;
                }
            });
            sectorWrapper.appendChild(button);
            this.container.appendChild(sectorWrapper);
        }
                    
    }

    renderSeatRow(container, seats, sector, getExpanded) {
        console.log("Render fila sector:", sector.name);
        const row = document.createElement("div");
        row.className = "seat-row";

        seats.forEach(seat => {
            const button = document.createElement("button");
            button.className = `seat seat-${seat.status.toLowerCase()}`;
            button.textContent = `${seat.rowIdentifier}${seat.seatNumber}`;
            if (seat.status !== "Available") {
                button.disabled = true;
            }
            button.addEventListener("click", () => {
                this.selectSeat(seat, button, sector, container, getExpanded) ;
            });
            row.appendChild(button);
        });
        container.appendChild(row);
    }

    groupSeatsByRow(seats) {
        const grouped = {};
        seats.forEach(seat => {
            if (!grouped[seat.rowIdentifier]) {
                grouped[seat.rowIdentifier] = [];
            }
            grouped[seat.rowIdentifier].push(seat);
        });
        return grouped;
    }

    rowIdentifierToNumber(rowIdentifier) {
        let number = 0;
        for (let i = 0; i < rowIdentifier.length; i++) {
            number = number * 26 + (rowIdentifier.charCodeAt(i) - 64);
        }
        return number;
    }

    selectSeat(seat, button, sector, content, getExpanded) {
        const modal = document.getElementById("seatModal");
        const modalText = document.getElementById("seatModalText");
        const confirmBtn = document.getElementById("confirmSeatBtn");
        const cancelBtn = document.getElementById("cancelSeatBtn");
        const seatName = `${seat.rowIdentifier}${seat.seatNumber}`;
        const sectorName = sector.name || sector.Name;
        modalText.textContent = `¿Desea reservar el asiento ${seatName} del sector ${sectorName}?`;
        modal.classList.remove("hidden");
        confirmBtn.onclick = async () => {
            try {
                const userData = UserDataService.getData();
                if (!userData) {
                    Toast.show("Debés iniciar sesión para reservar un asiento", "error");
                    modal.classList.add("hidden");
                    return;
                }
                const userId = userData.id;
                console.log("Seat completo:", seat);
                console.log("seat.id:", seat.id);
                console.log("seat.Id:", seat.Id);
                await ReservationService.CreateReservation(
                    userId,
                    seat.id
                );
                Toast.show(`Reservaste el asiento ${seatName}`,"success");
                modal.classList.add("hidden");
                console.log("Actualizando SOLO sector:", sector.name)
                content.innerHTML = "";
                const onlyRow = !getExpanded();
                const updatedSeats = await SeatService.GetSeatsBySector(sector.id, onlyRow);
                if (getExpanded()) {
                    const groupedRows = this.groupSeatsByRow(updatedSeats);
                    const rows = Object.keys(groupedRows).sort((a, b) => {
                        return this.rowIdentifierToNumber(a) - this.rowIdentifierToNumber(b);
                    });
                    rows.forEach(rowKey => {
                        this.renderSeatRow(content, groupedRows[rowKey], sector, getExpanded);
                    });
                } else {
                    this.renderSeatRow(content, updatedSeats, sector, getExpanded);
                }
            } catch (error) {
                console.error(error);
                Toast.show(error.message || "No se pudo reservar el asiento", "error");
                modal.classList.add("hidden");
            }
        };
        cancelBtn.onclick = () => {
            modal.classList.add("hidden");
        };
    }

    initBackButton() {
        const backButton = document.getElementById("backButton");
        backButton.addEventListener("click", () => {
            const params = new URLSearchParams(window.location.search);
            const from = params.get("from");
            if (from === "index") {
                window.location.href = "../index.html";
            } else {
                window.history.back();
            }
        });
    }

    /*
    getWarningUserHTML() {
        return `
            <a href="../Html/LogView.html"
            class="d-flex align-items-center user-button"
            id="login">
                <div class="user-avatar-container">
                    <img src="../Images/WarningProfile.PNG"
                        alt="Profile"
                        class="user-avatar">
                </div>
                <span class="user-name">LogIn</span>
            </a>
        `;
    }

    getLogedUserHTML(userName) {
        return `
            <a href="../Html/LogView.html"
            class="d-flex align-items-center user-button"
            id="login">
                <div class="user-avatar-container">
                    <img src="../Images/UserProfile.PNG"
                        alt="Profile"
                        class="user-avatar">
                </div>
                <span class="user-name">${userName}</span>
            </a>
        `;
    }

    renderUserButton() {
        const storageData = UserDataService.getData();
        if (!storageData) {
            this.containerUser.innerHTML = this.getWarningUserHTML();
        } else {
            this.containerUser.innerHTML = this.getLogedUserHTML(storageData.name);
        }
    }*/
}
new SeatMapView();