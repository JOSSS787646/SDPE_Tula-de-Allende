using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;
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

        public UpdateCogCommandHandler(ICogRepository repository)
        {
            _repository = repository;
        }


        public async Task<bool> Handle(
            UpdateCogCommand request,
            CancellationToken cancellationToken)
        {
            var cog = new COG
            {
                Code = request.Code,
                Description = request.Description,
                Active = request.Active
            };

            return await _repository.UpdateAsync(cog);
        }
    }
}
