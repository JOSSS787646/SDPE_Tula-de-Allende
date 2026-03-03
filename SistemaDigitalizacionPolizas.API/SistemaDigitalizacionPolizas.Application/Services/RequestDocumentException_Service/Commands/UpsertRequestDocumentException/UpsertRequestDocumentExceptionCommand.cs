using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestDocumentException_Service.Commands.UpsertRequestDocumentException
{
    public record UpsertRequestDocumentExceptionCommand(
      int IdRequest,
     int IdDocumentType,
     bool DoesNotApply,
     string Justification,
     int? CreatedBy
) : IRequest<bool>;
}
