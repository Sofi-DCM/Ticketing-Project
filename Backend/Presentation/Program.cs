
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
//HOLAAAAAAAA

// User
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICreateUserHandler, CreateUserHandler>();
builder.Services.AddScoped<IGetUserByIdHandler, GetUserByIdHandler>();
builder.Services.AddScoped<IValidateUserCredentialsHandler, ValidateUserCredentialsHandler>();

//AuditLog
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddScoped<ICreateAuditLogHandler, CreateAuditLogHandler>();

//Event
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IGetActiveEventsHandler, GetActiveEventsHandler>();
builder.Services.AddScoped<ICreateEventHandler, CreateEventHandler>();

// Reservation
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<ICreateReservationHandler, CreateReservationHandler>();

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

var app = builder.Build();

// -------- Dinamic Seeds --------

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    if (!context.Events.Any())
    {
        var newEvent = new Event
        {
            Name = "The Eras Tour",
            EventDate = DateTime.Now.AddMonths(3),
            Venue = "Movistar Arena",
            Status = EventStatusConstants.Active
        };

        context.Events.Add(newEvent);
        context.SaveChanges();

        var sectorsNames = new[] { "Platea Baja", "Platea Alta" };
        var rowIdentifiers = new[] {"A", "B", "C", "D", "E" };

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

// -------- Use Middleware --------
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors();

// Apply migrations automatically
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();
