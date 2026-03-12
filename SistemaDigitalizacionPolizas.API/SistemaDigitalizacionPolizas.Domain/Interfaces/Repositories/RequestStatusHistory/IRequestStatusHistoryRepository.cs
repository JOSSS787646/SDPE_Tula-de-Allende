using SistemaDigitalizacionPolizas.Domain.Entities.RequestStatusHistory_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.IRequestStatusHistory
{
    public interface IRequestStatusHistoryRepository
    {
        Task AddAsync(RequestStatusHistory entity);
    }
}
