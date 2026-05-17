import { UserService} from "../services/userService.js"; 
import { Toast } from "../tools/toast.js";
import { initEventModule } from '../modules/eventModule.js';
import { initAuditModule } from '../modules/auditModule.js';
import { initUserButtonModule } from "../modules/userButtonModule.js";

//tomar los botones
const buttonEvent = document.getElementById('createEvent');
const buttonAudit = document.getElementById('seeAudit');
const userContainer = document.getElementById('userContainer');

buttonEvent.addEventListener('click', () => {
    document.body.className = 'event-mod';
    buttonEvent.className = 'btn fw-bold button-custom button-create-event-selected';
    buttonAudit.className = 'btn fw-bold button-custom button-audit';
    initEventModule();
});

buttonAudit.addEventListener('click', () => {
    document.body.className = 'audit-mod';
    buttonEvent.className = 'btn fw-bold button-custom button-create-event';
    buttonAudit.className = 'btn fw-bold button-custom button-audit-selected';
    initAuditModule();
});

