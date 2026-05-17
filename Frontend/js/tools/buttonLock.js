//bloqueo de botón
export async function lockButton(button, asyncAction, loadingText = "Procesando...") {
    if (!button || button.disabled) return;
    const originalText = button.textContent;
    try {
        button.disabled = true;
        button.textContent = loadingText;

        await asyncAction();

    } finally {
        button.disabled = false;
        button.textContent = originalText;
    }
}