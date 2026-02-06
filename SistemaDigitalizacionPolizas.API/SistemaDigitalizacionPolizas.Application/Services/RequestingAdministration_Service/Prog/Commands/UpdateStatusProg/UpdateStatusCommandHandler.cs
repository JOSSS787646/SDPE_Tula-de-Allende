using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Proyect_Service.Commands.UpdateStatusProyect;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Prog.Commands.UpdateStatusProg
{
    public class UpdateStatusCommandHandler
        : IRequestHandler<UpdateStatusProgCommand, bool>
    {
        private readonly IProgRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public UpdateStatusCommandHandler(IProgRepository repository, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(
         UpdateStatusProgCommand request,
         CancellationToken cancellationToken)
        {
            var prog = await _repository.GetByCodeAsync(request.Code);

            if (prog == null)
                return false;
            prog.Active = request.Active;
            prog.UpdatedBy = _currentUserService.UserId;
            prog.UpdatedAt = DateTime.Now;

            return await _repository.UpdateAsync(prog);
        }
    }
}
