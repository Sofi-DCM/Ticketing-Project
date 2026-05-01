// importaciones
import { UserDataService, UserService} from "../services/userService.js"; //si da error en el html agregar Type="Module"
import { EventDataService, EventService } from "../services/eventService.js"; 
import { Toast } from "../tools/toast.js";

// Vista de catalogo de eventos
class EventCatalog{
    constructor(){
        // --- obtencion de contenedores ---
        this.pageDetail = document.getElementById('pageDetail');
        this.containerUser = document.getElementById('userContainer');
        this.sortButtons = document.getElementById('SortByButtons');
        this.tableBody = document.getElementById('tableBody');
        this.footer = document.getElementById('footer');
        // --- precarga de datos ---
        this.sortBy = "Newest";
        this.pageSize = 10;
        this.pageNumber = 1;
        console.log("creado");
        this.init();
    }
    
    init(){
        // --- mensajes de error para debug --- 
        if(!this.pageDetail){ console.error("Error: No se encontró el contenedor de detalles de pagina."); return;}
        if(!this.containerUser){ console.error("Error: No se encontró el contenedor del user."); return;}
        if(!this.sortButtons){ console.error("Error: No se encontró el contenedor de botones."); return;}
        if(!this.tableBody){ console.error("Error: No se encontró el contenedor de tabla."); return;}
        if(!this.footer){ console.error("Error: No se encontró el footer."); return;}

        // recuperacion de datos de navegacion anterior si la hubo
        //EventDataService.clearData();
        const prevData = EventDataService.getData();
        if(prevData){
            this.pageNumber = prevData.pageNumber;
            this.sortBy = prevData.sortBy;
            if (this.sortBy != "Newest") 
                this.setCorrectSortButton();
            EventDataService.clearData(); //una vez recuperados los libero
        }

        this.initSortButtonListeners();
        this.initPageButtonListeners();
        this.renderUserButton();
        this.handleAsync();
    }

    setCorrectSortButton(){
        //si habia sesion anterior marca el boton filtro 
        const correctButton = this.sortButtons.querySelector(`#${this.sortBy}`);
        correctButton.classList.add('button-filter-selected');
        correctButton.classList.remove('button-filter-waiting');

        const incorrectButton = this.sortButtons.querySelector('#Newest');
        incorrectButton.classList.remove('button-filter-selected');
        incorrectButton.classList.add('button-filter-waiting');
    }

    initSortButtonListeners(){
        const buttons = this.sortButtons.querySelectorAll('.btn');

        buttons.forEach(button => {
            button.addEventListener('click', (e) => {
                e.preventDefault();

                buttons.forEach(btn => {
                    btn.classList.remove('button-filter-selected');
                    btn.classList.add('button-filter-waiting');
                });

                button.classList.add('button-filter-selected');
                button.classList.remove('button-filter-waiting');
                this.setSortType(button.id);
            });
        });

    }

    initPageButtonListeners(){
        const buttons = this.footer.querySelectorAll('.btn');

        buttons.forEach(button => {
            button.addEventListener('click', (e) => {
                e.preventDefault();

                if(button.id == 'buttonPrev') this.pageNumber -= 1;
                else this.pageNumber += 1;
                console.log(this.pageNumber);
                this.handleAsync();
            });
        });
    }

// #region userHTML
    getWarningUserHTML(){
        return `
            <a href="Html/LogView.html" class="d-flex align-items-center user-button" id="login">
                <div class="user-avatar-container">
                    <img src="Images/WarningProfile.PNG" alt="Profile" class="user-avatar" >
                </div>
                <span class="user-name">LogIn</span>
            </a>     
        `
    }
    getLogedUserHTML(userName){
        return `
            <a href="./Html/LogView.html"
            class="d-flex align-items-center user-button"
            id="login">

                <div class="user-avatar-container">
                    <img src="./Images/UserProfile.PNG"
                        alt="Profile"
                        class="user-avatar">
                </div>
                <span class="user-name">${userName}</span>
            </a>
        `
    }
    getAdminUserHTML(){
        return `
            <a href="Html/AdminView.html" class="d-flex align-items-center user-button">
                <div class="user-avatar-container">
                    <img src="Images/UserProfile.PNG" alt="Profile" class="user-avatar">
                </div>
                <span class="user-name">Admin</span> 
            </a>`
    }
// #endregion

    renderUserButton(){
        //UserDataService.clearData();
        //UserDataService.saveData(5,"yo");
        //UserDataService.saveData(1,"admin");
        const storageData = UserDataService.getData();
        console.log("construyendo data");
        if(!storageData){
            console.log("construyendo login");
            this.containerUser.innerHTML = this.getWarningUserHTML();
            //set escucha
            const loginLink = this.containerUser.querySelector('#login');

            loginLink.addEventListener('click', (e) => {
                e.preventDefault();
                console.log("Se hizo click en el link de login");
                EventDataService.saveData(this.pageNumber, this.sortBy);
                window.location.href = e.currentTarget.href;
            });
        }
        else {
            console.log("construyendo user");
            if(storageData.id == 1)
                this.containerUser.innerHTML = this.getAdminUserHTML();
            else this.containerUser.innerHTML = this.getLogedUserHTML(storageData.name);
        }
    }

    setSortType(sortId){
        switch (sortId){
            case 'Newest':
                console.log("sorteo por newest");
                this.sortBy = "Newest";
                break;
            case 'DateAsc':
                console.log("sorteo por date asc");
                this.sortBy = "DateAsc";
                break;
            case 'DateDesc':
                console.log("sorteo por date desc");
                this.sortBy = "DateDesc";
                break;
            case 'NameAsc':
                console.log("sorteo por name asc");
                this.sortBy = "NameAsc";
                break;
            case 'NameDesc':
                console.log("sorteo por name desc");
                this.sortBy = "NameDesc";
                break;
        }
        this.pageNumber = 1; //cambia tipo de busqueda -> vuelve a pagina 1
        this.handleAsync();
    }

    async handleAsync (){
        try{
            var response = await EventService.GetActiveEvents(this.pageNumber, this.pageSize, this.sortBy);
            console.log(response);
            this.render(response);
        }catch(error)
        {
            const message = error.message || "Ocurrió un error inesperado";
            Toast.show(message, "error");
            console.error(`Error ${error.status}:`, error);
        }
    }

    render(response){
        if(response.events.length === 0) {
            console.log("entre");
            this.tableBody.innerHTML = '<tr><td colspan="4" class="message-error">No hay eventos activos disponibles</td></tr>';;
            return;
        }
        //eventos
        this.tableBody.innerHTML = '';
        const htmlMap = response.events.map( event =>
            `<tr>
                <td><button class="button-page rounded-pill" data-id="${event.id}">Ver Mas</button></td>
                <td>${event.name}</td>
                <td>${event.venue}</td>
                <td>${new Date(event.eventDate).toLocaleDateString()}</td>
            </tr>`
        ).join('');
        this.tableBody.innerHTML = htmlMap;
        
        //pagina
        console.log()
        this.pageDetail.textContent = `${response.totalEventsInBd} EVENTOS · PÁGINA ${response.pageNumber}`; 
        const totalPages = Math.ceil(response.totalEventsInBd / response.pageSize); 
        this.footer.querySelector('#pageIndicator').innerHTML = `<strong>${response.pageNumber}</strong> / ${totalPages}`;
        this.enableOrDisablePageNavigationButtons(response.hasPreviousPage, response.hasNextPage);
        this.initEventsListeners();
    }

    enableOrDisablePageNavigationButtons(hasPrev, hasNext){
        console.log("intentando hacer botones")
        // Si hasPrev es false, el botón se deshabilita
        this.footer.querySelector('#buttonPrev').disabled = !hasPrev;
        // Si hasNext es false, el botón se deshabilita
        this.footer.querySelector('#buttonNext').disabled = !hasNext;
        console.log("botones hechos")
    }

    initEventsListeners(){
        const buttons = this.tableBody.querySelectorAll('.button-page');

        buttons.forEach(button => {
            button.addEventListener('click', (e) => {
                e.preventDefault();

                if (e.target.classList.contains('button-page')) {
                    const id = e.target.getAttribute('data-id');
                    console.log(id);
                    EventDataService.saveData(this.pageNumber, this.sortBy);
                    //window.location.href = `../Html/seatMapView.html?id=${id}`;
                    window.location.href = `../Html/seatMapView.html?eventId=${id}&from=index`;
                }
            });
        });
    }
};

const app = new EventCatalog();
