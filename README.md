# Proyecto Ticketing
Plataforma de venta de entradas para eventos de una productora.

## Alcance Entrega 1 :
- El sistema gestiona creación de eventos en vista de administrador.
- El sistema presenta un catálogo de eventos con paginación.
- El sistema permite ver a detalle un evento especifico.
- El sistema permite a usuarios reservar asientos especificos de un evento.

## Alcance Entrega 2:
- El sistema gestiona el control de concurrencia evitando reservas duplicadas de una misma butaca.
- El sistema permite a usuarios bloquear temporalmente asientos durante el proceso de compra.
- El sistema permite confirmar la compra mediante un pago simulado actualizando el estado de la reserva y butaca.
- El sistema implementa la liberación automática de reservas vencidas.
- El sistema presenta manejo de errores visuales y temporizador de reserva en frontend.

## Pasos para ejecutar el proyecto :

> [!NOTE]
> Las migraciones son automáticas.
> No se requiere uso de terminal `update-database`.

1. Clonar el repositorio en una carpeta nueva.
2. Dentro del repositorio , en la carpeta *"Backend"* buscar el archivo **Backend.snl** de Visual Studio y abrirlo.
3. Cambiar la *"connection string"* en **appsettings.json**.
    `"ConnectionStrings": {"DefaultConnection": "coneccion"}`
4. Seleccionar el perfil segun objetivo.
5. Ejecutar.

### Perfiles y usos :

- **https** -> para generar el *swagger* y la documentacion automatica de *OpenApi*.
- **Frontend** -> para ejecutar el backend y el frontend automatico en el navegador predeterminado.

## IMPORTANTE -> Detalles de uso
> [!NOTE]
> **Vista de administración** se accede tocando el **boton "LogIn"** (incluido en vista de catalogo de eventos y vista de asiento), ingresando las credenciales de admin en el formulario y luego tocando el mismo boton que ahora dice **"admin"**.

#### Credenciales:
`{ "id" = 1, "name" = "admin",
"password" = "1234" }`

#### Reservas y pago: 
Las reservas realizadas pueden visualizarse y abonarse desde el carrito, ubicado en la parte superior de la interfaz, junto a los temporizadores de cada reserva.

<img width="1128" height="177" alt="image" src="https://github.com/user-attachments/assets/45969557-159f-4433-86f4-baf76c5b1db5" />

## Detalles del Sistema
### Tecnologías :
#### Backend y Base de Datos
- Lenguaje: **C#** con **.Net 8**.
- ORM: **Entity Framework Core** utilizando **Code-First**.
- Documentacion: **OpenAPI** (Swagger UI) autogenerada.

#### Frontend
- Interfaz: Arquitectura hecha con **Vanilla JavaScript**, **HTML5** y **CSS**.
- Estilos: **Bootstrap**.
- Comunicación: comunicacion asincrona con el backend mediante **Fetch API**.

### Prácticas y Patrones :
- **Clean Code** y **SOLID**.
- **Clean Architecture**.
- Transaccionalidad **ACID**.
- Endpoints siguiendo **Api RESTful**.

## Estructura :
Estructura implementada usando Seat de ejemplo.
```text
+---Backend
|   |
|   +---Application
|   |   |
|   |   +---Interfaces
|   |   |   +---Handlers 
|   |   |   \---Repositories
|   |   +---Response
|   |   \---UseCase
|   |       +---_Seat
|   |       |   +---Commands
|   |       |   \---Queries
|   |   
|   +---Domain
|   |   +---Constants
|   |   +---Entities
|   |   +---Exceptions
|   |
|   +---Infrastructure
|   |   |
|   |   +---BackgroundJobs
|   |   +---Migrations
|   |   +---Persistence
|   |   |   |   AppDbContext.cs
|   |   |   \---EntityConfiguration
|   |   \---Repositories
|   |
|   \---Presentation
|       |   GlobalUsings.cs
|       |   Program.cs
|       +---Controllers
|       +---Middlewares
|
\---Frontend
    |   index.html
    +---Css
    |   \---Views
    +---Html
    +---Images
    \---js
        +---configURLs
        +---modules
        +---services
        +---tools
        \---views
```
