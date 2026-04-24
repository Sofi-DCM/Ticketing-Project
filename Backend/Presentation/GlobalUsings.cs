
global using Infrastructure.Persistence;
global using Microsoft.EntityFrameworkCore;
global using Presentation.Middlewares;
global using Domain.Entities;
global using Domain.Constants;

// ----- for dependency injection -----

global using Application.Interfaces.Repositories;
global using Infrastructure.Repositories;

//--- User
global using Application.Interfaces.Handlers._User;

global using Application.UseCase._User.Commands.CreateUser;
global using Application.UseCase._User.Queries.GetUserById;
global using Application.UseCase._User.Queries.ValidateUserCredentials;

//--- AuditLog
global using Application.Interfaces.Handlers._AuditLog;

global using Application.UseCase._AuditLog.Commands.CreateAuditLog;

//--- Event
global using Application.Interfaces.Handlers._Event;

global using Application.UseCase._Event.Queries.GetActiveEvents;