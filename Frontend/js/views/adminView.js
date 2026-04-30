import { UserDataService, UserService} from "../services/userService.js"; //si da error en el html agregar Type="Module"
import { EventDataService, EventService } from "../services/eventService.js"; 
import { Toast } from "../tools/toast.js";
import { initEventModule } from '../modules/eventModule.js';
import { initAuditModule } from '../modules/auditModule.js';

//tomar los botones
const buttonEvent = document.getElementById('createEvent');
const buttonAudit = document.getElementById('seeAudit');

buttonEvent.addEventListener('click', () => {
    // Cambiamos la clase del body para activar el CSS correcto
    document.body.className = 'event-mod';
    // configuramos css de los botones
    buttonEvent.className = 'btn fw-bold button-custom button-create-event-selected';
    buttonAudit.className = 'btn fw-bold button-custom button-audit';
    // ejecutar el módulo
    initEventModule();
});

buttonAudit.addEventListener('click', () => {
    document.body.className = 'audit-mod';
    buttonEvent.className = 'btn fw-bold button-custom button-create-event';
    buttonAudit.className = 'btn fw-bold button-custom button-audit-selected';
    initAuditModule();
});




//Prueba fetch Funciona
const evento = {
    userId : 1,
    name : "Evento Frontend",
    eventDate : "2026-08-29T20:24:46.306Z",
    venue : "127.0.0.1:5500",
    sectorsCommands : [{
        name : "adminView.html",
        price : 200,
        columnsAmount : 10,
        rowsAmount : 2 
    },{
        name : "adminView.js",
        price : 1000,
        columnsAmount : 2,
        rowsAmount : 1 
    }]
}
try{
    //const response = EventService.CreateEvent(evento);
    //console.log(response);
} catch (error) {console.log(error.message)}
