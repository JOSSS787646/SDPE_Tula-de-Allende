using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Proyect_Service.Commands.CreatedProyect;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Prog.Commands.CreateProg
{
    public class CreateProgCommandHandler
        : IRequestHandler<CreateProgCommand, int>
    {
        private readonly IProgRepository _Progrepository;
        private readonly ICurrentUserService _currentUserService;


        public CreateProgCommandHandler(IProgRepository repository, ICurrentUserService currentUserService)
        {
            _Progrepository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(
          CreateProgCommand request,
          CancellationToken cancellationToken)
        {
            var prog = new Domain.Entities.RequestingAdministration_Entities.Prog
            {
                Code = request.Code,
                Description = request.Description,
                Active = true,
                CreatedBy = _currentUserService.UserId,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _Progrepository.AddAsync(prog);

            if (result == null)
                throw new InvalidOperationException(
                    $"Ya existe un Proyecto con el código {request.Code}");

            return result.idProg;
        }
    }
}
