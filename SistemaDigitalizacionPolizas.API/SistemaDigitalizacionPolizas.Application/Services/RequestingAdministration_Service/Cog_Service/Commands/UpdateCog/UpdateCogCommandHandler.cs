using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Commands.UpdateCog
{
    public class UpdateCogCommandHandler
    : IRequestHandler<UpdateCogCommand, bool>
    {
        private readonly ICogRepository _repository;
        private readonly ICurrentUserService _currentUserService;
        public UpdateCogCommandHandler(ICogRepository repository, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }


        public async Task<bool> Handle(
      UpdateCogCommand request,
      CancellationToken cancellationToken)
        {
            // 1️⃣ Buscar por ID
            var cog = await _repository.GetByIdAsync(request.idCog);

            if (cog == null)
                return false;

            // 2️⃣ Modificar
            cog.Code = request.Code;
            cog.Description = request.Description;
            cog.Active = request.Active;
            cog.UpdatedBy = _currentUserService.UserId;
            cog.UpdatedAt = DateTime.Now;

            // 3️⃣ Guardar
            return await _repository.UpdateAsync(cog);
        }
    }
}
