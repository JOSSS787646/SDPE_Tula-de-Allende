using SistemaDigitalizacionPolizas.Application.Services.Role_Service.Commands.CreatedRole;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Cod_Service.Commands.CreateCog
{
    public class CreateCogCommandHandler
          : IRequestHandler<CreateCogCommand, int>
    {
        private readonly ICogRepository _repository;
        public CreateCogCommandHandler(ICogRepository repository)
        {
            _repository = repository;
        }
        public async Task<int> Handle(
            CreateCogCommand request,
            CancellationToken cancellationToken)
        {
            var cog = new COG
            {
                idCog = request.idCog,
                Code = request.Code,
                Description = request.Description,
                Active = request.Active
            };
            await _repository.AddAsync(cog);
            return cog.idCog;
        }





    }
}
