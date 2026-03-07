using SistemaDigitalizacionPolizas.Domain.Dtos.RequestManager;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.CreateRequestManager_Service.Commands.CreateRequestManager
{
    public class CreateRequestManagerCommandHandler
       : IRequestHandler<CreateRequestManagerCommand, int>
    {
        private readonly IRequestManagerRepository _repository;

        public CreateRequestManagerCommandHandler(IRequestManagerRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(
            CreateRequestManagerCommand request,
            CancellationToken cancellationToken)
        {
            var dto = new CreateRequestManagerDto
            {
                IdRequest = request.IdRequest,
                IdAdministrativeUnit = request.IdAdministrativeUnit,
                FirstName = request.FirstName,
                LastName = request.LastName,
                SecondLastName = request.SecondLastName,
                Email = request.Email,
                Phone = request.Phone
            };

            return await _repository.AddAsync(dto);
        }
    }
}
