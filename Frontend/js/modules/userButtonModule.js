import { UserDataService, UserService} from "../services/userService.js"; 
import { EventDataService, EventService } from "../services/eventService.js"; 
import { Toast } from "../tools/toast.js";

export function initUserButtonModule(containerUser){
    const manager = new UserButtonModule(containerUser);
    manager.renderUserButton();
}

class UserButtonModule {
    constructor(containerUser){
        this.containerUser = containerUser;
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
            <button type="click" class="d-flex align-items-center user-button" id="sesion">
                <div class="user-avatar-container">
                    <img src="./Images/UserProfile.PNG"
                        alt="Profile"
                        class="user-avatar">
                </div>
                <span class="user-name">${userName}</span>
            </button>
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

// #region User sesion button
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
            else {
                this.containerUser.innerHTML = this.getLogedUserHTML(storageData.name);
                this.initUserSesionButtons();
            }
        }
    }

    initUserSesionButtons(){
        console.log("entre a init botones");
        const sesionButton = this.containerUser.querySelector('#sesion');
        sesionButton.addEventListener('click', (e) => {
            e.preventDefault();
            console.log("entre a boton cerrar");
            const modal = document.getElementById('userModal')
            modal.classList.remove('d-none');
            this.handleSesion(modal);
        });
    }

    async handleSesion(modal){
        console.log("entre a manejar sesion");
        const confirmBtn = document.getElementById('confirm');
        const cancelBtn = document.getElementById('cancel');
        return new Promise((resolve) => {
            confirmBtn.onclick = () => {
                console.log("entre a borrar sesion");
                UserDataService.clearData();
                modal.classList.add('d-none');
                Toast.show("Sesion cerrada", "info");
                this.renderUserButton();
                resolve(null);
            };

            cancelBtn.onclick = () => {
                console.log("entre a cancelar sesion");
                modal.classList.add('d-none');
                resolve(null);
            };
        });
    }
// #endregion
}