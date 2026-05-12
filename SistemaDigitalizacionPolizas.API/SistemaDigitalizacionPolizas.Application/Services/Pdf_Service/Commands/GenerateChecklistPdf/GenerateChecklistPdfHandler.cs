using MediatR;
using SistemaDigitalizacionPolizas.Application.Services.Pdf_Service;
using SistemaDigitalizacionPolizas.Application.Services.Pdf_Service.Commands.GenerateChecklistPdf;
using SistemaDigitalizacionPolizas.Domain.Dtos.AcquisitionRequest;
using SistemaDigitalizacionPolizas.Domain.Dtos.Pdf;
using SistemaDigitalizacionPolizas.Domain.Entities.AcquisitionRequest_Entities;
using SistemaDigitalizacionPolizas.Domain.Enums;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;

public class GenerateChecklistPdfHandler
    : IRequestHandler<GenerateChecklistPdfCommand, PdfFileResultDto>
{
    private readonly IAcquisitionRequest _requestRepository;
    private readonly IDocumentExpedientRepository _documentRepository;
    private readonly IPdfService _pdfService;

    public GenerateChecklistPdfHandler(
        IAcquisitionRequest requestRepository,
        IDocumentExpedientRepository documentRepository,
        IPdfService pdfService)
    {
        _requestRepository = requestRepository;
        _documentRepository = documentRepository;
        _pdfService = pdfService;
    }

    public async Task<PdfFileResultDto> Handle(
        GenerateChecklistPdfCommand request,
        CancellationToken cancellationToken)
    {
        // 🔹 1. Obtener solicitud
        var entity = await _requestRepository
            .GetByIdWithDetailsAsync(request.RequestId);

        if (entity == null)
            throw new Exception("Solicitud no encontrada");

        // 🔹 2. Obtener checklist dinámico
        var checklist = await _documentRepository
            .GetChecklistByRequestAsync(request.RequestId);

        // 🔹 3. Mapear
        var dto = MapToChecklistPdfDto(entity, checklist);

        // 🔹 4. Generar PDF
        var pdfBytes = _pdfService.GenerateChecklistPdf(dto);

        // 🔥 5. Retornar PDF + nombre del archivo
        return new PdfFileResultDto
        {
            Content = pdfBytes,
            FileName = $"Checklist_{entity.RequestNumber}.pdf"
        };
    }

    // 🔥 MAPPER PROFESIONAL
    private AcquisitionChecklistPdfDto MapToChecklistPdfDto(
        AcquisitionRequest entity,
        List<RequestDocumentChecklistDto> checklist)
    {
        int order = 1;

        return new AcquisitionChecklistPdfDto
        {
            // ── CABECERA ─────────────────────────────
            Folio = entity.RequestNumber,
            ClassificationName = entity.AcquisitionClassification?.Description,
            RequestDate = entity.RequestDate,
            Status = entity.ApplicationStatus?.Description,

            AdministrativeUnit = entity.AdministrativeUnit?.Description,
            ApplicantName = entity.Beneficiary?.FirstName,
            Program = entity.Program?.Description,
            Project = entity.Project?.Description,

 

            // ── CHECKLIST ────────────────────────────
            Checklist = checklist.Select(doc =>
            {
                var files = doc.Files ?? new List<DocumentFileDto>();

                // 🔥 1. ESTADO REAL (EXCEPCIONES)
                var exceptionStatus = files
                    .OrderByDescending(f => f.Status != null ? f.Status.Id : 0)
                    .Select(f => f.Status?.Description)
                    .FirstOrDefault(s => !string.IsNullOrWhiteSpace(s));

                // 🔥 2. FALLBACK
                var approved = files.Count(f =>
                    f.Status != null &&
                    f.Status.Id == (int)DocumentStatusEnum.Aprobado);

                string fallbackStatus;

                if (!doc.RequiredByRule || doc.NoApplies)
                    fallbackStatus = "NO_APLICA";
                else if (!files.Any())
                    fallbackStatus = "PENDIENTE";
                else if (approved == files.Count)
                    fallbackStatus = "APROBADO";
                else
                    fallbackStatus = "EN_REVISION";

                return new ChecklistPdfItemDto
                {
                    Order = order++,
                    DocumentName = doc.DocumentName,

                    RequiredByRule = doc.RequiredByRule,
                    NoApplies = doc.NoApplies,
                    Uploaded = doc.Uploaded,

                    // 🔥 PRINCIPAL
                    ExceptionStatus = exceptionStatus ?? fallbackStatus,

                    // ⚠️ LEGACY
                    GlobalStatus = fallbackStatus,

                    Observations = files
                        .Where(f => !string.IsNullOrWhiteSpace(f.ObservationUpload))
                        .Select(f => f.ObservationUpload!)
                        .ToList(),

                    Files = files.Select(f => new ChecklistFilePdfDto
                    {
                        FileId = f.FileId,
                        FileName = f.FileName,
                        FileUrl = f.FileUrl,
                        Status = f.Status?.Description
                    }).ToList()
                };
            }).ToList()
        };
    }
}