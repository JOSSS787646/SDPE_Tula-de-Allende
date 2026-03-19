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
        private const string ColorPrimario = "#6A1B1B";
        private const string ColorSecundario = "#8B2C2C";
        private const string ColorAcento = "#FDF2F2";
        private const string ColorGrisClaro = "#F5F5F5";
        private const string ColorTextoSuave = "#6B7280";
        private const string ColorDorado = "#C9A227";
        private const string ColorDoradoClaro = "#F4E4BC";
        private const string ColorVerde = "#166534";
        private const string ColorVerdeFondo = "#DCFCE7";
        private const string ColorNaranja = "#92400E";
        private const string ColorNaranjaFondo = "#FEF3C7";

        public byte[] GenerateAcquisitionRequestPdf(AcquisitionRequestPdfDto dto)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter);
                    page.MarginHorizontal(36);
                    page.MarginTop(36);
                    // ✅ Footer necesita margen generoso para no solaparse
                    page.MarginBottom(50);
                    page.Header().Element(c => ComposeHeader(c, dto));
                    page.Content().Element(c => ComposeContent(c, dto));
                    page.Footer().Element(ComposeFooter);
                });
            });

            return document.GeneratePdf();
        }

        // ─────────────────────────────────────────
        // LOGO
        // ─────────────────────────────────────────
        private byte[]? GetLogo()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName =
                "SistemaDigitalizacionPolizas.Infrastructure.Resources.Images.LogoPresidencia.webp";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null) return null;

            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            return ms.ToArray();
        }

        // ─────────────────────────────────────────
        // HEADER
        // ─────────────────────────────────────────
        private void ComposeHeader(IContainer container, AcquisitionRequestPdfDto dto)
        {
            var logo = GetLogo();

            container.Column(col =>
            {
                // Franja superior
                col.Item().Background(ColorPrimario).Height(4);

                col.Item().PaddingTop(8).PaddingBottom(8).Row(row =>
                {
                    // Logo
                    if (logo != null)
                        row.ConstantItem(60).Height(60).Image(logo, ImageScaling.FitArea);
                    else
                        row.ConstantItem(60).Height(60);

                    row.ConstantItem(10);

                    // Títulos
                    row.RelativeItem().AlignMiddle().Column(titleCol =>
                    {
                        titleCol.Item()
                            .Text("PRESIDENCIA MUNICIPAL DE TULA DE ALLENDE")
                            .Bold().FontSize(12).FontColor(ColorPrimario);

                        titleCol.Item().PaddingTop(2)
                            .Text("Dirección de Adquisiciones")
                            .FontSize(9).FontColor(ColorTextoSuave);

                        titleCol.Item().PaddingTop(2)
                            .Text("Solicitud de Materiales y Servicios")
                            .FontSize(9).Bold().FontColor(ColorSecundario);
                    });

                    row.ConstantItem(10);

                    // ✅ Caja de folio — ancho fijo, sin AlignMiddle conflictivo
                    row.ConstantItem(150).Background(ColorAcento)
                        .Border(1).BorderColor(ColorSecundario)
                        .Padding(7).Column(infoCol =>
                        {
                            infoCol.Item()
                                .Text("FOLIO")
                                .FontSize(7).FontColor(ColorTextoSuave).Bold();

                            infoCol.Item()
                                .Text(dto.Folio ?? "—")
                                .FontSize(10).Bold().FontColor(ColorPrimario);

                            infoCol.Item().PaddingTop(4)
                                .Text("FECHA DE SOLICITUD")
                                .FontSize(7).FontColor(ColorTextoSuave).Bold();

                            infoCol.Item()
                                .Text((dto.RequestDate ?? DateTime.Now).ToString("dd/MM/yyyy"))
                                .FontSize(10).FontColor(ColorPrimario);

                            // ✅ Fechas opcionales solo si tienen valor
                            if (dto.AuthorizationDate.HasValue)
                            {
                                infoCol.Item().PaddingTop(4)
                                    .Text("FECHA DE AUTORIZACIÓN")
                                    .FontSize(7).FontColor(ColorTextoSuave).Bold();

                                infoCol.Item()
                                    .Text(dto.AuthorizationDate.Value.ToString("dd/MM/yyyy"))
                                    .FontSize(10).FontColor(ColorPrimario);
                            }

                            if (dto.MaxCompletionDate.HasValue)
                            {
                                infoCol.Item().PaddingTop(4)
                                    .Text("FECHA LÍMITE")
                                    .FontSize(7).FontColor(ColorTextoSuave).Bold();

                                infoCol.Item()
                                    .Text(dto.MaxCompletionDate.Value.ToString("dd/MM/yyyy"))
                                    .FontSize(10).FontColor(ColorNaranja);
                            }
                        });
                });

                // Línea divisora
                col.Item().Background(ColorSecundario).Height(2);
            });
        }

        // ─────────────────────────────────────────
        // FOOTER
        // ─────────────────────────────────────────
        private void ComposeFooter(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Background(ColorSecundario).Height(1);
                col.Item().PaddingTop(5).Row(row =>
                {
                    row.RelativeItem()
                        .Text("Presidencia Municipal de Tula de Allende")
                        .FontSize(8).FontColor(ColorTextoSuave);

                    row.ConstantItem(110).AlignRight().Text(x =>
                    {
                        x.Span("Página ").FontSize(8).FontColor(ColorTextoSuave);
                        x.CurrentPageNumber().FontSize(8).FontColor(ColorTextoSuave);
                        x.Span(" de ").FontSize(8).FontColor(ColorTextoSuave);
                        x.TotalPages().FontSize(8).FontColor(ColorTextoSuave);
                    });
                });
            });
        }

        // ─────────────────────────────────────────
        // CONTENT
        // ─────────────────────────────────────────
        private void ComposeContent(IContainer container, AcquisitionRequestPdfDto dto)
        {
            container.PaddingTop(10).Column(col =>
            {
                col.Spacing(10);

                // Status badge
                if (!string.IsNullOrWhiteSpace(dto.Status))
                    col.Item().Element(c => ComposeStatusBadge(c, dto.Status!));

                // Datos generales
                col.Item().Element(c => ComposeSectionTitle(c, "Datos Generales"));
                col.Item().Background(ColorGrisClaro).Border(1).BorderColor("#E5E7EB")
                    .Padding(10).Column(data =>
                    {
                        data.Spacing(8);

                        data.Item().Row(r =>
                        {
                            r.RelativeItem().Element(c =>
                                ComposeField(c, "Unidad Administrativa", dto.AdministrativeUnit));
                            r.ConstantItem(10);
                            r.RelativeItem().Element(c =>
                                ComposeField(c, "Programa", dto.Program));
                        });

                        data.Item().Row(r =>
                        {
                            r.RelativeItem().Element(c =>
                                ComposeField(c, "Proyecto", dto.Project));
                            r.ConstantItem(10);
                            r.RelativeItem().Element(c =>
                                ComposeField(c, "Fuente de Financiamiento", dto.FundingSource));
                        });

                        data.Item().Row(r =>
                        {
                            r.RelativeItem().Element(c =>
                                ComposeField(c, "Tipo de Adquisición", dto.AcquisitionType));
                            r.ConstantItem(10);
                            r.RelativeItem().Element(c =>
                                ComposeField(c, "Clasificación", dto.Classification));
                        });
                    });

                // Localización / Beneficiario (condicional)
                if (!string.IsNullOrWhiteSpace(dto.Community) ||
                    !string.IsNullOrWhiteSpace(dto.Beneficiary))
                {
                    col.Item().Element(c => ComposeSectionTitle(c, "Localización y Beneficiario"));
                    col.Item().Background(ColorGrisClaro).Border(1).BorderColor("#E5E7EB")
                        .Padding(10).Row(r =>
                        {
                            r.RelativeItem().Element(c =>
                                ComposeField(c, "Comunidad / Localidad", dto.Community));
                            r.ConstantItem(10);
                            r.RelativeItem().Element(c =>
                                ComposeField(c, "Beneficiario", dto.Beneficiary));
                        });
                }

                // Justificación
                col.Item().Element(c => ComposeSectionTitle(c, "Justificación"));
                // ✅ ShowEntire evita que el bloque se corte entre páginas
                col.Item().ShowEntire().Border(1).BorderColor("#E5E7EB").Padding(10)
                    .Text(string.IsNullOrWhiteSpace(dto.Justification) ? "N/A" : dto.Justification)
                    .FontSize(10);

                // Observaciones (condicional)
                if (!string.IsNullOrWhiteSpace(dto.Observations))
                {
                    col.Item().Element(c => ComposeSectionTitle(c, "Observaciones"));
                    col.Item().ShowEntire().Border(1).BorderColor("#E5E7EB").Padding(10)
                        .Text(dto.Observations).FontSize(10).FontColor(ColorTextoSuave);
                }

                // Detalle de materiales
                col.Item().Element(c => ComposeSectionTitle(c, "Detalle de Materiales"));
                col.Item().Element(c => ComposeTable(c, dto.Items, dto.TotalAmount));

                // Proveedor / Pago (condicional)
                if (!string.IsNullOrWhiteSpace(dto.Supplier) ||
                    !string.IsNullOrWhiteSpace(dto.PaymentPolicy))
                {
                    col.Item().Element(c => ComposeSectionTitle(c, "Proveedor y Pago"));
                    col.Item().Background(ColorDoradoClaro).Border(1)
                        .BorderColor(ColorDorado).Padding(10).Column(data =>
                        {
                            data.Spacing(8);
                            data.Item().Row(r =>
                            {
                                r.RelativeItem().Element(c =>
                                    ComposeField(c, "Proveedor", dto.Supplier));
                                r.ConstantItem(10);
                                r.RelativeItem().Element(c =>
                                    ComposeField(c, "RFC", dto.SupplierRFC));
                            });
                            data.Item().Row(r =>
                            {
                                r.RelativeItem().Element(c =>
                                    ComposeField(c, "Póliza de Pago", dto.PaymentPolicy));
                                r.ConstantItem(10);
                                r.RelativeItem().Element(c =>
                                    ComposeField(c, "CFDI", dto.CFDI));
                            });
                        });
                }

                // Validación
                col.Item().Background(ColorVerdeFondo).Border(1)
                    .BorderColor("#86EFAC").Padding(8).Row(r =>
                    {
                        r.ConstantItem(20).AlignMiddle()
                            .Text("✔").FontSize(12).FontColor(ColorVerde).Bold();

                        r.RelativeItem().Column(c2 =>
                        {
                            c2.Item()
                                .Text("Documento Validado")
                                .Bold().FontSize(10).FontColor(ColorVerde);

                            var auditText = $"Generado el {dto.CreatedAt:dd/MM/yyyy} a las {dto.CreatedAt:HH:mm}";
                            if (!string.IsNullOrWhiteSpace(dto.CreatedByName))
                                auditText += $"  •  Creado por: {dto.CreatedByName}";

                            c2.Item().Text(auditText).FontSize(8).FontColor(ColorVerde);
                        });
                    });

                // Secciones contables
                col.Item().Element(c => ComposeEmptySections(c, dto));
            });
        }

        // ─────────────────────────────────────────
        // HELPERS VISUALES
        // ─────────────────────────────────────────

        private void ComposeSectionTitle(IContainer container, string title)
        {
            // ✅ Height fija para que la barra lateral no colapse
            container.Height(18).Row(row =>
            {
                row.ConstantItem(4).Background(ColorSecundario);
                row.ConstantItem(8);
                row.RelativeItem().AlignMiddle()
                    .Text(title).Bold().FontSize(10).FontColor(ColorPrimario);
            });
        }

        private void ComposeField(IContainer container, string label, string? value)
        {
            container.Column(col =>
            {
                col.Item()
                    .Text(label).FontSize(8).FontColor(ColorTextoSuave).Bold();
                col.Item()
                    .Text(string.IsNullOrWhiteSpace(value) ? "—" : value).FontSize(10);
            });
        }

        private void ComposeStatusBadge(IContainer container, string status)
        {
            var (bg, fg) = status.ToUpperInvariant() switch
            {
                "AUTORIZADO" => (ColorVerdeFondo, ColorVerde),
                "PENDIENTE" => (ColorNaranjaFondo, ColorNaranja),
                "RECHAZADO" => ("#FEE2E2", "#991B1B"),
                _ => (ColorGrisClaro, ColorTextoSuave)
            };

            // ✅ Wrapper Row evita conflicto de AlignRight directo sobre container
            container.Row(row =>
            {
                row.RelativeItem(); // Empuja el badge a la derecha
                row.AutoItem()
                    .Background(bg).Border(1).BorderColor(fg)
                    .PaddingHorizontal(10).PaddingVertical(4)
                    .Text($"Estado: {status}").Bold().FontSize(9).FontColor(fg);
            });
        }

        // ─────────────────────────────────────────
        // TABLA
        // ─────────────────────────────────────────
        private void ComposeTable(
            IContainer container,
            List<AcquisitionRequestPdfItemDto> items,
            decimal totalAmount)
        {
            var culture = new CultureInfo("es-MX");
            bool hasCog = items.Any(i => !string.IsNullOrWhiteSpace(i.CogKey));

            // ✅ Lista vacía — evitar tabla sin filas (crash garantizado en QuestPDF)
            if (!items.Any())
            {
                container.Border(1).BorderColor("#E5E7EB").Padding(10)
                    .Text("Sin partidas registradas.").FontSize(9).FontColor(ColorTextoSuave);
                return;
            }

            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    if (hasCog) columns.ConstantColumn(50);
                    columns.ConstantColumn(40);
                    columns.ConstantColumn(60);
                    columns.RelativeColumn();
                    columns.ConstantColumn(72);
                    columns.ConstantColumn(80);
                });

                // Header
                table.Header(header =>
                {
                    if (hasCog)
                        header.Cell().Background(ColorPrimario).Padding(6)
                            .Text("COG").Bold().FontSize(8).FontColor(Colors.White);

                    foreach (var cell in new[] { "Cant.", "Unidad", "Descripción", "Precio Unit.", "Total" })
                        header.Cell().Background(ColorPrimario).Padding(6)
                            .Text(cell).Bold().FontSize(8).FontColor(Colors.White);
                });

                bool isOdd = false;
                decimal calculatedTotal = 0;

                foreach (var item in items)
                {
                    decimal rowTotal = item.Total > 0
                        ? item.Total
                        : item.Quantity * item.UnitPrice;

                    calculatedTotal += rowTotal;
                    var rowBg = isOdd ? "#FFFFFF" : ColorGrisClaro;
                    isOdd = !isOdd;

                    if (hasCog)
                        table.Cell().Background(rowBg).Padding(5)
                            .Text(item.CogKey ?? "").FontSize(8).FontColor(ColorTextoSuave);

                    table.Cell().Background(rowBg).Padding(5)
                        .Text(item.Quantity.ToString("G29")).FontSize(9);

                    table.Cell().Background(rowBg).Padding(5)
                        .Text(item.UnitMeasure).FontSize(9);

                    // ✅ Sin AlignMiddle en celda con Column anidado
                    table.Cell().Background(rowBg).Padding(5).Column(c =>
                    {
                        c.Item().Text(item.Description).FontSize(9);
                        if (!string.IsNullOrWhiteSpace(item.CogName))
                            c.Item().Text(item.CogName).FontSize(7).FontColor(ColorTextoSuave);
                    });

                    table.Cell().Background(rowBg).Padding(5).AlignRight()
                        .Text(item.UnitPrice.ToString("C", culture)).FontSize(9);

                    table.Cell().Background(rowBg).Padding(5).AlignRight()
                        .Text(rowTotal.ToString("C", culture)).FontSize(9);
                }

                // Fila total
                uint spanCount = hasCog ? 5u : 4u;
                decimal displayTotal = totalAmount > 0 ? totalAmount : calculatedTotal;

                table.Cell().ColumnSpan(spanCount).Background(ColorAcento).Padding(6).AlignRight()
                    .Text("TOTAL GENERAL").Bold().FontSize(9).FontColor(ColorPrimario);

                table.Cell().Background(ColorAcento).Padding(6).AlignRight()
                    .Text(displayTotal.ToString("C", culture))
                    .Bold().FontSize(10).FontColor(ColorPrimario);
            });
        }

        // ─────────────────────────────────────────
        // SECCIONES CONTABLES
        // ─────────────────────────────────────────
        private void ComposeEmptySections(IContainer container, AcquisitionRequestPdfDto dto)
        {
            // ✅ MinHeight garantiza que las cajas no colapsen a 0
            container.Row(row =>
            {
                row.RelativeItem().MinHeight(70).Border(1).BorderColor("#E5E7EB")
                    .Padding(8).Column(col =>
                    {
                        col.Item().Text("COMPROMETIDO")
                            .Bold().FontSize(9).FontColor(ColorPrimario);
                        col.Item().PaddingTop(6)
                            .Text("Fecha de pedido: _______________")
                            .FontSize(9).FontColor(ColorTextoSuave);
                    });

                row.ConstantItem(6);

                row.RelativeItem().MinHeight(70).Border(1).BorderColor("#E5E7EB")
                    .Padding(8).Column(col =>
                    {
                        col.Item().Text("DEVENGADO")
                            .Bold().FontSize(9).FontColor(ColorPrimario);
                        col.Item().PaddingTop(6)
                            .Text("Fecha de recibido: _______________")
                            .FontSize(9).FontColor(ColorTextoSuave);
                    });

                row.ConstantItem(6);

                row.RelativeItem().MinHeight(70).Border(1).BorderColor("#E5E7EB")
                    .Padding(8).Column(col =>
                    {
                        col.Item().Text("EJERCIDO / PAGADO")
                            .Bold().FontSize(9).FontColor(ColorPrimario);
                        col.Item().PaddingTop(4)
                            .Text($"Proveedor: {dto.Supplier ?? "_______________"}")
                            .FontSize(9).FontColor(ColorTextoSuave);
                        col.Item().PaddingTop(4)
                            .Text($"RFC: {dto.SupplierRFC ?? "_______________"}")
                            .FontSize(9).FontColor(ColorTextoSuave);
                        col.Item().PaddingTop(4)
                            .Text($"Póliza de pago: {dto.PaymentPolicy ?? "_______________"}")
                            .FontSize(9).FontColor(ColorTextoSuave);
                        if (!string.IsNullOrWhiteSpace(dto.CFDI))
                            col.Item().PaddingTop(4)
                                .Text($"CFDI: {dto.CFDI}")
                                .FontSize(9).FontColor(ColorTextoSuave);
                    });
            });
        }
    }
}