using MediatR;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Proyect_Service.Commands.CreatedProyect
{
    public class CreatedProyectCommandHandler
        : IRequestHandler<CreatedProyectCommand, int>
    {
        private readonly IProyectRepository _proyectRepository;
        private readonly ICurrentUserService _currentUser;

        public CreatedProyectCommandHandler(
            IProyectRepository proyectRepository,
            ICurrentUserService currentUser)
        {
            _proyectRepository = proyectRepository;
            _currentUser = currentUser;
        }

        public async Task<int> Handle(
            CreatedProyectCommand request,
            CancellationToken cancellationToken)
        {
            var proyect = new Proyect
            {
                Code = request.Code,
                Description = request.Description,
                Active = true,
                CreatedBy = _currentUser.UserId,
                CreatedAt = DateTime.Now
            };

            var result = await _proyectRepository.AddAsync(proyect);

            if (result == null)
                throw new InvalidOperationException(
                    $"Ya existe un Proyecto con el código {request.Code}");

            return result.idProyect;
        }
    }
}
