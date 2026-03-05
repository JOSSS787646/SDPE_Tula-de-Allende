using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.RequestDocumentException_Service.Commands.SaveRequestDocumentExceptions
{
    public record ToggleRequestDocumentExceptionMassCommand
   (
       int IdRequest,
       List<RequestDocumentExceptionItem> Documents
   ) : IRequest<bool>;

    public record RequestDocumentExceptionItem
    (
        int IdDocumentType,
        bool DoesNotApply,
        string Justification
    );
}
