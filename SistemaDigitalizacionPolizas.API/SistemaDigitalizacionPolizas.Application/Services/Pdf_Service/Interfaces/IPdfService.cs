using SistemaDigitalizacionPolizas.Domain.Dtos.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Pdf_Service
{
    public interface IPdfService
    {
        /// <summary>
        /// Documento 2 — Resumen completo de la solicitud (diseño moderno con colores).
        /// </summary>
        byte[] GenerateAcquisitionRequestPdf(AcquisitionRequestPdfDto dto);

        /// <summary>
        /// Documento 1 — Réplica del formulario físico (mismo layout que el impreso).
        /// </summary>
        byte[] GenerateAcquisitionRequestFormPdf(AcquisitionRequestFormPdfDto dto);

        /// <summary>
        /// Documento 1 — Réplica del formulario físico del check list (mismo layout que el impreso).
        /// </summary>
        byte[] GenerateChecklistPdf(AcquisitionChecklistPdfDto dto);

    }
}
