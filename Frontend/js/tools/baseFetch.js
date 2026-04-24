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
            const errorText = await response.text();
            throw new Error(`Error ${response.status}: ${errorText}`);
        }

        // Si la respuesta es exitosa pero vacía (ej: 204 No Content)
        if (response.status === 204) return true;

        return await response.json();

    } catch (error) {
        console.error("Falla en la comunicación con la API:", error);
        throw error;
    }
}