
using Application.Interfaces;
using Application.UseCase._Reservation.Commands.ConfirmPayment;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// -------- Database Connection --------

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// -------- Dependency Injection --------

builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

// User
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICreateUserHandler, CreateUserHandler>();
builder.Services.AddScoped<IGetUserByIdHandler, GetUserByIdHandler>();
builder.Services.AddScoped<IValidateUserCredentialsHandler, ValidateUserCredentialsHandler>();
builder.Services.AddScoped<IGetUserReservationsByIdHandler, GetUserReservationsByIdHandler>();

//AuditLog
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddScoped<ICreateAuditLogHandler, CreateAuditLogHandler>();
builder.Services.AddScoped<IGetAuditLogsPaginatedHandler, GetAuditLogsPaginatedHandler>();

//Event
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IGetActiveEventsHandler, GetActiveEventsHandler>();
builder.Services.AddScoped<ICreateEventHandler, CreateEventHandler>();
builder.Services.AddScoped<IGetEventByIdHandler, GetEventByIdHandler>();    

// Reservation
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<ICreateReservationHandler, CreateReservationHandler>();
builder.Services.AddScoped<IConfirmPaymentHandler, ConfirmPaymentHandler>();
builder.Services.AddScoped<ICancelReservationHandler, CancelReservationHandler>();

//Seat
builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<IChangeSeatStatusHandler, ChangeSeatStatusHandler>();
builder.Services.AddScoped<IGetSeatsBySectorHandler, GetSeatsBySectorHandler>();
builder.Services.AddScoped<IExpireReservationsHandler, ExpireReservationsHandler>();
builder.Services.AddScoped<ICreateSeatsForSectorHandler, CreateSeatsForSectorHandler>();
builder.Services.AddHostedService<ReservationExpirationBackgroundService>();

//Sector
builder.Services.AddScoped<ISectorRepository, SectorRepository>();
builder.Services.AddScoped<ICreateSectorHandler, CreateSectorHandler>();
builder.Services.AddScoped<IGetSectorsByEventIdHandler, GetSectorsByEventIdHandler>();

// -------- Payment Simulation --------
builder.Services.AddScoped<IPaymentSimulator, SimulatedPayment>();

var app = builder.Build();

// -------- Use Middleware --------
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// -------- Configuration for "Frontend" profile --------

string rootPath = builder.Environment.ContentRootPath;
string frontendPath = Path.Combine(rootPath, "..", "Frontend"); 

if (!Directory.Exists(frontendPath)) 
{
    frontendPath = Path.Combine(rootPath, "..", "..", "Frontend");
}

// Imprimir en consola la url encontrada
Console.WriteLine($"Buscando Frontend en: {Path.GetFullPath(frontendPath)}");

if (Directory.Exists(frontendPath))
{
    app.UseDefaultFiles();
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
            Path.GetFullPath(frontendPath)), 
        RequestPath = ""
    });
}
else
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("ERROR: No se encontró la carpeta Frontend. Verifica la estructura de carpetas.");
    Console.ResetColor();
}

app.UseAuthorization();
app.MapControllers();
app.UseCors();

// -------- Automathic Migration and Dinamic Seeds --------

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
        if (!context.Events.Any())
        {
            var newEvent = new Event
            {
                Name = "The Eras Tour",
                EventDate = DateTime.UtcNow.AddMonths(3),
                Venue = "Movistar Arena",
                Status = EventStatusConstants.Active
            };

            context.Events.Add(newEvent);
            context.SaveChanges();

            var sectorsNames = new[] { "Platea Baja", "Platea Alta" };
            var rowIdentifiers = new[] { "A", "B", "C", "D", "E" };

            for (int i = 0; i <sectorsNames.Length; i++)
            {
                var newSector = new Sector
                {
                    EventId = newEvent.Id,
                    Name = sectorsNames[i],
                    Price = 100000 * (i + 1),
                    Capacity = 50,
                };

                context.Sectors.Add(newSector);
                context.SaveChanges();

                foreach (var row in rowIdentifiers)
                {
                    for (int seatNum = 1; seatNum <= 10; seatNum++)
                        context.Seats.Add(new Seat
                        {
                            SectorId = newSector.Id,
                            RowIdentifier = row,
                            SeatNumber = seatNum,
                            Status = SeatStatusConstants.Available,
                            Version = 1, //por ahora hasta implementar ConcurrencyToken
                        });
                }
                context.SaveChanges();
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("------ ERROR DE MIGRACIÓN ------");
        Console.WriteLine(ex.Message);
        if (ex.InnerException != null)
            Console.WriteLine($"Detalle: {ex.InnerException.Message}");
        Console.WriteLine("---------------------------------");
    }
}

// -------- Por si el navegador no va al lugar adecuado --------
app.MapGet("/", context =>
{
    context.Response.Redirect("/index.html");
    return Task.CompletedTask;
});

app.Run();
