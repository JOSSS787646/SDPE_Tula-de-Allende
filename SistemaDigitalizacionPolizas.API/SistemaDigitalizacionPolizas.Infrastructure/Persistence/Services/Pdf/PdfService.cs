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
        private const string CP = "#6A1B1B";   // Guinda primario
        private const string CS = "#8B2C2C";   // Guinda secundario
        private const string CA = "#FDF2F2";   // Fondo rosado suave
        private const string CG = "#F5F5F5";   // Gris claro
        private const string CT = "#6B7280";   // Texto suave
        private const string CDB = "#C9A227";   // Dorado borde
        private const string CDL = "#F4E4BC";   // Dorado claro fondo
        private const string CV = "#166534";   // Verde
        private const string CVF = "#DCFCE7";   // Verde fondo
        private const string CN = "#92400E";   // Naranja
        private const string CNF = "#FEF3C7";   // Naranja fondo

        // Estados checklist
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
        //  DOC 3 — Check List
        // ════════════════════════════════════════════════════════
        public byte[] GenerateChecklistPdf(AcquisitionChecklistPdfDto dto)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            return Document.Create(c => c.Page(p =>
            {
                p.Size(PageSizes.Letter);
                p.MarginHorizontal(30);
                p.MarginTop(24);
                p.MarginBottom(44);
                p.Header().Element(h => CLHeader(h, dto));
                p.Content().Element(h => CLContent(h, dto));
                p.Footer().Element(SharedFooter);
            })).GeneratePdf();
        }

        // ════════════════════════════════════════════════════════
        //  DOC 3 — HEADER
        // ════════════════════════════════════════════════════════
        private void CLHeader(IContainer container, AcquisitionChecklistPdfDto dto)
        {
            var logo = GetLogo();

            container.Column(col =>
            {
                // Franja superior guinda
                col.Item().Background(CP).Height(5);

                col.Item().PaddingTop(8).PaddingBottom(8).Row(row =>
                {
                    if (logo != null)
                        row.ConstantItem(52).Height(52).Image(logo, ImageScaling.FitArea);
                    else
                        row.ConstantItem(52).Height(52);

                    row.ConstantItem(12);

                    // Títulos
                    row.RelativeItem().AlignMiddle().Column(tc =>
                    {
                        tc.Item().Text("SECRETARÍA DE LA TESORERÍA Y ADMINISTRACIÓN")
                            .Bold().FontSize(7).FontColor(CT).LetterSpacing(0.08f);
                        tc.Item().PaddingTop(2)
                            .Text("CHECK LIST PARA PROCESO DE ADQUISICIONES")
                            .Bold().FontSize(13).FontColor(CP);
                        if (!string.IsNullOrWhiteSpace(dto.ClassificationName))
                            tc.Item().PaddingTop(1)
                                .Text(dto.ClassificationName.ToUpper())
                                .FontSize(9).FontColor(CS);
                    });

                    row.ConstantItem(12);

                    // Caja folio / fecha — compacta
                    row.ConstantItem(132).Background(CA).Border(1).BorderColor(CS)
                        .Padding(7).Column(ic =>
                        {
                            ic.Item().Text("FOLIO").FontSize(7).FontColor(CT).Bold();
                            ic.Item().Text(dto.Folio ?? "—").FontSize(13).Bold().FontColor(CP);

                            ic.Item().PaddingTop(4).Text("FECHA DE SOLICITUD").FontSize(7).FontColor(CT).Bold();
                            ic.Item().Text((dto.RequestDate ?? DateTime.Now).ToString("dd/MM/yyyy"))
                                .FontSize(9).FontColor(CP);

                            // Estado GLOBAL del checklist
                            if (!string.IsNullOrWhiteSpace(dto.ExceptionStatus))
                            {
                                var (stBg, stFg) = ResolveStatusColors(dto.ExceptionStatus);
                                ic.Item().PaddingTop(5).Background(stBg)
                                    .PaddingHorizontal(5).PaddingVertical(3)
                                    .Text(dto.ExceptionStatus.ToUpper())
                                    .FontSize(8).Bold().FontColor(stFg);
                            }
                        });
                });

                // Banda de datos de solicitud — solo si tienen valor
                bool hasBand = !string.IsNullOrWhiteSpace(dto.AdministrativeUnit)
                    || !string.IsNullOrWhiteSpace(dto.ApplicantName)
                    || !string.IsNullOrWhiteSpace(dto.Program)
                    || !string.IsNullOrWhiteSpace(dto.Project);

                if (hasBand)
                {
                    col.Item().Background(CG).BorderTop(1).BorderBottom(1).BorderColor("#E5E7EB")
                        .PaddingVertical(5).PaddingHorizontal(10).Row(r =>
                        {
                            CLInfoChip(r, "Unidad", dto.AdministrativeUnit);
                            r.ConstantItem(16);
                            CLInfoChip(r, "Solicitante", dto.ApplicantName);
                            r.ConstantItem(16);
                            CLInfoChip(r, "Programa", dto.Program);
                            r.ConstantItem(16);
                            CLInfoChip(r, "Proyecto", dto.Project);
                        });
                }

                col.Item().Background(CS).Height(2);
            });
        }

        private void CLInfoChip(RowDescriptor row, string label, string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return;
            row.AutoItem().Column(c =>
            {
                c.Item().Text(label.ToUpper()).FontSize(6).FontColor(CT).Bold().LetterSpacing(0.07f);
                c.Item().Text(value).FontSize(8.5f).FontColor(CP);
            });
        }

        // ════════════════════════════════════════════════════════
        //  DOC 3 — CONTENT
        // ════════════════════════════════════════════════════════
        private void CLContent(IContainer container, AcquisitionChecklistPdfDto dto)
        {
            // Separar ítems: obligatorios y con excepción (NoApplies)
            var required = dto.Checklist.Where(i => i.RequiredByRule && !i.NoApplies).OrderBy(i => i.Order).ToList();
            var exceptions = dto.Checklist.Where(i => i.NoApplies).OrderBy(i => i.Order).ToList();

            container.PaddingTop(8).Column(col =>
            {
                col.Spacing(8);

                // ── Resumen de estado global ──────────────────
                col.Item().Element(c => CLGlobalSummary(c, dto));

                // ── Documentos obligatorios ───────────────────
                if (required.Any())
                {
                    col.Item().Element(c => SharedSectionTitle(c, "Documentos Obligatorios"));
                    col.Item().Element(c => CLTable(c, required));
                }

                // ── Documentos con excepción ──────────────────
                if (exceptions.Any())
                {
                    col.Item().Element(c => SharedSectionTitle(c, "Excepciones / No Aplica"));
                    col.Item().Element(c => CLExceptionsTable(c, exceptions));
                }

                // ── Firmas ────────────────────────────────────
                col.Item().PaddingTop(10).Element(c => CLSignatures(c, dto));
            });
        }

        // ── Resumen global compacto ───────────────────────────
        private void CLGlobalSummary(IContainer container, AcquisitionChecklistPdfDto dto)
        {
            var items = dto.Checklist;
            int total = items.Count;
            int approved = items.Count(i => NormalizeCode(i.GlobalStatus) == "APROBADO");
            int pending = items.Count(i => !i.NoApplies
                && NormalizeCode(i.GlobalStatus) is "PENDIENTE" or "");
            int review = items.Count(i => NormalizeCode(i.GlobalStatus) == "EN_REVISION");
            int rejected = items.Count(i => NormalizeCode(i.GlobalStatus) == "RECHAZADO");
            int na = items.Count(i => i.NoApplies);
            int active = total - na;
            int pct = active > 0 ? (int)Math.Round((double)approved / active * 100) : 0;

            container.Border(1).BorderColor("#E5E7EB").Row(row =>
            {
                // Stat boxes
                CLStat(row, approved.ToString(), "APROBADOS", CVF, CV, "#86EFAC");
                CLStat(row, pending.ToString(), "PENDIENTES", CNF, CN, "#FCD34D");
                CLStat(row, review.ToString(), "EN REVISION", CBluF, CBlue, CBluB);
                CLStat(row, rejected.ToString(), "RECHAZADOS", CRedF, CRed, CRedB);
                CLStat(row, na.ToString(), "NO APLICA", CG, CT, "#D1D5DB");

                // Barra divisora
                row.ConstantItem(1).Background("#E5E7EB");

                // Porcentaje
                row.ConstantItem(80).Background(CA).AlignMiddle().AlignCenter().Column(c =>
                {
                    c.Item().AlignCenter()
                        .Text($"{pct}%").FontSize(22).Bold().FontColor(CP);
                    c.Item().AlignCenter()
                        .Text($"{approved}/{active} docs").FontSize(7).FontColor(CT);
                    c.Item().AlignCenter()
                        .Text("AVANCE").FontSize(6).Bold().FontColor(CT).LetterSpacing(0.06f);
                });
            });
        }

        private void CLStat(RowDescriptor row, string value, string label,
                             string bg, string fg, string border)
        {
            row.RelativeItem().Background(bg).BorderRight(1).BorderColor(border)
                .PaddingVertical(8).PaddingHorizontal(6).Column(c =>
                {
                    c.Item().AlignCenter().Text(value).FontSize(18).Bold().FontColor(fg);
                    c.Item().AlignCenter().Text(label)
                .FontSize(6)
                .Bold()
                .FontColor(fg)
                .LetterSpacing(0.05f); // ✅ CORRECTO
                });
        }

        // ── Tabla principal de documentos obligatorios ────────
        private void CLTable(IContainer container, List<ChecklistPdfItemDto> items)
        {
            container.Table(t =>
            {
                t.ColumnsDefinition(cd =>
                {
                    cd.ConstantColumn(24);   // Nº
                    cd.RelativeColumn();     // Nombre del documento
                    cd.ConstantColumn(95);   // Estado de validación
                });

                // Header
                t.Header(h =>
                {
                    h.Cell().Background(CP).PaddingVertical(6).PaddingHorizontal(5)
                        .AlignCenter().Text("N°").Bold().FontSize(8).FontColor(Colors.White);

                    h.Cell().Background(CP).PaddingVertical(6).PaddingHorizontal(8)
                        .Text("DOCUMENTO REQUERIDO").Bold().FontSize(8).FontColor(Colors.White);

                    h.Cell().Background(CP).PaddingVertical(6).PaddingHorizontal(5)
                        .AlignCenter().Text("ESTATUS").Bold().FontSize(8).FontColor(Colors.White);
                });

                bool odd = false;
                foreach (var item in items)
                {
                    var rowBg = odd ? "#FFFFFF" : CG;
                    odd = !odd;

                    // Nº
                    t.Cell().MinHeight(30).Background(rowBg)
                        .BorderBottom(1).BorderColor("#E5E7EB")
                        .AlignCenter().AlignMiddle()
                        .Text(item.Order.ToString())
                        .Bold().FontSize(11).FontColor(CP);

                    // Documento + observaciones
                    t.Cell().MinHeight(30).Background(rowBg)
                        .BorderBottom(1).BorderColor("#E5E7EB")
                        .PaddingHorizontal(8).PaddingVertical(6).Column(c =>
                        {
                            c.Item().Text(item.DocumentName)
                                .FontSize(8.5f).FontColor("#1F2937");

                            if (item.Observations != null)
                            {
                                foreach (var obs in item.Observations.Where(o => !string.IsNullOrWhiteSpace(o)))
                                    c.Item().PaddingTop(2)
                                        .Text(obs)
                                        .FontSize(7).Italic().FontColor(CT);
                            }
                        });

                    // Estado
                    t.Cell().MinHeight(30).Background(rowBg)
                        .BorderBottom(1).BorderColor("#E5E7EB")
                        .AlignCenter().AlignMiddle()
                        .Element(c => CLStatusCell(c, item.GlobalStatus, false));
                }
            });
        }

        // ── Tabla de excepciones ──────────────────────────────
        private void CLExceptionsTable(IContainer container, List<ChecklistPdfItemDto> items)
        {
            container.Table(t =>
            {
                t.ColumnsDefinition(cd =>
                {
                    cd.ConstantColumn(24);
                    cd.RelativeColumn();
                    cd.ConstantColumn(95);
                });

                t.Header(h =>
                {
                    h.Cell().Background(CT).PaddingVertical(6).PaddingHorizontal(5)
                        .AlignCenter().Text("N°").Bold().FontSize(8).FontColor(Colors.White);
                    h.Cell().Background(CT).PaddingVertical(6).PaddingHorizontal(8)
                        .Text("DOCUMENTO").Bold().FontSize(8).FontColor(Colors.White);
                    h.Cell().Background(CT).PaddingVertical(6).PaddingHorizontal(5)
                        .AlignCenter().Text("EXCEPCIÓN").Bold().FontSize(8).FontColor(Colors.White);
                });

                bool odd = false;
                foreach (var item in items)
                {
                    var rowBg = odd ? "#FFFFFF" : CG;
                    odd = !odd;

                    t.Cell().MinHeight(26).Background(rowBg)
                        .BorderBottom(1).BorderColor("#E5E7EB")
                        .AlignCenter().AlignMiddle()
                        .Text(item.Order.ToString()).Bold().FontSize(10).FontColor(CT);

                    t.Cell().MinHeight(26).Background(rowBg)
                        .BorderBottom(1).BorderColor("#E5E7EB")
                        .PaddingHorizontal(8).PaddingVertical(5).Column(c =>
                        {
                            c.Item().Text(item.DocumentName).FontSize(8.5f).FontColor(CT);
                            if (item.Observations != null)
                            {
                                foreach (var obs in item.Observations.Where(o => !string.IsNullOrWhiteSpace(o)))
                                    c.Item().PaddingTop(2).Text(obs).FontSize(7).Italic().FontColor(CT);
                            }
                        });

                    // Excepción — badge NA
                    t.Cell().MinHeight(26).Background(rowBg)
                        .BorderBottom(1).BorderColor("#E5E7EB")
                        .AlignCenter().AlignMiddle()
                        .Element(c => CLStatusCell(c, null, true));
                }
            });
        }

        // ── Celda de estado — sin íconos ─────────────────────
        private void CLStatusCell(IContainer c, string? status, bool noApplies)
        {
            if (noApplies)
            {
                c.Background(CG).Border(1).BorderColor("#D1D5DB")
                    .Width(78).PaddingVertical(5).PaddingHorizontal(4)
                    .AlignCenter().AlignMiddle()
                    .Text("NO APLICA").FontSize(7).Bold().FontColor(CT).LetterSpacing(0.05f);
                return;
            }

            var code = NormalizeCode(status);
            var (bg, fg, border, label) = code switch
            {
                "APROBADO" => (CVF, CV, "#86EFAC", "APROBADO"),
                "EN_REVISION" => (CBluF, CBlue, CBluB, "EN REVISION"),
                "RECHAZADO" => (CRedF, CRed, CRedB, "RECHAZADO"),
                "PENDIENTE" => (CNF, CN, "#FCD34D", "PENDIENTE"),
                _ => (CG, CT, "#D1D5DB", "SIN ESTADO"),
            };

            c.Background(bg).Border(1).BorderColor(border)
                .Width(78).PaddingVertical(5).PaddingHorizontal(4)
                .AlignCenter().AlignMiddle()
                .Text(label).FontSize(7).Bold().FontColor(fg).LetterSpacing(0.04f);
        }

        // ── Firmas ────────────────────────────────────────────
        private void CLSignatures(IContainer container, AcquisitionChecklistPdfDto dto)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(c =>
                {
                    c.Item().Height(28);
                    c.Item().BorderBottom(1).BorderColor("#374151").PaddingBottom(3)
                        .Text(dto.AcquisitionsSignatoryName ?? "").FontSize(9).FontColor(CP);
                    c.Item().PaddingTop(3).Text("Nombre y firma").FontSize(7).Italic().FontColor(CT);
                    c.Item().Text(
                        dto.AcquisitionsSignatoryTitle ?? "Guadalupe Noguez Becerra / Adquisiciones")
                        .FontSize(7.5f).Bold().FontColor(CP);
                });

                row.ConstantItem(40);

                row.RelativeItem().Column(c =>
                {
                    c.Item().Height(28);
                    c.Item().BorderBottom(1).BorderColor("#374151").PaddingBottom(3)
                        .Text(dto.TreasurySignatoryName ?? "").FontSize(9).FontColor(CP);
                    c.Item().PaddingTop(3).Text("Nombre y firma").FontSize(7).Italic().FontColor(CT);
                    c.Item().Text(
                        dto.TreasurySignatoryTitle ?? "Jaqueline Moreno Martinez / Tesoreria")
                        .FontSize(7.5f).Bold().FontColor(CP);
                });
            });
        }

        // ════════════════════════════════════════════════════════
        //  DOC 1 — FORMULARIO FISICO
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
                            .Text("SOLICITUD Y VALIDACION DE MATERIALES Y/O SERVICIOS")
                            .Bold().FontSize(10).FontColor(CS);
                    });
                    row.ConstantItem(10);
                    row.ConstantItem(90).AlignMiddle().Column(c =>
                    {
                        c.Item().Text("No.").FontSize(8).FontColor(CT).Bold();
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
                col.Item().Element(c => FormSectionCentered(c, "DESCRIPCION DETALLADA DEL MATERIAL O CONTRATACION DEL SERVICIO"));
                col.Item().Element(c => FormItemsTable(c, dto.Items));
                col.Item().PaddingTop(4).Element(c => FormRequestSignatures(c, dto));
                col.Item().PaddingTop(6).Element(c => FormBigLabel(c, "VALIDACION"));
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
                    .Text("JUSTIFICACION DE LA ADQUISICION Y/O SERVICIO")
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
                    foreach (var label in new[] { "CANTIDAD", "UNIDAD DE MEDIDA", "DESCRIPCION", "IMPORTE" })
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
                        .Padding(5).AlignMiddle().AlignCenter().Text(item?.UnitMeasure ?? "").FontSize(9);
                    t.Cell().MinHeight(22).Background(bg).Border(1).BorderColor("#D1D5DB")
                        .Padding(5).AlignMiddle().Text(item?.Description ?? "").FontSize(9);
                    var total = item != null ? (item.Total > 0 ? item.Total : item.Quantity * item.UnitPrice) : 0m;
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
                        .Padding(5).AlignCenter().Text("AUTORIZACION").Bold().FontSize(8).FontColor(CP);
                    h.Cell().Background(CG).Border(1).BorderColor("#D1D5DB")
                        .Padding(5).AlignCenter().Text("RECEPCION DE MATERIAL Y/O SERVICIO").Bold().FontSize(8).FontColor(CP);
                    h.Cell().Background(CG).Border(1).BorderColor("#D1D5DB")
                        .Padding(5).AlignCenter().Text("RESPONSABLE DE LA COTIZACION").Bold().FontSize(8).FontColor(CP);
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
                t.Cell().Border(1).BorderColor("#D1D5DB").Column(c =>
                {
                    c.Item().Background(CG).Padding(5).AlignCenter()
                        .Text("DEVENGADO").Bold().FontSize(8).FontColor(CP);
                    c.Item().Background(CA).Padding(4).AlignCenter()
                        .Text("RECEPCION DE MATERIALES Y/O SERVICIO").FontSize(7).Italic().FontColor(CT);
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
                                .Text(dto.AuthorizedForPayment == true ? "V" : "")
                                .FontSize(8).FontColor(CV);
                        });
                        inner.Item().Text("FORMA DE PAGO:").FontSize(7).Bold().FontColor(CT);
                        foreach (var (label, val) in new[]
                        {
                            ("CONTADO",          dto.CashAmount),
                            ("CREDITO",          dto.CreditAmount),
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
                        .Padding(3).Text("DESCRIPCION").Bold().FontSize(7).FontColor(CP);
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
        //  DOC 2 — RESUMEN COMPLETO
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
                        tc.Item().PaddingTop(2).Text("Direccion de Adquisiciones").FontSize(9).FontColor(CT);
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
                            ic.Item().PaddingTop(4).Text("FECHA DE AUTORIZACION").FontSize(7).FontColor(CT).Bold();
                            ic.Item().Text(dto.AuthorizationDate.Value.ToString("dd/MM/yyyy"))
                                .FontSize(10).FontColor(CP);
                        }
                        if (dto.MaxCompletionDate.HasValue)
                        {
                            ic.Item().PaddingTop(4).Text("FECHA LIMITE").FontSize(7).FontColor(CT).Bold();
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
                        r.RelativeItem().Element(c => SummaryField(c, "Tipo de Adquisicion", dto.AcquisitionType));
                        r.ConstantItem(10);
                        r.RelativeItem().Element(c => SummaryField(c, "Clasificacion", dto.Classification));
                    });
                });

                if (!string.IsNullOrWhiteSpace(dto.Community) || !string.IsNullOrWhiteSpace(dto.Beneficiary))
                {
                    col.Item().Element(c => SharedSectionTitle(c, "Localizacion y Beneficiario"));
                    col.Item().Background(CG).Border(1).BorderColor("#E5E7EB").Padding(10).Row(r =>
                    {
                        r.RelativeItem().Element(c => SummaryField(c, "Comunidad / Localidad", dto.Community));
                        r.ConstantItem(10);
                        r.RelativeItem().Element(c => SummaryField(c, "Beneficiario", dto.Beneficiary));
                    });
                }

                col.Item().Element(c => SharedSectionTitle(c, "Justificacion"));
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
                            r.RelativeItem().Element(c => SummaryField(c, "Poliza de Pago", dto.PaymentPolicy));
                            r.ConstantItem(10);
                            r.RelativeItem().Element(c => SummaryField(c, "CFDI", dto.CFDI));
                        });
                    });
                }

                col.Item().Background(CVF).Border(1).BorderColor("#86EFAC").Padding(8).Row(r =>
                {
                    r.ConstantItem(20).AlignMiddle().Text("V").FontSize(12).FontColor(CV).Bold();
                    r.RelativeItem().Column(c =>
                    {
                        c.Item().Text("Documento Validado").Bold().FontSize(10).FontColor(CV);
                        var audit = $"Generado el {dto.CreatedAt:dd/MM/yyyy} a las {dto.CreatedAt:HH:mm}";
                        if (!string.IsNullOrWhiteSpace(dto.CreatedByName))
                            audit += $"  -  Creado por: {dto.CreatedByName}";
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
                    foreach (var cell in new[] { "Cant.", "Unidad", "Descripcion", "Precio Unit.", "Total" })
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
                    c.Item().PaddingTop(4).Text($"Poliza de pago: {dto.PaymentPolicy ?? "_______________"}").FontSize(9).FontColor(CT);
                    if (!string.IsNullOrWhiteSpace(dto.CFDI))
                        c.Item().PaddingTop(4).Text($"CFDI: {dto.CFDI}").FontSize(9).FontColor(CT);
                });
            });
        }

        // ════════════════════════════════════════════════════════
        //  SHARED
        // ════════════════════════════════════════════════════════
        private void SharedSectionTitle(IContainer c, string title)
        {
            c.Height(16).Row(row =>
            {
                row.ConstantItem(3).Background(CS);
                row.ConstantItem(7);
                row.RelativeItem().AlignMiddle()
                    .Text(title.ToUpper()).Bold().FontSize(9).FontColor(CP).LetterSpacing(0.05f);
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
                        .FontSize(7).FontColor(CT);
                    row.ConstantItem(110).AlignRight().Text(x =>
                    {
                        x.Span("Pagina ").FontSize(7).FontColor(CT);
                        x.CurrentPageNumber().FontSize(7).FontColor(CT);
                        x.Span(" de ").FontSize(7).FontColor(CT);
                        x.TotalPages().FontSize(7).FontColor(CT);
                    });
                });
            });
        }

        // ════════════════════════════════════════════════════════
        //  HELPERS
        // ════════════════════════════════════════════════════════
        private static string NormalizeCode(string? s)
        {
            if (string.IsNullOrWhiteSpace(s)) return string.Empty;
            return s.ToUpperInvariant()
                .Replace(" ", "_")
                .Replace("E\u0301", "E").Replace("O\u0301", "O").Replace("A\u0301", "A")
                .Replace("I\u0301", "I").Replace("U\u0301", "U")
                .Replace("\u00C9", "E").Replace("\u00D3", "O").Replace("\u00C1", "A")
                .Replace("\u00CD", "I").Replace("\u00DA", "U");
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