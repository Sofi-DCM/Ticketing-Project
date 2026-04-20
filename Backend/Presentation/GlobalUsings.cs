
//Archivo de usings globales para evitar el enchastre de usings en el Program.cs

global using Infrastructure.Persistence;
global using Microsoft.EntityFrameworkCore;

// ----- for dependency injection -----

global using Application.Interfaces.Handlers;

global using Application.Interfaces.Repositories;
global using Infrastructure.Repositories;

global using Application.UseCase._User.Commands.CreateUser;