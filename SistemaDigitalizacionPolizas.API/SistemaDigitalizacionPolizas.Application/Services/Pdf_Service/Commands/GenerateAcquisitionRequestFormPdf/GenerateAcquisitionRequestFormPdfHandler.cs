using SistemaDigitalizacionPolizas.Domain.Dtos.Pdf;
using SistemaDigitalizacionPolizas.Domain.Entities.AcquisitionRequest_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Pdf_Service.Commands.GenerateAcquisitionRequestFormPdf
{
    public class GenerateAcquisitionRequestFormPdfHandler
       : IRequestHandler<GenerateAcquisitionRequestFormPdfCommand, byte[]>
    {
        private readonly IAcquisitionRequest _repository;
        private readonly IPdfService _pdfService;

        public GenerateAcquisitionRequestFormPdfHandler(
            IAcquisitionRequest repository,
            IPdfService pdfService)
        {
            _repository = repository;
            _pdfService = pdfService;
        }

        public async Task<byte[]> Handle(
            GenerateAcquisitionRequestFormPdfCommand request,
            CancellationToken cancellationToken)
        {
            // 🔹 1. Obtener datos completos
            var entity = await _repository.GetByIdWithDetailsAsync(request.RequestId);

            if (entity == null)
                throw new Exception("Solicitud no encontrada");

            // 🔹 2. Mapear a DTO (NUEVO)
            var dto = MapToFormDto(entity);

            // 🔹 3. Generar PDF
            return _pdfService.GenerateAcquisitionRequestFormPdf(dto);
        }

        // 🔥 MAPPER INTERNO (puedes moverlo después si quieres)
        private AcquisitionRequestFormPdfDto MapToFormDto(AcquisitionRequest entity)
        {
            return new AcquisitionRequestFormPdfDto
            {
                Folio = entity.RequestNumber,

                UnitKey = entity.AdministrativeUnit?.Code.ToString(),
                UnitName = entity.AdministrativeUnit?.Description,
                FundingSource = entity.FundingSource?.Description,
                Program = entity.Program?.Description,
                CogKey = entity.AcquisitionClassification?.Description,
                Project = entity.Project?.Description,
                RequestDate = entity.RequestDate,

                Justification = entity.Justification,

                // 🔹 Items principales
                Items = entity.Details.Select(d => new AcquisitionFormItemDto
                {
                    Quantity = d.Quantity,
                    UnitMeasure = d.UnitMeasure,
                    Description = d.Description,
                    UnitPrice = d.UnitAmount,
                    Total = d.TotalAmount
                }).ToList(),

                // 🔹 Firmas (puedes mejorar después con usuarios reales)
                ApplicantName = entity.Beneficiary?.FirstName,
                ReceivedAndQuotedBy = entity.Supplier?.ContactName,

                // 🔹 Validación
                AuthorizedBy = "TESORERO MUNICIPAL",
                QuotationResponsible = "DIRECTOR DE ADQUISICIONES",

                // 🔹 Secciones inferiores (puedes ajustar lógica después)
                CommittedItems = entity.Details.Select(d => new AcquisitionFormItemDto
                {
                    Quantity = d.Quantity,
                    UnitMeasure = d.UnitMeasure,
                    Description = d.Description,
                    UnitPrice = d.UnitAmount,
                    Total = d.TotalAmount
                }).ToList(),

                AccruedItems = entity.Details.Select(d => new AcquisitionFormItemDto
                {
                    Quantity = d.Quantity,
                    UnitMeasure = d.UnitMeasure,
                    Description = d.Description,
                    UnitPrice = d.UnitAmount,
                    Total = d.TotalAmount
                }).ToList(),

                Supplier = entity.Supplier?.ContactName,
                SupplierRFC = entity.Supplier?.Rfc,

                PaymentMethod = entity.PaymentPolicy?.PolicyCode,
                AuthorizedForPayment = true
            };
        }
    }
}
