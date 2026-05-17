import { Toast } from "../tools/toast.js";
import { AuditLogService } from "../services/auditService.js";

export function initAuditModule(){
    const manager = new auditModule();
    const container = document.getElementById('mainContainer');
    manager.renderModule(container);
}

//para capturar las opciones seleccionadas al tocar un boton:
//1- obtener el select
//2- asignar un event listener a un boton
//3- obtener el valor del select al hacer click en el boton "select.value"
class auditModule{
    constructor(){
    }

    async renderModule(container){
        Toast.show("Abriendo vista de auditoria","info");
        const queryDto = {
        pageNumber: 1,
        pageSize: 20, // Coincide con tu default del backend
        userId: null,
        action: "CREATE_USER",
        entityType: null,
        entityId: null,
        startDate: null,
        endDate: null
    };
        let response = await AuditLogService.GetAuditLogsPaginated(queryDto);
        console.log(response);
    }
}