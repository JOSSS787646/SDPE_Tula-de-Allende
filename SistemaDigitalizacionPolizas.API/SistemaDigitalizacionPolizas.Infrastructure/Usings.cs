//Usings globales de las entidades
global using Microsoft.EntityFrameworkCore;
//Referencias a JWT Y relacionado a la seguridad
global using Microsoft.Extensions.Configuration;
//Inyecciond e Dependencias y MediatR
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Options;
global using Microsoft.IdentityModel.Tokens;
global using SistemaDigitalizacionPolizas.Domain.Dtos.Auth;
global using SistemaDigitalizacionPolizas.Domain.Entities.Areas_Entities;
global using SistemaDigitalizacionPolizas.Domain.Entities.Permissions_Entities;
global using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Auth;
global using SistemaDigitalizacionPolizas.Infrastructure.Persistence;
global using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Auth_Persistences;
global using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Permissions_Persistences;
global using System;
global using System.Collections.Generic;
global using System.IdentityModel.Tokens.Jwt;
global using System.Linq;
global using System.Security.Claims;
global using System.Text;
global using System.Threading.Tasks;
