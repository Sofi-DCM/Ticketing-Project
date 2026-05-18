import { Toast } from "../tools/toast.js";
import { AuditLogService } from "../services/auditService.js";
import { lockButton } from "../tools/buttonLock.js";

export function initAuditModule(){
    const manager = new auditModule();
    const container = document.getElementById('mainContainer');
    const body = document.getElementById('body');
    body.classList.remove("event-mod");
    manager.init(container);
}

class auditModule{
    constructor(){
        this.pageSize = 10;
        this.pageNumber = 1;
    }

    init(container){
        this.renderModule(container);
        this.footer = document.getElementById('footer');
        this.initPageButtonListeners(container);
        this.initFilterButtonListener(container);
    }

// #region handle page navigation buttons

    initPageButtonListeners(container){
        const buttons = this.footer.querySelectorAll('.btn');

        buttons.forEach(button => {
            button.addEventListener('click', (e) => {
                e.preventDefault();

                if(button.id == 'buttonPrev') this.pageNumber -= 1;
                else this.pageNumber += 1;
                console.log(this.pageNumber);
                this.handleGetAudits(this.createFilterDto(), container);
            });
        });
    }

    enableOrDisablePageNavigationButtons(hasPrev, hasNext){
        console.log("intentando hacer botones")
        // Si hasPrev es false, el botón se deshabilita
        this.footer.querySelector('#buttonPrev').disabled = !hasPrev;
        // Si hasNext es false, el botón se deshabilita
        this.footer.querySelector('#buttonNext').disabled = !hasNext;
        console.log("botones hechos")
    }
// #endregion
 
    initFilterButtonListener(container){
        const filterButton = document.getElementById('filter');
        filterButton.addEventListener('click', async (e) => {
            await lockButton(e.currentTarget, async () => {
                this.pageNumber = 1;
                await this.handleGetAudits(this.createFilterDto(), container);
            })
        });
    }

    createFilterDto(){
        return {
            pageNumber: this.pageNumber,
            pageSize: this.pageSize,

            userId: document.getElementById('userId').value ? Number(document.getElementById('userId').value) : null,
            entityId: document.getElementById('entityId').value.trim() || null,
                        
            action: document.getElementById('actionOptions').value || null,
            entityType: document.getElementById('entityTypeOptions').value || null,

            startDate: document.getElementById('startDate').value || null,
            endDate: document.getElementById('endDate').value || null,
            createdAt: document.getElementById('createdAt').value || null
        };
    }

    async handleGetAudits(queryDto, container){
        try {
            console.log("Enviando filtros al servidor...", queryDto);
            const response = await AuditLogService.GetAuditLogsPaginated(queryDto);
            console.log(response);
            if(response && response.auditLogs){
                this.renderAudits(response.auditLogs);
                this.enableOrDisablePageNavigationButtons(response.hasPreviousPage, response.hasNextPage);
            } else {
                console.warn("No se encontraron registros para los filtros aplicados.");
            }
        } catch (error) {
            Toast.show("Error al procesar el filtrado de auditorías:", "error");
        }
    }

// #region manejo de renderizado
    async renderModule(container){
        container.innerHTML = this.getAuditModuleHtml();
    }

    getAuditModuleHtml(){
        return `
        <nav id="filters" class="p-2 text-white">
            <div class="filter-group">
                <label for="userId">User ID:</label>
                <input type="number" id="userId" name="userId">
            </div>
            <div class="filter-group">
                <label for="entityId">Entity ID:</label>
                <input type="text" id="entityId" name="entityId">
            </div>
            <div class="filter-group">
                <label for="actionOptions">Action:</label>
                <select name="actionOptions" id="actionOptions">
                    <option value="">-- All Actions --</option>
                    <option value="CREATE_USER">Create User</option>
                    <option value="RESERVE_SUCCESS">Reserve Success</option>
                    <option value="RESERVE_ATTEMP">Reserve Attemp</option>
                    <option value="RESERVE_EXPIRED">Reserve Expired</option>
                    <option value="PAYMENT_CONFIRMED">Payment Confirmed</option>
                </select>
            </div>
            <div class="filter-group">
                <label for="entityTypeOptions">Entity Type:</label>
                <select name="entityTypeOptions" id="entityTypeOptions">
                    <option value="">-- All Entities --</option>
                    <option value="User">User</option>
                    <option value="Seat">Seat</option>
                    <option value="Reservation">Reservation</option>
                </select>
            </div>
            <div class="filter-group">
                <label for="startDate">Start Date:</label>
                <input name="startDate" type="datetime-local" id="startDate">
            </div>
            <div class="filter-group">
                <label for="endDate">End Date:</label>
                <input name="endDate" type="datetime-local" id="endDate">
            </div>
            <div class="filter-group">
                <label for="createdAt">Created At:</label>
                <input name="createdAt" type="date" id="createdAt">
            </div>
            <button id="filter" class="btn btn-sm rounded-pill button-filter-waiting">Filter</button>
        </nav>
        <div class="scroll-container p-2">
            <div id="audits" class="cards-grid"> </div>
        </div>
        <footer id="footer" class="pagination-footer d-flex justify-content-center align-items-center gap-3 mt-4">
            <!--Disabled o no segun hay o no mas paginas-->
            <button id="buttonPrev" class="btn btn-light btn-sm button-page" disabled>‹ Anterior</button>
            <span id="pageIndicator" class="page-indicator"></span>
            <button id="buttonNext" class="btn btn-light btn-sm button-page" disabled>Siguiente ›</button>
        </footer>`
    }

    async renderAudits(audits){
        const containerAudits = document.getElementById('audits');
        containerAudits.innerHTML = '';
        containerAudits.innerHTML = audits.map(a => {
            return this.getAuditCard(a);
        }).join('')
    }

    getAuditCard(audit){
        let action = "nada";
        switch (audit.action){
            case "CREATE_USER":
                action = "Create User";
                break;
            case "RESERVE_SUCCESS":
                action = "Reserve Success";
                break;
            case "RESERVE_ATTEMP":
                action = "Reserve Attemp";
                break;
            case "RESERVE_EXPIRED":
                action = "Reserve expired";
                break;
            case "PAYMENT_CONFIRMED":
                action = "Payment Confirmed";
                break;
        };
        return `
            <div class="card-item">
                <h3><strong>Action:</strong> ${action}</h3>
                <h5><strong>Created at:</strong> ${audit.createdAt}</h5>
                <h6><strong>User ID:</strong> ${audit.userId}</h6>
                <h6>${audit.entityType}: ${audit.entityId}</h6>
                <p><strong>Details:</strong> ${audit.details}</p>
            </div>`
    }
// #endregion
}