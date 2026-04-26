// importaciones
import { UserDataService, UserService} from "../services/userService.js"; //si da error en el html agregar Type="Module"
import { EventService } from "../services/eventService.js"; //si da error en el html agregar Type="Module"
import { Toast } from "../tools/toast.js";

// Vista de catalogo de eventos
class EventCatalog{
    constructor(){
        this.containerUser = document.getElementById('userContainer');
        this.sortButtons = document.getElementById('SortByButtons');
        this.tableBody = document.getElementById('tableBody');
        this.prevBtn = document.getElementById('buttonPrev');
        this.nextBtn = document.getElementById('buttonNext');
        this.pageDetail = document.getElementById('pageDetail');
        this.pageIndicator = document.getElementById('pageIndicator');
        this.sortBy = "Newest";
        this.pageSize = 10;
        this.pageNumber = 1;
        console.log("creado");
        this.init();
    }
    
    init(){
        if(!this.containerUser){ 
            console.error("Error: No se encontró el contenedor del user.");
            return;}
        if(!this.sortButtons){ 
            console.error("Error: No se encontró el contenedor de botones.");
            return;}
        if(!this.tableBody){ 
            console.error("Error: No se encontró el contenedor de tabla.");
            return;}
        this.initSortButtonListeners();
        this.userButton();
        //this.handleAsync('sortByNewest');
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

    getWarningUserHTML(){
        return `
            <a href="Html/LogView.html" class="d-flex align-items-center user-button">
                <div class="user-avatar-container">
                    <img src="Images/WarningProfile.PNG" alt="Profile" class="user-avatar" >
                </div>
                <span class="user-name">LogIn</span>
            </a>     
        `
    }
    getLogedUserHTML(userName){
        return `
            <div id="referenceUser" class="d-flex align-items-center user">
                <div class="user-avatar-container">
                    <img src="Images/UserProfile.PNG" alt="Profile" class="user-avatar">
                </div>
                <span class="user-name">${userName}</span> 
            </div>
        `
    }
    getAdminUserHTML(){
        return `
            <a id="referenceUser" href="Html/AdminView.html" class="d-flex align-items-center user-button">
                <div class="user-avatar-container">
                    <img src="Images/UserProfile.PNG" alt="Profile" class="user-avatar">
                </div>
                <span class="user-name">Admin</span> 
            </a>`
    }

    userButton(){
        //UserDataService.clearData();
        //UserDataService.saveData(5,"yo");
        //UserDataService.saveData(1,"admin");
        const storageData = UserDataService.getData();
        if(!storageData){
            this.containerUser.innerHTML = this.getWarningUserHTML();
        }
        else {
            if(storageData.id == 1)
                this.containerUser.innerHTML = this.getAdminUserHTML();
            else this.containerUser.innerHTML = this.getLogedUserHTML(storageData.name);
        }
    }

    setSortType(sortId){
        switch (sortId){
            case 'sortByNewest':
                console.log("sorteo por newest");
                this.sortBy = "Newest";
                break;
            case 'sortByDateAsc':
                console.log("sorteo por date asc");
                this.sortBy = "DateAsc";
                break;
            case 'sortByDateDesc':
                console.log("sorteo por date desc");
                this.sortBy = "DateDesc";
                break;
            case 'sortByNameAsc':
                console.log("sorteo por name asc");
                this.sortBy = "NameAsc";
                break;
            case 'sortByNameDesc':
                console.log("sorteo por name desc");
                this.sortBy = "NameDesc";
                break;
        }
        this.handleAsync();
    }

    async handleAsync (){
        try{
            var response = await EventService.GetActiveEvents(this.pageNumber, this.pageSize, this.sortBy);
            console.log(response);
            this.setPageButtons(response.hasPreviousPage, response.hasNextPage);
            this.render(response);
        }catch(error){
            console.log(error);
            //control de errores
        }
    }

    setPageButtons(hasPrev, hasNext){
        console.log("intentando hacer botones")
        // Si hasPrev es false, el botón se deshabilita
        this.prevBtn.disabled = !hasPrev;
        // Si hasNext es false, el botón se deshabilita
        this.nextBtn.disabled = !hasNext;
        console.log("botones hechos")
    }

    render(response){
        //eventos
        this.tableBody.innerHTML = '';
        const htmlMap = response.events.map( event =>
            `<tr>
                <td><button onclick="verDetalle(${event.id}) class="button-page"">Ver Mas</button></td>
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
        this.pageIndicator.innerHTML = `<strong>${response.pageNumber}</strong> / ${totalPages}`;
    }
};

//prueba fetch
const app = new EventCatalog();