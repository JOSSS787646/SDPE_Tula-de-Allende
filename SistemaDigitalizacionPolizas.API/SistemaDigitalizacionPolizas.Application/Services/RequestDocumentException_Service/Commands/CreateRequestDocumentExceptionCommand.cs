using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestDocumentException_Service.Commands
{
    public record CreateRequestDocumentExceptionCommand
 (
     int IdRequest,
     int IdDocumentType,
     bool DoesNotApply,
     string Justification,
     int? CreatedBy
 ) : IRequest<int>; // Retorna el Id creado
}
