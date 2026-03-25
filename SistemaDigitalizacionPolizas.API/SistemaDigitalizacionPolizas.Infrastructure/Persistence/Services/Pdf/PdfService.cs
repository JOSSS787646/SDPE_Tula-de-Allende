using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SistemaDigitalizacionPolizas.Application.Services.Pdf_Service;
using SistemaDigitalizacionPolizas.Domain.Dtos.Pdf;
using System.Globalization;
using System.Reflection;

namespace SistemaDigitalizacionPolizas.Infrastructure.Services
{
    public class PdfService : IPdfService
    {
        // ── Paleta institucional ──────────────────────────────
        private const string CP = "#6A1B1B";
        private const string CS = "#8B2C2C";
        private const string CA = "#FDF2F2";
        private const string CG = "#F5F5F5";
        private const string CT = "#6B7280";
        private const string CDB = "#C9A227";
        private const string CDL = "#F4E4BC";
        private const string CV = "#166534";
        private const string CVF = "#DCFCE7";
        private const string CN = "#92400E";
        private const string CNF = "#FEF3C7";

        // Paleta estados checklist
        private const string CBlue = "#1D4ED8";
        private const string CBluF = "#EFF6FF";
        private const string CBluB = "#BFDBFE";
        private const string CRed = "#991B1B";
        private const string CRedF = "#FEF2F2";
        private const string CRedB = "#FCA5A5";

        private static readonly CultureInfo MX = new("es-MX");

        // ════════════════════════════════════════════════════════
        //  LOGO
        // ════════════════════════════════════════════════════════
        private byte[]? GetLogo()
        {
            var asm = Assembly.GetExecutingAssembly();
            var name = "SistemaDigitalizacionPolizas.Infrastructure.Resources.Images.LogoPresidencia.webp";
            using var s = asm.GetManifestResourceStream(name);
            if (s == null) return null;
            using var ms = new MemoryStream();
            s.CopyTo(ms);
            return ms.ToArray();
        }

        // ════════════════════════════════════════════════════════
        //  DOC 1 — Formulario físico
        // ════════════════════════════════════════════════════════
        public byte[] GenerateAcquisitionRequestFormPdf(AcquisitionRequestFormPdfDto dto)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            return Document.Create(c => c.Page(p =>
            {
                p.Size(PageSizes.Letter);
                p.MarginHorizontal(28);
                p.MarginTop(28);
                p.MarginBottom(28);
                p.Header().Element(h => FormHeader(h, dto));
                p.Content().Element(h => FormContent(h, dto));
            })).GeneratePdf();
        }

        // ════════════════════════════════════════════════════════
        //  DOC 2 — Resumen completo
        // ════════════════════════════════════════════════════════
        public byte[] GenerateAcquisitionRequestPdf(AcquisitionRequestPdfDto dto)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            return Document.Create(c => c.Page(p =>
            {
                p.Size(PageSizes.Letter);
                p.MarginHorizontal(36);
                p.MarginTop(36);
                p.MarginBottom(50);
                p.Header().Element(h => SummaryHeader(h, dto));
                p.Content().Element(h => SummaryContent(h, dto));
                p.Footer().Element(SharedFooter);
            })).GeneratePdf();
        }

        // ════════════════════════════════════════════════════════
        //  DOC 3 — Check List de Adquisiciones
        // ════════════════════════════════════════════════════════
        public byte[] GenerateChecklistPdf(AcquisitionChecklistPdfDto dto)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            return Document.Create(c => c.Page(p =>
            {
                p.Size(PageSizes.Letter);
                p.MarginHorizontal(32);
                p.MarginTop(28);
                p.MarginBottom(46);
                p.Header().Element(h => ChecklistHeader(h, dto));
                p.Content().Element(h => ChecklistContent(h, dto));
                p.Footer().Element(SharedFooter);
            })).GeneratePdf();
        }

        // ════════════════════════════════════════════════════════
        //  ██  DOC 3 — CHECKLIST — IMPLEMENTACIÓN
        // ════════════════════════════════════════════════════════

        private void ChecklistHeader(IContainer container, AcquisitionChecklistPdfDto dto)
        {
            var logo = GetLogo();
            container.Column(col =>
            {
                col.Item().Background(CP).Height(5);

                col.Item().PaddingTop(8).PaddingBottom(8).Row(row =>
                {
                    if (logo != null)
                        row.ConstantItem(55).Height(55).Image(logo, ImageScaling.FitArea);
                    else
                        row.ConstantItem(55).Height(55);

                    row.ConstantItem(12);

                    row.RelativeItem().AlignMiddle().Column(tc =>
                    {
                        tc.Item().Text("SECRETARÍA DE LA TESORERÍA Y ADMINISTRACIÓN")
                            .Bold().FontSize(8).FontColor(CT);
                        tc.Item().PaddingTop(3)
                            .Text("CHECK LIST PARA PROCESO DE ADQUISICIONES")
                            .Bold().FontSize(14).FontColor(CP);
                        if (!string.IsNullOrWhiteSpace(dto.ClassificationName))
                            tc.Item().PaddingTop(2)
                                .Text($"({dto.ClassificationName.ToUpper()})")
                                .Bold().FontSize(10).FontColor(CS);
                    });

                    row.ConstantItem(12);

                    // Caja folio / fecha / estado
                    row.ConstantItem(140).Background(CA).Border(1).BorderColor(CS)
                        .Padding(8).Column(ic =>
                        {
                            ic.Item().Text("FOLIO").FontSize(7).FontColor(CT).Bold();
                            ic.Item().Text(dto.Folio ?? "—")
                                .FontSize(13).Bold().FontColor(CP);

                            ic.Item().PaddingTop(5).Text("FECHA").FontSize(7).FontColor(CT).Bold();
                            ic.Item().Text((dto.RequestDate ?? DateTime.Now).ToString("dd/MM/yyyy"))
                                .FontSize(10).FontColor(CP);

                            if (!string.IsNullOrWhiteSpace(dto.Status))
                            {
                                var (stBg, stFg) = ResolveStatusColors(dto.Status);
                                ic.Item().PaddingTop(5).Background(stBg).Padding(3)
                                    .Text($"Estado: {dto.Status}")
                                    .FontSize(8).Bold().FontColor(stFg);
                            }
                        });
                });

                // Banda informativa
                col.Item().Background(CG).Border(1).BorderColor("#E5E7EB")
                    .PaddingVertical(6).PaddingHorizontal(12).Row(r =>
                    {
                        CLInfoChip(r, "Unidad", dto.AdministrativeUnit);
                        r.ConstantItem(18);
                        CLInfoChip(r, "Solicitante", dto.ApplicantName);
                        r.ConstantItem(18);
                        CLInfoChip(r, "Programa", dto.Program);
                        r.ConstantItem(18);
                        CLInfoChip(r, "Proyecto", dto.Project);
                    });

                col.Item().Background(CS).Height(2);
            });
        }

        private void CLInfoChip(RowDescriptor row, string label, string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return;
            row.AutoItem().Column(c =>
            {
                c.Item().Text(label).FontSize(7).FontColor(CT).Bold();
                c.Item().Text(value).FontSize(9).FontColor(CP);
            });
        }

        private void ChecklistContent(IContainer container, AcquisitionChecklistPdfDto dto)
        {
            container.PaddingTop(10).Column(col =>
            {
                col.Spacing(10);

                // Bloque de progreso
                col.Item().Element(c => ChecklistProgress(c, dto.Checklist));

                // Título
                col.Item().Element(c => SharedSectionTitle(c, "Documentos Requeridos"));

                // Tabla
                col.Item().Element(c => ChecklistMainTable(c, dto.Checklist));

                // Firmas
                col.Item().PaddingTop(16).Element(c => ChecklistSignatures(c, dto));
            });
        }

        // ── Bloque de progreso / contadores ──────────────────
        private void ChecklistProgress(IContainer container, List<ChecklistPdfItemDto> items)
        {
            if (items == null || !items.Any()) return;

            int total = items.Count;
            int approved = items.Count(i => NormalizeCode(i.ExceptionStatus) == "APROBADO");
            int pending = items.Count(i => NormalizeCode(i.ExceptionStatus) is "PENDIENTE" or "" && !i.NoApplies);
            int review = items.Count(i => NormalizeCode(i.ExceptionStatus) == "EN_REVISION");
            int na = items.Count(i => i.NoApplies);
            int active = total - na;
            int pct = active > 0 ? (int)Math.Round((double)approved / active * 100) : 0;

            container.Text($"Avance: {pct}% ({approved}/{active})");
        }

        private void CLStatBox(RowDescriptor row, string value, string label,
                                string bg, string fg, string border)
        {
            row.ConstantItem(65).Background(bg).Border(1).BorderColor(border)
                .Padding(6).Column(c =>
                {
                    c.Item().Text(value).FontSize(18).Bold().FontColor(fg);
                    c.Item().Text(label).FontSize(7).FontColor(fg);
                });
        }

        // ── Tabla principal ───────────────────────────────────
        private void ChecklistMainTable(IContainer container, List<ChecklistPdfItemDto> items)
        {
            if (items == null || !items.Any())
            {
                container.Border(1).BorderColor("#E5E7EB").Padding(10)
                    .Text("Sin documentos registrados.").FontSize(9).FontColor(CT);
                return;
            }

            container.Table(t =>
            {
                t.ColumnsDefinition(cd =>
                {
                    cd.ConstantColumn(28);
                    cd.RelativeColumn();
                    cd.ConstantColumn(100);
                });

                t.Header(h =>
                {
                    h.Cell().Background(CP).Padding(7).AlignCenter()
                        .Text("Nº").Bold().FontSize(9).FontColor(Colors.White);
                    h.Cell().Background(CP).Padding(7)
                        .Text("CONTROL DE DOCUMENTACIÓN").Bold().FontSize(9).FontColor(Colors.White);
                    h.Cell().Background(CP).Padding(7).AlignCenter()
                        .Text("ESTATUS DE VALIDACIÓN").Bold().FontSize(9).FontColor(Colors.White);
                });

                bool odd = false;
                foreach (var item in items.OrderBy(x => x.Order))
                {
                    var rowBg = odd ? "#FFFFFF" : CG;
                    odd = !odd;

                    t.Cell().MinHeight(36).Background(rowBg)
                        .Border(1).BorderColor("#E5E7EB")
                        .AlignCenter().AlignMiddle()
                        .Text(item.Order.ToString())
                        .Bold().FontSize(12).FontColor(CP);

                    t.Cell().MinHeight(36).Background(rowBg)
                        .Border(1).BorderColor("#E5E7EB")
                        .Padding(7).Column(c =>
                        {
                            c.Item().Text(item.DocumentName)
                                .FontSize(9)
                                .FontColor("#374151");

                            if (item.Files != null && item.Files.Any())
                            {
                                c.Item().PaddingTop(3)
                                    .Text("Documento adjunto")
                                    .FontSize(7).FontColor(CV);
                            }
                            else if (!item.NoApplies)
                            {
                                c.Item().PaddingTop(3)
                                    .Text("Sin documento")
                                    .FontSize(7).FontColor(CT);
                            }

                            if (item.Observations != null)
                            {
                                foreach (var obs in item.Observations
                                    .Where(o => !string.IsNullOrWhiteSpace(o)))
                                {
                                    c.Item().PaddingTop(3)
                                        .Text(obs)
                                        .FontSize(7).Italic().FontColor(CT);
                                }
                            }
                        });

                    t.Cell().MinHeight(36).Background(rowBg)
                        .Border(1).BorderColor("#E5E7EB")
                        .AlignCenter().AlignMiddle()
                        .Element(c => CLStatusBadge(c, item.ExceptionStatus, item.NoApplies));
                }
            });
        }


        // ── Badge visual de estado ────────────────────────────
        private void CLStatusBadge(IContainer c, string? status, bool noApplies)
        {
            if (noApplies)
            {
                c.Text("N/A").FontSize(9).FontColor(CT).Bold();
                return;
            }

            var code = NormalizeCode(status);

            var (bg, fg, border, label) = code switch
            {
                "APROBADO" => (CVF, CV, "#86EFAC", "Aprobado"),
                "EN_REVISION" => (CBluF, CBlue, CBluB, "En revisión"),
                "RECHAZADO" => (CRedF, CRed, CRedB, "Rechazado"),
                "PENDIENTE" => (CNF, CN, "#FCD34D", "Pendiente"),
                _ => (CG, CT, "#D1D5DB", "Sin estado"),
            };

            c.Background(bg).Border(1).BorderColor(border)
                .Padding(5)
                .AlignCenter().AlignMiddle()
                .Text(label).FontSize(8).Bold().FontColor(fg);
        }

        // ── Firmas del checklist ──────────────────────────────
        private void ChecklistSignatures(IContainer container, AcquisitionChecklistPdfDto dto)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(c =>
                {
                    c.Item().Height(30); // espacio firma
                    c.Item().BorderBottom(1).BorderColor("#374151").PaddingBottom(3)
                        .Text(dto.AcquisitionsSignatoryName ?? "").FontSize(9).FontColor(CP);
                    c.Item().PaddingTop(4).Text("Nombre y firma").FontSize(8).Italic().FontColor(CT);
                    c.Item().Text(dto.AcquisitionsSignatoryTitle
                        ?? "Guadalupe Noguez Becerra / Adquisiciones")
                        .FontSize(8).Bold().FontColor(CP);
                });

                row.ConstantItem(50);

                row.RelativeItem().Column(c =>
                {
                    c.Item().Height(30);
                    c.Item().BorderBottom(1).BorderColor("#374151").PaddingBottom(3)
                        .Text(dto.TreasurySignatoryName ?? "").FontSize(9).FontColor(CP);
                    c.Item().PaddingTop(4).Text("Nombre y firma").FontSize(8).Italic().FontColor(CT);
                    c.Item().Text(dto.TreasurySignatoryTitle
                        ?? "Jaqueline Moreno Martínez / Tesorería")
                        .FontSize(8).Bold().FontColor(CP);
                });
            });
        }

        // ════════════════════════════════════════════════════════
        //  ██  DOC 1 — FORMULARIO FÍSICO — IMPLEMENTACIÓN
        // ════════════════════════════════════════════════════════
        private void FormHeader(IContainer container, AcquisitionRequestFormPdfDto dto)
        {
            var logo = GetLogo();
            container.Column(col =>
            {
                col.Item().Background(CP).Height(5);
                col.Item().PaddingVertical(6).Row(row =>
                {
                    if (logo != null)
                        row.ConstantItem(58).Height(58).Image(logo, ImageScaling.FitArea);
                    else
                        row.ConstantItem(58).Height(58);
                    row.ConstantItem(10);
                    row.RelativeItem().AlignMiddle().AlignCenter().Column(c =>
                    {
                        c.Item().Text("PRESIDENCIA MUNICIPAL DE TULA DE ALLENDE, HGO.")
                            .Bold().FontSize(13).FontColor(CP);
                        c.Item().PaddingTop(2)
                            .Text("SOLICITUD Y VALIDACIÓN DE MATERIALES Y/O SERVICIOS")
                            .Bold().FontSize(10).FontColor(CS);
                    });
                    row.ConstantItem(10);
                    row.ConstantItem(90).AlignMiddle().Column(c =>
                    {
                        c.Item().Text("Nº").FontSize(8).FontColor(CT).Bold();
                        c.Item().Text(dto.Folio ?? "________").FontSize(14).Bold().FontColor(CP);
                    });
                });
                col.Item().Background(CS).Height(2);
            });
        }

        private void FormContent(IContainer container, AcquisitionRequestFormPdfDto dto)
        {
            container.PaddingTop(6).Column(col =>
            {
                col.Spacing(5);
                col.Item().Element(c => FormSectionLabel(c, "SOLICITUD", "DATOS DE LA UNIDAD ADMINISTRATIVA SOLICITANTE"));
                col.Item().Element(c => FormHeaderGrid(c, dto));
                col.Item().Element(c => FormJustification(c, dto.Justification));
                col.Item().Element(c => FormSectionCentered(c, "DESCRIPCIÓN DETALLADA DEL MATERIAL O CONTRATACIÓN DEL SERVICIO"));
                col.Item().Element(c => FormItemsTable(c, dto.Items));
                col.Item().PaddingTop(4).Element(c => FormRequestSignatures(c, dto));
                col.Item().PaddingTop(6).Element(c => FormBigLabel(c, "VALIDACIÓN"));
                col.Item().Element(c => FormValidationGrid(c, dto));
                col.Item().PaddingTop(4).Element(c => FormBottomSections(c, dto));
            });
        }

        private void FormSectionLabel(IContainer c, string left, string right)
        {
            c.Row(row =>
            {
                row.AutoItem().Background(CP).PaddingHorizontal(8).PaddingVertical(4)
                    .Text(left).Bold().FontSize(10).FontColor(Colors.White);
                row.ConstantItem(10);
                row.RelativeItem().AlignMiddle().Text(right).Bold().FontSize(9).FontColor(CP);
            });
        }

        private void FormBigLabel(IContainer c, string text)
        {
            c.Row(row =>
            {
                row.ConstantItem(4).Background(CP);
                row.ConstantItem(6);
                row.RelativeItem().AlignMiddle().Text(text).Bold().FontSize(14).FontColor(CP);
            });
        }

        private void FormSectionCentered(IContainer c, string text)
        {
            c.Background(CG).Border(1).BorderColor("#E5E7EB")
                .PaddingVertical(5).PaddingHorizontal(10)
                .AlignCenter().Text(text).Bold().FontSize(9).FontColor(CP);
        }

        private void FormHeaderGrid(IContainer container, AcquisitionRequestFormPdfDto dto)
        {
            container.Column(col =>
            {
                col.Item().Table(t =>
                {
                    t.ColumnsDefinition(cd =>
                    {
                        cd.RelativeColumn(1.4f);
                        cd.RelativeColumn(2.5f);
                        cd.RelativeColumn(1.5f);
                        cd.RelativeColumn(1f);
                        cd.RelativeColumn(1f);
                        cd.RelativeColumn(1.5f);
                        cd.RelativeColumn(1.5f);
                    });
                    t.Header(h =>
                    {
                        foreach (var label in new[]
                        {
                            "CLAVE DE LA\nUNIDAD ADMVA.",
                            "NOMBRE DE LA UNIDAD SOLICITANTE",
                            "FUENTE DE\nFINANCIAMIENTO",
                            "PROG", "COG", "PROYECTO", "FECHA DE SOLICITUD"
                        })
                            h.Cell().Background(CG).Border(1).BorderColor("#D1D5DB")
                                .Padding(5).AlignCenter().Text(label).Bold().FontSize(7).FontColor(CP);
                    });
                    FormCell(t, dto.UnitKey);
                    FormCell(t, dto.UnitName);
                    FormCell(t, dto.FundingSource);
                    FormCell(t, dto.Program);
                    FormCell(t, dto.CogKey);
                    FormCell(t, dto.Project);
                    FormCell(t, dto.RequestDate?.ToString("dd/MM/yyyy"));
                });
            });
        }

        private void FormCell(TableDescriptor t, string? value, int height = 28)
        {
            t.Cell().MinHeight(height).Border(1).BorderColor("#D1D5DB")
                .Padding(5).AlignMiddle().Text(value ?? "").FontSize(9);
        }

        private void FormJustification(IContainer container, string? justification)
        {
            container.Column(col =>
            {
                col.Item().Background(CG).Border(1).BorderColor("#E5E7EB")
                    .PaddingVertical(4).PaddingHorizontal(10).AlignCenter()
                    .Text("JUSTIFICACIÓN DE LA ADQUISICIÓN Y/O SERVICIO")
                    .Bold().FontSize(9).FontColor(CP);
                col.Item().Border(1).BorderColor("#D1D5DB").Row(row =>
                {
                    row.RelativeItem().MinHeight(70).Padding(8)
                        .Text(justification ?? "").FontSize(9);
                    row.ConstantItem(100).Border(1).BorderColor("#D1D5DB")
                        .Padding(6).AlignMiddle().AlignCenter().Column(c =>
                        {
                            c.Item().Height(55).Border(1).BorderColor("#D1D5DB");
                            c.Item().PaddingTop(4).AlignCenter()
                                .Text("SELLO DE PRESUPUESTO").FontSize(7).FontColor(CT);
                        });
                });
            });
        }

        private void FormItemsTable(IContainer container, List<AcquisitionFormItemDto> items)
        {
            container.Table(t =>
            {
                t.ColumnsDefinition(cd =>
                {
                    cd.ConstantColumn(55);
                    cd.ConstantColumn(70);
                    cd.RelativeColumn();
                    cd.ConstantColumn(80);
                });
                t.Header(h =>
                {
                    foreach (var label in new[] { "CANTIDAD", "UNIDAD DE MEDIDA", "DESCRIPCIÓN", "IMPORTE" })
                        h.Cell().Background(CG).Border(1).BorderColor("#D1D5DB")
                            .Padding(5).AlignCenter().Text(label).Bold().FontSize(8).FontColor(CP);
                });
                int minRows = Math.Max(6, items.Count);
                for (int i = 0; i < minRows; i++)
                {
                    var item = i < items.Count ? items[i] : null;
                    var bg = i % 2 == 0 ? "#FFFFFF" : CG;
                    t.Cell().MinHeight(22).Background(bg).Border(1).BorderColor("#D1D5DB")
                        .Padding(5).AlignMiddle().AlignCenter()
                        .Text(item?.Quantity > 0 ? item.Quantity.ToString("G29") : "").FontSize(9);
                    t.Cell().MinHeight(22).Background(bg).Border(1).BorderColor("#D1D5DB")
                        .Padding(5).AlignMiddle().AlignCenter()
                        .Text(item?.UnitMeasure ?? "").FontSize(9);
                    t.Cell().MinHeight(22).Background(bg).Border(1).BorderColor("#D1D5DB")
                        .Padding(5).AlignMiddle().Text(item?.Description ?? "").FontSize(9);
                    var total = item != null
                        ? (item.Total > 0 ? item.Total : item.Quantity * item.UnitPrice) : 0m;
                    t.Cell().MinHeight(22).Background(bg).Border(1).BorderColor("#D1D5DB")
                        .Padding(5).AlignMiddle().AlignRight()
                        .Text(total > 0 ? total.ToString("C", MX) : "").FontSize(9);
                }
                t.Cell().ColumnSpan(3).Background(CA).Padding(5).AlignRight()
                    .Text("IMPORTE TOTAL:").Bold().FontSize(9).FontColor(CP);
                decimal grand = items.Sum(x => x.Total > 0 ? x.Total : x.Quantity * x.UnitPrice);
                t.Cell().Background(CA).Padding(5).AlignRight()
                    .Text(grand > 0 ? grand.ToString("C", MX) : "").Bold().FontSize(9).FontColor(CP);
            });
        }

        private void FormRequestSignatures(IContainer container, AcquisitionRequestFormPdfDto dto)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(c =>
                {
                    c.Item().Text("RECIBIDO Y COTIZADO").FontSize(7).FontColor(CT);
                    c.Item().PaddingTop(20).BorderBottom(1).BorderColor("#D1D5DB")
                        .Text(dto.ReceivedAndQuotedBy ?? "").FontSize(9);
                    c.Item().PaddingTop(3).Text("RECIBIDO Y COTIZADO").FontSize(7).FontColor(CT);
                });
                row.RelativeItem();
                row.ConstantItem(200).Column(c =>
                {
                    c.Item().Text("SOLICITA").FontSize(7).FontColor(CT);
                    c.Item().PaddingTop(20).BorderBottom(1).BorderColor("#D1D5DB")
                        .Text(dto.ApplicantName ?? "").FontSize(9);
                    c.Item().PaddingTop(3)
                        .Text("NOMBRE Y FIRMA DEL TITULAR DE LA UNIDAD SOLICITANTE")
                        .FontSize(7).FontColor(CT);
                });
            });
        }

        private void FormValidationGrid(IContainer container, AcquisitionRequestFormPdfDto dto)
        {
            container.Table(t =>
            {
                t.ColumnsDefinition(cd =>
                {
                    cd.RelativeColumn();
                    cd.RelativeColumn();
                    cd.RelativeColumn();
                });
                t.Header(h =>
                {
                    h.Cell().Background(CG).Border(1).BorderColor("#D1D5DB")
                        .Padding(5).AlignCenter().Text("AUTORIZACIÓN").Bold().FontSize(8).FontColor(CP);
                    h.Cell().Background(CG).Border(1).BorderColor("#D1D5DB")
                        .Padding(5).AlignCenter().Text("RECEPCIÓN DE MATERIAL Y/O SERVICIO").Bold().FontSize(8).FontColor(CP);
                    h.Cell().Background(CG).Border(1).BorderColor("#D1D5DB")
                        .Padding(5).AlignCenter().Text("RESPONSABLE DE LA COTIZACIÓN").Bold().FontSize(8).FontColor(CP);
                });
                foreach (var _ in Enumerable.Range(0, 1))
                {
                    t.Cell().MinHeight(55).Border(1).BorderColor("#D1D5DB").Padding(6).AlignBottom().Column(c =>
                    {
                        c.Item().BorderBottom(1).BorderColor("#D1D5DB").Text(dto.AuthorizedBy ?? "").FontSize(9);
                        c.Item().PaddingTop(3).Text("TESORERO MUNICIPAL").FontSize(7).Bold().FontColor(CT);
                    });
                    t.Cell().MinHeight(55).Border(1).BorderColor("#D1D5DB").Padding(6).AlignBottom().Column(c =>
                    {
                        c.Item().BorderBottom(1).BorderColor("#D1D5DB").Text(dto.ReceivedSignedBy ?? "").FontSize(9);
                        c.Item().PaddingTop(3).Text("SELLO Y FIRMA DE RECIBIDO").FontSize(7).Bold().FontColor(CT);
                    });
                    t.Cell().MinHeight(55).Border(1).BorderColor("#D1D5DB").Padding(6).AlignBottom().Column(c =>
                    {
                        c.Item().BorderBottom(1).BorderColor("#D1D5DB").Text(dto.QuotationResponsible ?? "").FontSize(9);
                        c.Item().PaddingTop(3).Text("DIRECTOR DE ADQUISICIONES").FontSize(7).Bold().FontColor(CT);
                    });
                }
            });
        }

        private void FormBottomSections(IContainer container, AcquisitionRequestFormPdfDto dto)
        {
            container.Table(t =>
            {
                t.ColumnsDefinition(cd =>
                {
                    cd.RelativeColumn();
                    cd.RelativeColumn();
                    cd.RelativeColumn();
                });
                // COMPROMETIDO
                t.Cell().Border(1).BorderColor("#D1D5DB").Column(c =>
                {
                    c.Item().Background(CG).Padding(5).AlignCenter()
                        .Text("COMPROMETIDO").Bold().FontSize(8).FontColor(CP);
                    c.Item().Background(CA).Padding(4).AlignCenter()
                        .Text("ORDEN DE COMPROMETIDO Y/O SERVICIO").FontSize(7).Italic().FontColor(CT);
                    c.Item().Element(inner => FormMiniItemsTable(inner, dto.CommittedItems));
                    c.Item().Border(1).BorderColor("#D1D5DB").Padding(5).Row(r =>
                    {
                        r.RelativeItem().Column(ic =>
                        {
                            ic.Item().Text("FECHA DE PEDIDO:").FontSize(7).Bold().FontColor(CP);
                            ic.Item().PaddingTop(2).Text(dto.OrderDate?.ToString("dd/MM/yyyy") ?? "").FontSize(9);
                        });
                    });
                });
                // DEVENGADO
                t.Cell().Border(1).BorderColor("#D1D5DB").Column(c =>
                {
                    c.Item().Background(CG).Padding(5).AlignCenter()
                        .Text("DEVENGADO").Bold().FontSize(8).FontColor(CP);
                    c.Item().Background(CA).Padding(4).AlignCenter()
                        .Text("RECEPCIÓN DE MATERIALES Y/O SERVICIO").FontSize(7).Italic().FontColor(CT);
                    c.Item().Element(inner => FormMiniItemsTable(inner, dto.AccruedItems));
                    c.Item().Border(1).BorderColor("#D1D5DB").Padding(5).Row(r =>
                    {
                        r.RelativeItem().Column(ic =>
                        {
                            ic.Item().Text("FECHA DE RECIBIDO:").FontSize(7).Bold().FontColor(CP);
                            ic.Item().PaddingTop(2).Text(dto.ReceivedDate?.ToString("dd/MM/yyyy") ?? "").FontSize(9);
                        });
                    });
                });
                // EJERCIDO / PAGADO
                t.Cell().Border(1).BorderColor("#D1D5DB").Column(c =>
                {
                    c.Item().Background(CG).Padding(5).AlignCenter()
                        .Text("EJERCIDO/PAGADO").Bold().FontSize(8).FontColor(CP);
                    c.Item().Background(CA).Padding(4).AlignCenter()
                        .Text("ORDEN DE PAGO").FontSize(7).Italic().FontColor(CT);
                    c.Item().Border(1).BorderColor("#D1D5DB").Padding(5).Column(inner =>
                    {
                        inner.Spacing(4);
                        inner.Item().Row(r =>
                        {
                            r.AutoItem().Text("PROVEEDOR:").FontSize(7).Bold().FontColor(CT);
                            r.ConstantItem(4);
                            r.RelativeItem().BorderBottom(1).BorderColor("#D1D5DB")
                                .Text(dto.Supplier ?? "").FontSize(8);
                        });
                        inner.Item().Row(r =>
                        {
                            r.AutoItem().Text("R.F.C.:").FontSize(7).Bold().FontColor(CT);
                            r.ConstantItem(4);
                            r.RelativeItem().BorderBottom(1).BorderColor("#D1D5DB")
                                .Text(dto.SupplierRFC ?? "").FontSize(8);
                        });
                        inner.Item().Row(r =>
                        {
                            r.AutoItem().Text("AUTORIZADO PARA PAGO").FontSize(7).Bold().FontColor(CT);
                            r.ConstantItem(6);
                            r.AutoItem().Width(14).Height(14).Border(1).BorderColor("#D1D5DB")
                                .Background(dto.AuthorizedForPayment == true ? CVF : "#FFFFFF")
                                .AlignMiddle().AlignCenter()
                                .Text(dto.AuthorizedForPayment == true ? "✔" : "")
                                .FontSize(8).FontColor(CV);
                        });
                        inner.Item().Text("FORMA DE PAGO:").FontSize(7).Bold().FontColor(CT);
                        foreach (var (label, val) in new[]
                        {
                            ("CONTADO",          dto.CashAmount),
                            ("CRÉDITO",          dto.CreditAmount),
                            ("REEMBOLSO",        dto.ReimbursementAmount),
                            ("PAGO A PROVEEDOR", dto.SupplierPaymentAmount),
                        })
                        {
                            inner.Item().Row(r =>
                            {
                                r.ConstantItem(10);
                                r.AutoItem().Text(label + ":").FontSize(7).FontColor(CT);
                                r.ConstantItem(4);
                                r.RelativeItem().Height(14).Border(1).BorderColor("#D1D5DB")
                                    .Padding(2).AlignRight()
                                    .Text(val.HasValue && val > 0 ? val.Value.ToString("C", MX) : "").FontSize(8);
                            });
                        }
                    });
                });
            });
        }

        private void FormMiniItemsTable(IContainer container, List<AcquisitionFormItemDto> items)
        {
            container.Table(t =>
            {
                t.ColumnsDefinition(cd =>
                {
                    cd.ConstantColumn(35);
                    cd.ConstantColumn(40);
                    cd.RelativeColumn();
                });
                t.Header(h =>
                {
                    h.Cell().Background(CG).Border(1).BorderColor("#E5E7EB")
                        .Padding(3).AlignCenter().Text("CANT.").Bold().FontSize(7).FontColor(CP);
                    h.Cell().Background(CG).Border(1).BorderColor("#E5E7EB")
                        .Padding(3).AlignCenter().Text("U. MED.").Bold().FontSize(7).FontColor(CP);
                    h.Cell().Background(CG).Border(1).BorderColor("#E5E7EB")
                        .Padding(3).Text("DESCRIPCIÓN").Bold().FontSize(7).FontColor(CP);
                });
                int rows = Math.Max(3, items.Count);
                for (int i = 0; i < rows; i++)
                {
                    var item = i < items.Count ? items[i] : null;
                    t.Cell().MinHeight(18).Border(1).BorderColor("#E5E7EB").Padding(3).AlignCenter()
                        .Text(item?.Quantity > 0 ? item.Quantity.ToString("G29") : "").FontSize(8);
                    t.Cell().MinHeight(18).Border(1).BorderColor("#E5E7EB").Padding(3).AlignCenter()
                        .Text(item?.UnitMeasure ?? "").FontSize(8);
                    t.Cell().MinHeight(18).Border(1).BorderColor("#E5E7EB").Padding(3)
                        .Text(item?.Description ?? "").FontSize(8);
                }
            });
        }

        // ════════════════════════════════════════════════════════
        //  ██  DOC 2 — RESUMEN COMPLETO — IMPLEMENTACIÓN
        // ════════════════════════════════════════════════════════
        private void SummaryHeader(IContainer container, AcquisitionRequestPdfDto dto)
        {
            var logo = GetLogo();
            container.Column(col =>
            {
                col.Item().Background(CP).Height(4);
                col.Item().PaddingTop(8).PaddingBottom(8).Row(row =>
                {
                    if (logo != null)
                        row.ConstantItem(60).Height(60).Image(logo, ImageScaling.FitArea);
                    else
                        row.ConstantItem(60).Height(60);
                    row.ConstantItem(10);
                    row.RelativeItem().AlignMiddle().Column(tc =>
                    {
                        tc.Item().Text("PRESIDENCIA MUNICIPAL DE TULA DE ALLENDE")
                            .Bold().FontSize(12).FontColor(CP);
                        tc.Item().PaddingTop(2).Text("Dirección de Adquisiciones").FontSize(9).FontColor(CT);
                        tc.Item().PaddingTop(2).Text("Solicitud de Materiales y Servicios")
                            .FontSize(9).Bold().FontColor(CS);
                    });
                    row.ConstantItem(10);
                    row.ConstantItem(150).Background(CA).Border(1).BorderColor(CS).Padding(7).Column(ic =>
                    {
                        ic.Item().Text("FOLIO").FontSize(7).FontColor(CT).Bold();
                        ic.Item().Text(dto.Folio ?? "—").FontSize(10).Bold().FontColor(CP);
                        ic.Item().PaddingTop(4).Text("FECHA DE SOLICITUD").FontSize(7).FontColor(CT).Bold();
                        ic.Item().Text((dto.RequestDate ?? DateTime.Now).ToString("dd/MM/yyyy"))
                            .FontSize(10).FontColor(CP);
                        if (dto.AuthorizationDate.HasValue)
                        {
                            ic.Item().PaddingTop(4).Text("FECHA DE AUTORIZACIÓN").FontSize(7).FontColor(CT).Bold();
                            ic.Item().Text(dto.AuthorizationDate.Value.ToString("dd/MM/yyyy"))
                                .FontSize(10).FontColor(CP);
                        }
                        if (dto.MaxCompletionDate.HasValue)
                        {
                            ic.Item().PaddingTop(4).Text("FECHA LÍMITE").FontSize(7).FontColor(CT).Bold();
                            ic.Item().Text(dto.MaxCompletionDate.Value.ToString("dd/MM/yyyy"))
                                .FontSize(10).FontColor(CN);
                        }
                    });
                });
                col.Item().Background(CS).Height(2);
            });
        }

        private void SummaryContent(IContainer container, AcquisitionRequestPdfDto dto)
        {
            container.PaddingTop(10).Column(col =>
            {
                col.Spacing(10);

                if (!string.IsNullOrWhiteSpace(dto.Status))
                    col.Item().Element(c => SummaryStatusBadge(c, dto.Status!));

                col.Item().Element(c => SharedSectionTitle(c, "Datos Generales"));
                col.Item().Background(CG).Border(1).BorderColor("#E5E7EB").Padding(10).Column(d =>
                {
                    d.Spacing(8);
                    d.Item().Row(r =>
                    {
                        r.RelativeItem().Element(c => SummaryField(c, "Unidad Administrativa", dto.AdministrativeUnit));
                        r.ConstantItem(10);
                        r.RelativeItem().Element(c => SummaryField(c, "Programa", dto.Program));
                    });
                    d.Item().Row(r =>
                    {
                        r.RelativeItem().Element(c => SummaryField(c, "Proyecto", dto.Project));
                        r.ConstantItem(10);
                        r.RelativeItem().Element(c => SummaryField(c, "Fuente de Financiamiento", dto.FundingSource));
                    });
                    d.Item().Row(r =>
                    {
                        r.RelativeItem().Element(c => SummaryField(c, "Tipo de Adquisición", dto.AcquisitionType));
                        r.ConstantItem(10);
                        r.RelativeItem().Element(c => SummaryField(c, "Clasificación", dto.Classification));
                    });
                });

                if (!string.IsNullOrWhiteSpace(dto.Community) || !string.IsNullOrWhiteSpace(dto.Beneficiary))
                {
                    col.Item().Element(c => SharedSectionTitle(c, "Localización y Beneficiario"));
                    col.Item().Background(CG).Border(1).BorderColor("#E5E7EB").Padding(10).Row(r =>
                    {
                        r.RelativeItem().Element(c => SummaryField(c, "Comunidad / Localidad", dto.Community));
                        r.ConstantItem(10);
                        r.RelativeItem().Element(c => SummaryField(c, "Beneficiario", dto.Beneficiary));
                    });
                }

                col.Item().Element(c => SharedSectionTitle(c, "Justificación"));
                col.Item().ShowEntire().Border(1).BorderColor("#E5E7EB").Padding(10)
                    .Text(string.IsNullOrWhiteSpace(dto.Justification) ? "N/A" : dto.Justification).FontSize(10);

                if (!string.IsNullOrWhiteSpace(dto.Observations))
                {
                    col.Item().Element(c => SharedSectionTitle(c, "Observaciones"));
                    col.Item().ShowEntire().Border(1).BorderColor("#E5E7EB").Padding(10)
                        .Text(dto.Observations).FontSize(10).FontColor(CT);
                }

                col.Item().Element(c => SharedSectionTitle(c, "Detalle de Materiales"));
                col.Item().Element(c => SummaryTable(c, dto.Items, dto.TotalAmount));

                if (!string.IsNullOrWhiteSpace(dto.Supplier) || !string.IsNullOrWhiteSpace(dto.PaymentPolicy))
                {
                    col.Item().Element(c => SharedSectionTitle(c, "Proveedor y Pago"));
                    col.Item().Background(CDL).Border(1).BorderColor(CDB).Padding(10).Column(d =>
                    {
                        d.Spacing(8);
                        d.Item().Row(r =>
                        {
                            r.RelativeItem().Element(c => SummaryField(c, "Proveedor", dto.Supplier));
                            r.ConstantItem(10);
                            r.RelativeItem().Element(c => SummaryField(c, "RFC", dto.SupplierRFC));
                        });
                        d.Item().Row(r =>
                        {
                            r.RelativeItem().Element(c => SummaryField(c, "Póliza de Pago", dto.PaymentPolicy));
                            r.ConstantItem(10);
                            r.RelativeItem().Element(c => SummaryField(c, "CFDI", dto.CFDI));
                        });
                    });
                }

                col.Item().Background(CVF).Border(1).BorderColor("#86EFAC").Padding(8).Row(r =>
                {
                    r.ConstantItem(20).AlignMiddle().Text("✔").FontSize(12).FontColor(CV).Bold();
                    r.RelativeItem().Column(c =>
                    {
                        c.Item().Text("Documento Validado").Bold().FontSize(10).FontColor(CV);
                        var audit = $"Generado el {dto.CreatedAt:dd/MM/yyyy} a las {dto.CreatedAt:HH:mm}";
                        if (!string.IsNullOrWhiteSpace(dto.CreatedByName))
                            audit += $"  •  Creado por: {dto.CreatedByName}";
                        c.Item().Text(audit).FontSize(8).FontColor(CV);
                    });
                });

                col.Item().Element(c => SummaryBottomSections(c, dto));
            });
        }

        private void SummaryStatusBadge(IContainer c, string status)
        {
            var (bg, fg) = status.ToUpperInvariant() switch
            {
                "AUTORIZADO" => (CVF, CV),
                "PENDIENTE" => (CNF, CN),
                "RECHAZADO" => ("#FEE2E2", "#991B1B"),
                _ => (CG, CT)
            };
            c.Row(row =>
            {
                row.RelativeItem();
                row.AutoItem().Background(bg).Border(1).BorderColor(fg)
                    .PaddingHorizontal(10).PaddingVertical(4)
                    .Text($"Estado: {status}").Bold().FontSize(9).FontColor(fg);
            });
        }

        private void SummaryTable(IContainer container, List<AcquisitionRequestPdfItemDto> items, decimal totalAmount)
        {
            if (!items.Any())
            {
                container.Border(1).BorderColor("#E5E7EB").Padding(10)
                    .Text("Sin partidas registradas.").FontSize(9).FontColor(CT);
                return;
            }
            bool hasCog = items.Any(i => !string.IsNullOrWhiteSpace(i.CogKey));
            container.Table(t =>
            {
                t.ColumnsDefinition(cd =>
                {
                    if (hasCog) cd.ConstantColumn(50);
                    cd.ConstantColumn(40);
                    cd.ConstantColumn(60);
                    cd.RelativeColumn();
                    cd.ConstantColumn(72);
                    cd.ConstantColumn(80);
                });
                t.Header(h =>
                {
                    if (hasCog)
                        h.Cell().Background(CP).Padding(6).Text("COG").Bold().FontSize(8).FontColor(Colors.White);
                    foreach (var cell in new[] { "Cant.", "Unidad", "Descripción", "Precio Unit.", "Total" })
                        h.Cell().Background(CP).Padding(6).Text(cell).Bold().FontSize(8).FontColor(Colors.White);
                });
                bool odd = false; decimal calc = 0;
                foreach (var item in items)
                {
                    decimal rt = item.Total > 0 ? item.Total : item.Quantity * item.UnitPrice;
                    calc += rt;
                    var bg = odd ? "#FFFFFF" : CG; odd = !odd;
                    if (hasCog) t.Cell().Background(bg).Padding(5).Text(item.CogKey ?? "").FontSize(8).FontColor(CT);
                    t.Cell().Background(bg).Padding(5).Text(item.Quantity.ToString("G29")).FontSize(9);
                    t.Cell().Background(bg).Padding(5).Text(item.UnitMeasure).FontSize(9);
                    t.Cell().Background(bg).Padding(5).Column(c =>
                    {
                        c.Item().Text(item.Description).FontSize(9);
                        if (!string.IsNullOrWhiteSpace(item.CogName))
                            c.Item().Text(item.CogName).FontSize(7).FontColor(CT);
                    });
                    t.Cell().Background(bg).Padding(5).AlignRight().Text(item.UnitPrice.ToString("C", MX)).FontSize(9);
                    t.Cell().Background(bg).Padding(5).AlignRight().Text(rt.ToString("C", MX)).FontSize(9);
                }
                uint span = hasCog ? 5u : 4u;
                decimal display = totalAmount > 0 ? totalAmount : calc;
                t.Cell().ColumnSpan(span).Background(CA).Padding(6).AlignRight()
                    .Text("TOTAL GENERAL").Bold().FontSize(9).FontColor(CP);
                t.Cell().Background(CA).Padding(6).AlignRight()
                    .Text(display.ToString("C", MX)).Bold().FontSize(10).FontColor(CP);
            });
        }

        private void SummaryBottomSections(IContainer container, AcquisitionRequestPdfDto dto)
        {
            container.Row(row =>
            {
                row.RelativeItem().MinHeight(70).Border(1).BorderColor("#E5E7EB").Padding(8).Column(c =>
                {
                    c.Item().Text("COMPROMETIDO").Bold().FontSize(9).FontColor(CP);
                    c.Item().PaddingTop(6).Text("Fecha de pedido: _______________").FontSize(9).FontColor(CT);
                });
                row.ConstantItem(6);
                row.RelativeItem().MinHeight(70).Border(1).BorderColor("#E5E7EB").Padding(8).Column(c =>
                {
                    c.Item().Text("DEVENGADO").Bold().FontSize(9).FontColor(CP);
                    c.Item().PaddingTop(6).Text("Fecha de recibido: _______________").FontSize(9).FontColor(CT);
                });
                row.ConstantItem(6);
                row.RelativeItem().MinHeight(70).Border(1).BorderColor("#E5E7EB").Padding(8).Column(c =>
                {
                    c.Item().Text("EJERCIDO / PAGADO").Bold().FontSize(9).FontColor(CP);
                    c.Item().PaddingTop(4).Text($"Proveedor: {dto.Supplier ?? "_______________"}").FontSize(9).FontColor(CT);
                    c.Item().PaddingTop(4).Text($"RFC: {dto.SupplierRFC ?? "_______________"}").FontSize(9).FontColor(CT);
                    c.Item().PaddingTop(4).Text($"Póliza de pago: {dto.PaymentPolicy ?? "_______________"}").FontSize(9).FontColor(CT);
                    if (!string.IsNullOrWhiteSpace(dto.CFDI))
                        c.Item().PaddingTop(4).Text($"CFDI: {dto.CFDI}").FontSize(9).FontColor(CT);
                });
            });
        }

        // ════════════════════════════════════════════════════════
        //  SHARED — Título de sección + Footer
        // ════════════════════════════════════════════════════════
        private void SharedSectionTitle(IContainer c, string title)
        {
            c.Height(18).Row(row =>
            {
                row.ConstantItem(4).Background(CS);
                row.ConstantItem(8);
                row.RelativeItem().AlignMiddle().Text(title).Bold().FontSize(10).FontColor(CP);
            });
        }

        private void SummaryField(IContainer c, string label, string? value)
        {
            c.Column(col =>
            {
                col.Item().Text(label).FontSize(8).FontColor(CT).Bold();
                col.Item().Text(string.IsNullOrWhiteSpace(value) ? "—" : value).FontSize(10);
            });
        }

        private void SharedFooter(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Background(CS).Height(1);
                col.Item().PaddingTop(5).Row(row =>
                {
                    row.RelativeItem().Text("Presidencia Municipal de Tula de Allende")
                        .FontSize(8).FontColor(CT);
                    row.ConstantItem(110).AlignRight().Text(x =>
                    {
                        x.Span("Página ").FontSize(8).FontColor(CT);
                        x.CurrentPageNumber().FontSize(8).FontColor(CT);
                        x.Span(" de ").FontSize(8).FontColor(CT);
                        x.TotalPages().FontSize(8).FontColor(CT);
                    });
                });
            });
        }

        // ════════════════════════════════════════════════════════
        //  HELPERS — Normalización de estados
        // ════════════════════════════════════════════════════════
        private static string NormalizeCode(string? s)
        {
            if (string.IsNullOrWhiteSpace(s)) return string.Empty;
            return s.ToUpperInvariant()
                .Replace(" ", "_")
                .Replace("É", "E").Replace("Ó", "O").Replace("Á", "A")
                .Replace("Í", "I").Replace("Ú", "U");
        }

        private static (string bg, string fg) ResolveStatusColors(string? status)
        {
            return NormalizeCode(status) switch
            {
                "APROBADO" => (CVF, CV),
                "EN_REVISION" => (CBluF, CBlue),
                "RECHAZADO" => (CRedF, CRed),
                "PENDIENTE" => (CNF, CN),
                _ => (CG, CT)
            };
        }
    }
}