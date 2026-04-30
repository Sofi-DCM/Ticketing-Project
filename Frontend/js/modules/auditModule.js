import { Toast } from "../tools/toast.js";

export function initAuditModule(){
    const container = document.getElementById('mainContainer');
    container.innerHTML = `
        <h1 class="p-5 text-white">Aun sin implementación<h1>`
    Toast.show("Abriendo vista de auditoria","info");
}