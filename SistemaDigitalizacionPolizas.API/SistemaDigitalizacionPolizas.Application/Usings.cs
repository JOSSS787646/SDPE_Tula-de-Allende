global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using System.Threading.Tasks;
global using MediatR;
global using SistemaDigitalizacionPolizas.Domain.Dtos.Auth;
global using SistemaDigitalizacionPolizas.Application.AutoMapper;


//Llamado a libreias externas
global using BCrypt.Net;

//Llamdo a 

//Llaamdo de las interfaces y repostirios
global using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Auth;
global using SistemaDigitalizacionPolizas.Application.Services.Auth_Service.Feature.CRUD.Command.Login;


//Llamada a los Dtos
global using SistemaDigitalizacionPolizas.Domain.Dtos.Permission;
global using SistemaDigitalizacionPolizas.Domain.Dtos.User;



//Inyeccion de Dependecias y mediatr

global using Microsoft.Extensions.DependencyInjection;
global using SistemaDigitalizacionPolizas.Application.Common.Behaviours;
global using System.Reflection;


//Los imports en relacion con Handlers
global using SistemaDigitalizacionPolizas.Domain.Entities.Areas_Entities;
global using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.AdministrtiveUnit;

