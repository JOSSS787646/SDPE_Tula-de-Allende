using SistemaDigitalizacionPolizas.Domain.Dtos.RequestManager;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.CreateRequestManager_Service.Commands.UpdateRequestManager
{
    public class UpdateRequestManagerCommandHandler
        : IRequestHandler<UpdateRequestManagerCommand, bool>
    {
        private readonly IRequestManagerRepository _repository;

        public UpdateRequestManagerCommandHandler(IRequestManagerRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateRequestManagerCommand request,
            CancellationToken cancellationToken)
        {
            var dto = new UpdateRequestManagerDto
            {
                IdRequestManager = request.IdRequestManager,
                IdAdministrativeUnit = request.IdAdministrativeUnit,
                FirstName = request.FirstName,
                LastName = request.LastName,
                SecondLastName = request.SecondLastName,
                Email = request.Email,
                Phone = request.Phone
            };

            return await _repository.UpdateAsync(dto);
        }
    }
}
