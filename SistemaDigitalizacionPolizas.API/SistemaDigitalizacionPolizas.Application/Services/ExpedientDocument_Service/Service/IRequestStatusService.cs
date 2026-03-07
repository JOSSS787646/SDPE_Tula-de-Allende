using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service
{
    public interface IRequestStatusService
    {
        Task RecalculateStatus(int requestId);
    }
}
