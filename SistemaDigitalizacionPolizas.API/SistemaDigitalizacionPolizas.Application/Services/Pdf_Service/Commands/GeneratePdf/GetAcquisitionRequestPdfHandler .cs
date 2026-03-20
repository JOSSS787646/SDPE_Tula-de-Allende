using FluentValidation;
using SistemaDigitalizacionPolizas.Application.Services.Pdf_Service.Query.GetAcquisitionRequestPdf;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Pdf_Service.Commands.GeneratePdf
{
    public class GetAcquisitionRequestPdfHandler
       : IRequestHandler<GetAcquisitionRequestPdfCommand, byte[]>
    {
        private readonly IAcquisitionRequest _repository;
        private readonly IPdfService _pdfService;

        public GetAcquisitionRequestPdfHandler(
            IAcquisitionRequest repository,
            IPdfService pdfService)
        {
            _repository = repository;
            _pdfService = pdfService;
        }

        public async Task<byte[]> Handle(
            GetAcquisitionRequestPdfCommand request,
            CancellationToken cancellationToken)
        {
            // 🔥 1. Obtener datos
            var entity = await _repository.GetByIdWithDetailsAsync(request.RequestId);

            if (entity == null)
                throw new Exception("Request not found");

            // 🔥 2. Mapear a DTO
            var dto = AcquisitionRequestMapper.ToPdfDto(entity);

            // 🔥 3. Generar PDF
            return _pdfService.GenerateAcquisitionRequestPdf(dto);
        }
    }
}
