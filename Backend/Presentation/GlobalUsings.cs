
//Archivo de usings globales para evitar el enchastre de usings en el Program.cs

global using Infrastructure.Persistence;
global using Microsoft.EntityFrameworkCore;

// ----- for dependency injection -----

global using Application.Interfaces.Repositories;
global using Infrastructure.Repositories;

//--- User
global using Application.Interfaces.Handlers._User;

global using Application.UseCase._User.Commands.CreateUser;
global using Application.UseCase._User.Queries.GetUserById;

//--- AuditLog
global using Application.Interfaces.Handlers._AuditLog;

global using Application.UseCase._AuditLog.Commands.CreateAuditLog;
