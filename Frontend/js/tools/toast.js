
export const Toast = {
    show(message, type = 'success', duration = 3000) {
        // Crear el contenedor si no existe
        let container = document.getElementById('toastContainer');
        if (!container) {
            container = document.createElement('div');
            container.id = 'toastContainer';
            container.className= 'toast-container';
            document.body.appendChild(container);
        }

        // Crear el elemento del toast
        const toast = document.createElement('div');
        toast.innerText = message;
        toast.className = `toast-item toast-${type}`;
        container.appendChild(toast);

        // Animacion de entrada
        setTimeout(() => {
            toast.style.opacity = '1';
            toast.style.transform = 'translateX(0)';
        }, 10);

        // Eliminacion automatica
        setTimeout(() => {
            toast.style.opacity = '0';
            toast.style.transform = 'translateY(-20px)';
            setTimeout(() => toast.remove(), 400); // Espera a que termine la animación
        }, duration);
    }
};