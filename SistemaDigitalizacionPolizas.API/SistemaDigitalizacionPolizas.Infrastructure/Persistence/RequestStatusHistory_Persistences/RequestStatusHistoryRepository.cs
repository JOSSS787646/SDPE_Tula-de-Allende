using SistemaDigitalizacionPolizas.Domain.Entities.RequestStatusHistory_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.IRequestStatusHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.RequestStatusHistory_Persistences
{

    public class RequestStatusHistoryRepository : IRequestStatusHistoryRepository
    {
        private readonly SdpeDbContext _context;

        public RequestStatusHistoryRepository(SdpeDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(RequestStatusHistory entity)
        {
            await _context.RequestStatusHistories.AddAsync(entity);
        }
    }
}
