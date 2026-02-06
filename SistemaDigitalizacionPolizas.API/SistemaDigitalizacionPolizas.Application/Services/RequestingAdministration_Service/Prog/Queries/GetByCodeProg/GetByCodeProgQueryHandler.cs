using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Prog.Queries.GetByCodeProg;
using SistemaDigitalizacionPolizas.Domain.Dtos.RequestingAdministration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;
using MediatR;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Prog.Queries.GetByCodeProg
{
    public class GetByCodeProgQueryHandler
        : IRequestHandler<GetByCodeProgQuery, ProgDto?>
    {
        private readonly IProgRepository _repository;

        public GetByCodeProgQueryHandler(IProgRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProgDto?> Handle(
            GetByCodeProgQuery request,
            CancellationToken cancellationToken)
        {
            var prog = await _repository.GetByCodeAsync(request.code);

            if (prog == null)
                return null;

            return new ProgDto
            {
                idProg = prog.idProg,  
                Code = prog.Code,
                Description = prog.Description,
                Active = prog.Active
            };
        }
    }
}
