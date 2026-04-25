import { CONFIG } from "../configURLs/config.js";

// apiClient.js
export async function baseFetch(url, method = 'GET', body = null) {

    // Definimos los headers básicos
    const headers = {
        'Content-Type': 'application/json'
    };

    const options = { 
        method, 
        headers 
    };

    // Si hay un cuerpo (POST/PUT), lo convertimos a string
    if (body) {
        options.body = JSON.stringify(body);
    }

    try {
        const response = await fetch(url, options);

        // Si el servidor responde con error (400, 404, 500, etc)
        if (!response.ok) {
            // Intentamos leer la respuesta como JSON
            let errorData;
            try {
                errorData = await response.json();
            } catch {
                // Por si el servidor no devuelve un JSON, usamos el texto plano
                errorData = { message: await response.text() };
            }

            // Lanzamos el objeto. Usamos errorData.message para que sea solo el texto.
            throw { 
                status: response.status, 
                message: errorData.message || "Error desconocido" 
            };
        }

        // Si la respuesta es exitosa pero vacía (ej: 204 No Content)
        if (response.status === 204) return true;

        return await response.json();

    } catch (error) {
        console.error("Falla en la comunicación con la API:", error);

        if (!error.status) {
            error = { 
                status: 503, 
                message: "Servidor no disponible o error de red" 
            };
        }

        throw error;
    }
}