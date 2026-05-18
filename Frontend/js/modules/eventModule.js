import { Toast } from "../tools/toast.js";
import { UserDataService, UserService } from "../services/userService.js";
import { EventService } from "../services/eventService.js";
import { lockButton } from "../tools/buttonLock.js";

export function initEventModule(){
    const manager = new eventModule();
    const container = document.getElementById('mainContainer');
    const body = document.getElementById('body');
    body.classList.remove("body-audit");
    body.classList.add("event-mod");
    manager.renderForm(container);
}

class eventModule{
    constructor(){
        this.sectorNumber = 1;
    }

    // #region renderizado 
    renderForm(container){
        //renderiza el formulario basico de evento con al menos un sector obligatorio
        container.innerHTML = this.getEventFormHTML();
        const sectorContainer = container.querySelector('#customForm');
        sectorContainer.innerHTML = this.getSectorFormHTML(this.sectorNumber);
        Toast.show("Abriendo vista de creacion de evento","info");
        this.setupInputControls(container);
        this.initButtonListeners(container);
        this.initSectorInputsListeners(container);
    }

    getEventFormHTML(){
        return `
            <form id="mainForm" class="event-container text-white">
                <div class="d-flex flex-wrap justify-content-between align-items-center m-4 main-event-container">
                    <div class="d-flex flex-column align-items-center justify-content-center text-white px-4 text-obligatory">
                        <h1><strong>Crear</strong></h1>
                        <h1><strong>evento</strong></h1>
                    </div>
                    
                    <div id="customFormEvent" class = "custom-Form-Event">
                        <div class="mb-2"> <!--nombre evento-->
                            <label for="eventName" class="form-label" >Nombre del Evento</label>
                            <input name="name" id="eventName" type="text" class="form-control" placeholder="Super Evento" required>
                        </div>

                        <div class="mb-2"> <!--fecha-->
                            <label for="eventDate" class="form-label">Fecha del Evento</label>
                            <input name="eventDate" type="datetime-local" id="eventDate" class="form-control custom-date-input" required>
                        </div>
                        <div class="mb-2"><!-- direccion -->
                            <label for="venue" class="form-label">Direccion del Evento</label>
                            <input name="venue" id="venue" type="text" class="form-control " placeholder="Un lugar interesante" required>
                        </div>
                    </div>
                    <button id="createButton" type="submit" class="btn fw-bold mb-3 button-custom button-create">CREAR</button>
                </div> 
                <div class="main-sector-container d-flex flex-wrap justify-content-between align-items-center mb-4">
                    <div id="customForm">
                    </div>
                </div>
                <button id="addSector" type="click" class="btn fw-bold mb-3 button-custom button-create-event">Agregar Sector +</button>
                <button id="removeSector" type="click" class="btn fw-bold mb-3 button-custom button-delete-event">Quitar Sector -</button>
            </form>
            `
    }

    getSectorFormHTML(num){
        return `
            <div id="sector_${num}" class="sector-container">
                <div id="sectorBox" class="mb-3 position-relative sector-form">
                    <h2 class="pb-2"><strong><em>Sector ${num} :</em></strong></h2>
                    <div class="mb-2 position-relative">
                        <label for="sectorName" class="form-label">Nombre del Sector</label>
                        <input name="nameSector_${num}" id="sectorName" type="text" class="form-control custom-input" placeholder="Platea Alta" required>
                    </div>
                    <div class="mb-2">
                        <label for="price" class="form-label">Precio</label>
                        <input name="price_${num}" type="number" id="price" class="form-control custom-num-input" placeholder="1" min="1" required>
                    </div>
                    <div class="mb-2">
                        <label for="rowsAmount" class="form-label">Filas</label>
                        <input name="rowsAmount_${num}" type="number" id="rowsAmount" class="form-control custom-num-input rows-input" min="1" step="1" required>
                    </div>
                    <div>
                        <label for="columnsAmount" class="form-label">Columnas</label>
                        <input name="columnsAmount_${num}" type="number" id="columnsAmount" class="form-control custom-num-input cols-input" min="1" step="1" required>
                    </div>
                </div>
                <div class="map-wrapper flex-grow-1">
                    <div id="seatsMap" class=" seats-map-display seat-grid-container"></div>
                </div>
            </div>
        `
    }
    //#endregion

    //#region configurations and listeners
    setupInputControls(container){
        // Bloqueado de fechas pasadas
        const dateInput = container.querySelector('#eventDate');
        if (dateInput) {
                const ahora = new Date();
                
                // Ajustamos la fecha al formato YYYY-MM-DDTHH:mm local
                const anio = ahora.getFullYear();
                const mes = String(ahora.getMonth() + 1).padStart(2, '0');
                const dia = String(ahora.getDate()).padStart(2, '0');
                const horas = String(ahora.getHours()).padStart(2, '0');
                const minutos = String(ahora.getMinutes()).padStart(2, '0');

                const fechaMinima = `${anio}-${mes}-${dia}T${horas}:${minutos}`;
                dateInput.setAttribute('min', fechaMinima);
            }

        // Evitar que la rueda del mouse mueva valores numericos por error
        container.querySelectorAll('input[type="number"]').forEach(input => {
            input.addEventListener('wheel', (e) => {
                e.preventDefault();
            }, { passive: false });
        });
    }

    initButtonListeners(container){
        const createButton = container.querySelector('#mainForm');
        createButton.addEventListener('submit', (e) => {
            
            e.preventDefault();
            console.log("boton crear funciona");
            const form = container.querySelector('#mainForm');
            const data = Object.fromEntries(new FormData(form).entries());
            lockButton(SubmitEvent, async () => {
                await this.mapEventCommand(data);
            });
        });

        const addSectorButton = container.querySelector('#addSector');
        addSectorButton.addEventListener('click', (e) => {
            e.preventDefault();
            console.log("boton mas funciona");
            this.addNewSectorForm(container);
        });

        const removeSectorButton = container.querySelector('#removeSector');
        removeSectorButton.addEventListener('click', (e) => {
            e.preventDefault();
            console.log("boton mas funciona");
            this.removeSectorForm(container);
        });
    }

    initSectorInputsListeners(){
        document.querySelectorAll('.sector-container').forEach(sector => {
            sector.addEventListener('input', (e) => this.handleDrawSectorSeats(e));
        });
    }
    //#endregion

    //#region Individual Sector Control
    addNewSectorForm(container){
        this.sectorNumber +=1;
        const sectorContainer = container.querySelector('#customForm');
        sectorContainer.insertAdjacentHTML('beforeend', this.getSectorFormHTML(this.sectorNumber));
        this.setupInputControls(container);
        this.initSectorInputsListeners();
    }

    removeSectorForm(container){
        const sectorId = `#sector_${this.sectorNumber}`;
        const elementToRemove = container.querySelector(sectorId);
        if(elementToRemove){
            elementToRemove.remove();
            Toast.show(`Sector ${this.sectorNumber} removido`, "info");
            this.sectorNumber -= 1;
        } else {
            Toast.show(`No hay mas sectores a remover`, "error");
        }
        if(this.sectorNumber == 0)Toast.show(`Un evento necesita al menos 1 sector`, "info");
    }

    handleDrawSectorSeats(event) {
        // 1. Encontrar el contenedor principal del sector donde ocurrió el cambio
        const contenedor = event.target.closest('.sector-container');
        
        // 2. Buscar los elementos específicos DENTRO de ese contenedor
        const inputFilas = contenedor.querySelector('.rows-input');
        const inputCols = contenedor.querySelector('.cols-input');
        const mapaDestino = contenedor.querySelector('.seats-map-display');

        const filas = parseInt(inputFilas.value) || 0;
        const columnas = parseInt(inputCols.value) || 0;

        // 3. Limpiar y configurar SOLO el mapa de este sector
        mapaDestino.innerHTML = '';
        mapaDestino.style.gridTemplateColumns = `repeat(${columnas}, 1fr)`;

        for (let f = 0; f < filas; f++) {
            const letraFila = this.obtainRowLetter(f);
            for (let c = 1; c <= columnas; c++) {
                const asiento = document.createElement('div');
                asiento.classList.add('seat-pixel');
                asiento.textContent = `${letraFila}${c}`;
                mapaDestino.appendChild(asiento);
            }
        }
    }

    obtainRowLetter(index) {
        let letras = '';
        while (index >= 0) {
            // Obtenemos el resto de la división por 26 para saber qué letra toca (A-Z)
            letras = String.fromCharCode((index % 26) + 65) + letras;
            // Restamos 1 y dividimos para pasar a la siguiente "posición" de la columna
            index = Math.floor(index / 26) - 1;
        }
        return letras;
    }
    //#endregion

    //#region fetch Control
    mapEventCommand(data){
        console.log("entre a mapEventData");
        const eventCommand = {
            userId : 0,
            name : data.name,
            eventDate : data.eventDate,
            venue : data.venue,
            sectorsCommands : []
        }

        // Filtra todas las llaves que empiecen con 'nameSector_'
        const keys = Object.keys(data).filter(key => key.startsWith('nameSector_'));

        keys.forEach(key => {
            const id = key.split('_')[1]; // Extrae el número después del guion bajo
            const priceRaw = data[`price_${id}`].replace(',', '.');

            eventCommand.sectorsCommands.push({
                name : data[`nameSector_${id}`],
                price : parseFloat(priceRaw) || 0,
                rowsAmount : parseInt(data[`rowsAmount_${id}`]) || 0,
                columnsAmount : parseInt(data[`columnsAmount_${id}`]) || 0,
            });
        });

        console.log(eventCommand);
        //mandar a validar
        if(!this.validateEventData(eventCommand)) return;
        if(!this.validateSectors(eventCommand)) return;

        this.handleCreationAsync(eventCommand);
    }

    validateEventData(eventCommand){
        console.log("vine a validar evento");
        if(!eventCommand.name.trim() || !eventCommand.venue.trim()){
            Toast.show("el evento necesita un nombre y una dirección", "error");
            return false;
        }

        const eventDate = new Date(eventCommand.eventDate);
        if (isNaN(eventDate.getTime()) || eventDate <= new Date()) {
            Toast.show("La fecha y hora deben ser futuras", "error");
            return false;
        }
        return true;
    }

    validateSectors(eventCommand){

        if (eventCommand.sectorsCommands.length === 0) {
            Toast.show("Debes agregar al menos un sector", "error");
            return false;
        }

        let i = 1;
        for (const sector of eventCommand.sectorsCommands) {
            if (sector.price <= 0) {
                Toast.show(`El precio del sector ${i} debe ser mayor a 0.`, "error");
                return false;
            }
            if(!sector.name.trim()) {
                Toast.show(`El sector ${i} debe tener nombre`, "error");
                return false;
            }
            if(sector.rowsAmount <= 0 || sector.columnsAmount <=0 ) {
                Toast.show(`El sector ${i} debe tener filas y columnas de asientos`, "error");
                return false;
            }

            i+=1;
        }
        return true;
    }

    async handleCreationAsync(eventCommand){
        try {
            const data = UserDataService.getData();
            //pedimos la contraseña de usuario para comprobar si es realmente el admin
            const authentication = await this.adminAuth(data);
            if(!authentication) return;
            //obtenemos id
            eventCommand.userId = data.id;
            const response = await EventService.CreateEvent(eventCommand);
            if(response) Toast.show(`Evento creado, ID : ${response.eventId}`, "success");
            //limpiar formulario
        } catch (error)
        {
            const message = error.message || "Ocurrió un error inesperado";
            Toast.show(message, "error");
            console.error(`Error ${error.status}:`, error);
        }
    }

    async adminAuth(data){
        const password = await this.requestAdminAuth();
        if (!password) {
            Toast.show("Operación cancelada", "info");
            return false;
        }
        const response = await UserService.ValidateUserCredentials(data.name, password);

        //doble validacion para asegurar que la id del usuario validado sea la id del usuario que navega
        if(Number(response) !== Number(data.id)){
            Toast.show("Credenciales invalidas, creacion denegada", "error");
            return false;
        }
        return true;
    }

    async requestAdminAuth() {
        const modal = document.getElementById('adminAuthModal');
        const input = document.getElementById('adminPassword');
        const confirmBtn = document.getElementById('confirmAuth');
        const cancelBtn = document.getElementById('cancelAuth');

        modal.classList.remove('d-none');
        input.value = ''; 
        input.focus();

        return new Promise((resolve) => {
            confirmBtn.onclick = () => {
                const password = input.value;
                if (password) { 
                    modal.classList.add('d-none');
                    resolve(password);
                }
            };

            cancelBtn.onclick = () => {
                modal.classList.add('d-none');
                resolve(null);
            };
        });
    }
    //#endregion
}

