using MediatR;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Proyect_Service.Commands.UpdateProyect
{
    public class UpdateProyectCommandHandler
    : IRequestHandler<UpdateProyectCommand, bool>
    {
        private readonly IProyectRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public UpdateProyectCommandHandler(
            IProyectRepository repository,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(
            UpdateProyectCommand request,
            CancellationToken cancellationToken)
        {
            var proyect = new Proyect
            {
                Code = request.Code,
                Description = request.Description,
                Active = request.Active,
                UpdatedBy = _currentUser.UserId,
                UpdatedAt = DateTime.Now
            };

            var result = await _repository.UpdateAsync(proyect);

            if (!result)
            {
                throw new InvalidOperationException(
                    $"No se pudo actualizar el proyecto con código {request.Code}");
            }

            return true;
        }
    }

}
