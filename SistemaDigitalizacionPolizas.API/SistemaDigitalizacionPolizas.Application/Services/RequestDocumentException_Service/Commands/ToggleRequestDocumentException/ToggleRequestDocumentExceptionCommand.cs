using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestDocumentException_Service.Commands.ToggleRequestDocumentException
{
    public record ToggleRequestDocumentExceptionCommand
    (
        int IdRequest,
        int IdDocumentType,
        bool DoesNotApply,
        int? UpdatedBy
    ) : IRequest<bool>;
}
