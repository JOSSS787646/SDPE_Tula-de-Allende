using MediatR;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Prog.Commands.UpdateProg;
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
            // 1️⃣ Buscar por ID
            var proyect = await _repository.GetByIdAsync(request.idProyect);

            if (proyect == null)
                return false;

            // 2️⃣ Modificar
            proyect.Code = request.Code;
            proyect.Description = request.Description;
            proyect.Active = request.Active;
            proyect.UpdatedBy = _currentUser.UserId;
            proyect.UpdatedAt = DateTime.Now;

            // 3️⃣ Guardar
            return await _repository.UpdateAsync(proyect);
        }
    }

}
