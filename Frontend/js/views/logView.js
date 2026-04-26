// importaciones
import { UserDataService, UserService} from "../services/userService.js"; //si da error en el html agregar Type="Module"
import { Toast } from "../tools/toast.js";


class UserForm {
    constructor(containerId) {
        this.container = document.getElementById(containerId);
        this.isLogin = true; //estado inicial de login);
        this.init();
    }

    init() {
        if (!this.container) {
            console.error("Error: No se encontró el contenedor.");
            return;
        }
        console.log("UserForm inicializado");
        this.render(); 
    }

    //------- Plantillas de renderizado -------
    getLoginHTML() {
        return `
            <h2 class="mb-4 fw-bold text-uppercase">Log In</h2>
            <form id="userForm">
                <div class="mb-3 position-relative">
                    <input name="name" type="text" class="form-control custom-input" placeholder="User Name">
                </div>
               
                <div class="mb-3 position-relative">
                    <input name="password" type="password" class="form-control custom-input" placeholder="Password">
                </div>

                <button type="submit" class="btn fw-bold mb-3 button-login">SIGN IN</button>
               
                <div class="text-center small">
                    No tiene una cuenta? <a href="#" id="formLink" class="text-white fw-bold custom-link">Registrar</a>
                </div>
            </form> `
    }

    getRegisterHTML() {
        return `            
            <h2 class="mb-4 fw-bold text-uppercase">Create Account</h2>
            <form id="userForm">
                <div class="mb-3 position-relative">
                    <input name="name" type="text" class="form-control custom-input" placeholder="User Name">
                </div>

                <div class="mb-3 position-relative">
                    <input name="email" type="email" class="form-control custom-input" placeholder="E-mail address">
                </div>
               
                <div class="mb-3 position-relative">
                    <input name="password" type="password" class="form-control custom-input" placeholder="Password">
                </div>

                <button type="submit" class="btn fw-bold mb-3 button-login">SIGN UP</button>
               
                <div class="text-center small">
                    Ya tiene una cuenta? <a href="#" id="formLink" class="text-white fw-bold custom-link">Ingresar</a>
                </div>
            </form> `
    }

    render() {
        this.container.classList.add('form-hidden');
        setTimeout(() => {
            this.container.innerHTML = this.isLogin ? this.getLoginHTML() : this.getRegisterHTML();
            this.initListeners();
            this.container.classList.remove('form-hidden');
        }, 400); 
    }

    initListeners() {
        // Listener para el link de cambio entre formularios (Login <-> Registro)
        const link = this.container.querySelector('#formLink');
        link.addEventListener('click', (e) => {
            e.preventDefault();
            this.isLogin = !this.isLogin; // Cambiamos el estado
            this.render(); // Re-renderizamos
        });

        // Listener para el envío del formulario
        const form = this.container.querySelector('#userForm');
        form.addEventListener('submit', (e) => {
            e.preventDefault();
            this.handleSubmit();
        });
    }

    async handleSubmit() {
        const formElement = this.container.querySelector('#userForm'); //extraer datos
        const formData = new FormData(formElement); //crea objeto especial con pares clave/valor
        const data = Object.fromEntries(formData.entries()); //lo transforma en un objeto javaScript {}

        try{
            let userId = null;
            if (this.isLogin) {
                userId = await UserService.ValidateUserCredentials(data.name, data.password);
                
            } else {
                const command = {
                    name: data.name, 
                    email: data.email,
                    passwordHash : data.password
                };
                userId = await UserService.CreateUser(command);
            }
            UserDataService.saveData(userId, data.name);
            Toast.show("¡Bienvenido "+ data.name +"!");
        } catch (error) 
        {
            switch (error.status) {
                case 400:
                    console.warn("Datos mal formados");
                    Toast.show(error.message, "error");
                    break;

                case 401:
                    Toast.show(error.message, "error");
                    break;

                case 409:
                    Toast.show(error.message, "error");
                    break;

                case 500:
                    Toast.show(error.message, "error");
                    break;

                default:
                    Toast.show("error inesperado: "+error.message, "error");
                    break;    
            }

            //DuplicateNameException (ya existe el nombre o mail) 409
            //UnauthorizedException (credenciales invalidas) 401
            //KeyNotFoundException (id invalida) 404
        }
    }
}

// Uso:
const app = new UserForm('userFormBox');

/* 
            if (document.referrer) {
                window.location.href = document.referrer; //va directo a la pagina anterior
            } else {
                window.location.href = "index.html"; // Backup por si no hay historial
            }
console.log("🚀 Sistemas de prueba listos. Podés usar UserService en la consola.");

------- Prueba de conexion con fetch y query
const user = await UserService.GetUserById(1);
console.log(user);

------- Prueba de conexion con fetch y body
let miNuevoUsuario = { name: "Mimu", email: "Mimu@unaj.edu.ar", passwordHash: "123" };
const response = await UserService.CreateUser(miNuevoUsuario);
console.log(response);

----------- Para guardar el id de usuario en el local storage
const idUsuario = 123;
localStorage.setItem('usuarioId', idUsuario);
----------Para recuperar el valor 
const idGuardado = localStorage.getItem('usuarioId');

console.log("El ID permanente es:", idGuardado);
*/