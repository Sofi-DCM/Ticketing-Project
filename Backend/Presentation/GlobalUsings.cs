
global using Infrastructure.Persistence;
global using Microsoft.EntityFrameworkCore;
global using Presentation.Middlewares;
global using Domain.Entities;
global using Domain.Constants;
global using Microsoft.AspNetCore.Mvc;
global using Domain.Exceptions;
global using System.Data;
global using System.Net;
global using System.Text.Json;
global using Application.Response;
global using Application.Interfaces.Payments;
global using Infrastructure.Payments;


// ----- for dependency injection -----

global using Application.Interfaces.Repositories;
global using Infrastructure.Repositories;

//--- User
global using Application.Interfaces.Handlers._User;

global using Application.UseCase._User.Commands.CreateUser;
global using Application.UseCase._User.Queries.GetUserById;
global using Application.UseCase._User.Queries.ValidateUserCredentials;
global using Application.UseCase._User.Queries.GetUserReservationsById;

//--- AuditLog
global using Application.Interfaces.Handlers._AuditLog;

global using Application.UseCase._AuditLog.Commands.CreateAuditLog;

//--- Reservation
global using Application.Interfaces.Handlers._Reservation;

global using Application.UseCase._Reservation.Commands.CreateReservation;

//--- Seat
global using Application.Interfaces.Handlers._Seat;

global using Application.UseCase._Seat.Commands.ChangeSeatStatus;
global using Application.UseCase._Reservation.Commands.ExpireReservations;
global using Application.UseCase._Seat.Commands.CreateSeatsForSector;
global using Application.UseCase._Seat.Queries.GetSeatsBySector;
global using Infrastructure.BackgroundJobs;

//--- Event
global using Application.Interfaces.Handlers._Event;

global using Application.UseCase._Event.Queries.GetActiveEvents;
global using Application.UseCase._Event.Commands.CreateEvent;
global using Application.UseCase._Event.Queries.GetEventById;

//--- Sector
global using Application.Interfaces.Handlers._Sector;
global using Application.UseCase._Sector.Queries;
global using Application.UseCase._Sector.Commands.CreateSector;
