using SistemaDigitalizacionPolizas.Domain.Entities.Areas_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.AdministrativeUnit_Service.Future.CRUD.Command.CreatedAdministrativeUnit
{/*
    public class CreateAdministrativeUnitCommandHandler
    : IRequestHandler<CreateAdministrativeUnitCommand, int>
    {
        private readonly SdpeDbContext _context;

        public CreateAdministrativeUnitCommandHandler(SdpeDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(
            CreateAdministrativeUnitCommand request,
            CancellationToken cancellationToken)
        {
            var entity = new AdministrativeUnit
            {
                Code = request.Code,
                Description = request.Description
            };

            _context.AdministrativeUnits.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.IdAdministrativeUnit;
        }
    }*/
}
