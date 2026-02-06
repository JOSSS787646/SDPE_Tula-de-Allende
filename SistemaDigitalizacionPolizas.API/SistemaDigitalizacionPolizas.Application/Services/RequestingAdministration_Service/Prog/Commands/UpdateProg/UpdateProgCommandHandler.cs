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
        private readonly IProgRepository _Progrepository;
        private readonly ICurrentUserService _currentUserService;

        public UpdateProgCommandHandler(IProgRepository repository, ICurrentUserService currentUserService)
        {
            _Progrepository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(
           UpdateProgCommand request,
           CancellationToken cancellationToken)
        {
            var prog = new Domain.Entities.RequestingAdministration_Entities.Prog
            {
                Code = request.Code,
                Description = request.Description,
                Active = request.Active,
                UpdatedBy = _currentUserService.UserId,
                UpdatedAt = DateTime.Now
            };

            var result = await _Progrepository.UpdateAsync(prog);

            if (!result)
            {
                throw new InvalidOperationException(
                    $"No se pudo actualizar el proyecto con código {request.Code}");
            }

            return true;
        }
    }
}
