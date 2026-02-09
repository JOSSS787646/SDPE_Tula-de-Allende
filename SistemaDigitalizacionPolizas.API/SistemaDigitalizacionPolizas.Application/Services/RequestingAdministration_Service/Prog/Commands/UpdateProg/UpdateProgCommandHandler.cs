using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Commands.UpdateCog;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Proyect_Service.Commands.UpdateProyect;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Prog.Commands.UpdateProg
{
    public class UpdateProgCommandHandler
        : IRequestHandler<UpdateProgCommand, bool>
    {
        private readonly IProgRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public UpdateProgCommandHandler(IProgRepository repository, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(
     UpdateProgCommand request,
     CancellationToken cancellationToken)
        {
            // 1️⃣ Buscar por ID
            var progr = await _repository.GetByIdAsync(request.idProg);

            if (progr == null)
                return false;

            // 2️⃣ Modificar
            progr.Code = request.Code;
            progr.Description = request.Description;
            progr.Active = request.Active;
            progr.UpdatedBy = _currentUserService.UserId;
            progr.UpdatedAt = DateTime.Now;

            // 3️⃣ Guardar
            return await _repository.UpdateAsync(progr);
        }
    }
}
