using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SistemaDigitalizacionPolizas.Domain.Dtos.Pdf;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Services.Pdf;
using System.Globalization;

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
                    page.Margin(36);
                    page.Header().Element(c => ComposeHeader(c, dto));
                    page.Content().Element(c => ComposeContent(c, dto));
                    page.Footer().Element(ComposeFooter);
                });
            });

            return document.GeneratePdf();
        }

        // ─────────────────────────────────────────
        // HEADER
        // ─────────────────────────────────────────
        void ComposeHeader(IContainer container, AcquisitionRequestPdfDto dto)
        {
            container.Column(col =>
            {
                col.Item().Background(ColorPrimario).Height(4);

                col.Item().PaddingTop(10).PaddingBottom(10).Row(row =>
                {
                    row.ConstantItem(65).Height(65).Image("wwwroot/logo.png");
                    row.ConstantItem(15);

                    row.RelativeItem().Column(titleCol =>
                    {
                        titleCol.Item().Text("PRESIDENCIA MUNICIPAL DE TULA DE ALLENDE")
                            .Bold().FontSize(13).FontColor(ColorPrimario);
                        titleCol.Item().PaddingTop(3).Text("Dirección de Adquisiciones")
                            .FontSize(10).FontColor(ColorTextoSuave);
                        titleCol.Item().PaddingTop(2).Text("Solicitud de Materiales y Servicios")
                            .FontSize(10).Bold().FontColor(ColorSecundario);
                    });

                    // Caja folio / fechas
                    row.ConstantItem(160).Background(ColorAcento).Border(1)
                        .BorderColor(ColorSecundario).Padding(8).Column(infoCol =>
                        {
                            infoCol.Item().Text("FOLIO").FontSize(7).FontColor(ColorTextoSuave).Bold();
                            infoCol.Item().Text(dto.Folio ?? DateTime.Now.Ticks.ToString().Substring(10))
                                .FontSize(11).Bold().FontColor(ColorPrimario);

                            infoCol.Item().PaddingTop(5).Text("FECHA DE SOLICITUD")
                                .FontSize(7).FontColor(ColorTextoSuave).Bold();
                            infoCol.Item().Text((dto.RequestDate ?? DateTime.Now).ToString("dd/MM/yyyy"))
                                .FontSize(10).FontColor(ColorPrimario);

                            if (dto.AuthorizationDate.HasValue)
                            {
                                infoCol.Item().PaddingTop(4).Text("FECHA DE AUTORIZACIÓN")
                                    .FontSize(7).FontColor(ColorTextoSuave).Bold();
                                infoCol.Item().Text(dto.AuthorizationDate.Value.ToString("dd/MM/yyyy"))
                                    .FontSize(10).FontColor(ColorPrimario);
                            }

                            if (dto.MaxCompletionDate.HasValue)
                            {
                                infoCol.Item().PaddingTop(4).Text("FECHA LÍMITE")
                                    .FontSize(7).FontColor(ColorTextoSuave).Bold();
                                infoCol.Item().Text(dto.MaxCompletionDate.Value.ToString("dd/MM/yyyy"))
                                    .FontSize(10).FontColor(ColorNaranja);
                            }
                        });
                });

                col.Item().Background(ColorSecundario).Height(2);
            });
        }

        // ─────────────────────────────────────────
        // FOOTER
        // ─────────────────────────────────────────
        void ComposeFooter(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Background(ColorSecundario).Height(1);
                col.Item().PaddingTop(6).Row(row =>
                {
                    row.RelativeItem().Text("Presidencia Municipal de Tula de Allende")
                        .FontSize(8).FontColor(ColorTextoSuave);
                    row.ConstantItem(100).AlignRight().Text(x =>
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
        void ComposeContent(IContainer container, AcquisitionRequestPdfDto dto)
        {
            container.PaddingTop(12).Column(col =>
            {
                col.Spacing(12);

                // ── Status badge ──────────────────────────
                if (!string.IsNullOrWhiteSpace(dto.Status))
                    col.Item().Element(c => ComposeStatusBadge(c, dto.Status));

                // ── Datos generales ───────────────────────
                col.Item().Element(c => ComposeSectionTitle(c, "Datos Generales"));
                col.Item().Background(ColorGrisClaro).Border(1).BorderColor("#E5E7EB")
                    .Padding(12).Column(data =>
                    {
                        data.Spacing(6);

                        data.Item().Row(r =>
                        {
                            r.RelativeItem().Element(c => ComposeField(c,
                                "Unidad Administrativa", dto.AdministrativeUnit));
                            r.ConstantItem(12);
                            r.RelativeItem().Element(c => ComposeField(c,
                                "Programa", dto.Program));
                        });

                        data.Item().Row(r =>
                        {
                            r.RelativeItem().Element(c => ComposeField(c,
                                "Proyecto", dto.Project));
                            r.ConstantItem(12);
                            r.RelativeItem().Element(c => ComposeField(c,
                                "Fuente de Financiamiento", dto.FundingSource));
                        });

                        data.Item().Row(r =>
                        {
                            r.RelativeItem().Element(c => ComposeField(c,
                                "Tipo de Adquisición", dto.AcquisitionType));
                            r.ConstantItem(12);
                            r.RelativeItem().Element(c => ComposeField(c,
                                "Clasificación", dto.Classification));
                        });
                    });

                // ── Localización / Beneficiario ───────────
                if (!string.IsNullOrWhiteSpace(dto.Community) ||
                    !string.IsNullOrWhiteSpace(dto.Beneficiary))
                {
                    col.Item().Element(c => ComposeSectionTitle(c, "Localización y Beneficiario"));
                    col.Item().Background(ColorGrisClaro).Border(1).BorderColor("#E5E7EB")
                        .Padding(12).Row(r =>
                        {
                            r.RelativeItem().Element(c => ComposeField(c,
                                "Comunidad / Localidad", dto.Community));
                            r.ConstantItem(12);
                            r.RelativeItem().Element(c => ComposeField(c,
                                "Beneficiario", dto.Beneficiary));
                        });
                }

                // ── Justificación ─────────────────────────
                col.Item().Element(c => ComposeSectionTitle(c, "Justificación"));
                col.Item().Border(1).BorderColor("#E5E7EB").Padding(12)
                    .Text(dto.Justification ?? "N/A").FontSize(10);

                // ── Observaciones ─────────────────────────
                if (!string.IsNullOrWhiteSpace(dto.Observations))
                {
                    col.Item().Element(c => ComposeSectionTitle(c, "Observaciones"));
                    col.Item().Border(1).BorderColor("#E5E7EB").Padding(12)
                        .Text(dto.Observations).FontSize(10).FontColor(ColorTextoSuave);
                }

                // ── Detalle de materiales ─────────────────
                col.Item().Element(c => ComposeSectionTitle(c, "Detalle de Materiales"));
                col.Item().Element(c => ComposeTable(c, dto.Items, dto.TotalAmount));

                // ── Proveedor / Pago ──────────────────────
                if (!string.IsNullOrWhiteSpace(dto.Supplier) ||
                    !string.IsNullOrWhiteSpace(dto.PaymentPolicy))
                {
                    col.Item().Element(c => ComposeSectionTitle(c, "Proveedor y Pago"));
                    col.Item().Background(ColorDoradoClaro).Border(1)
                        .BorderColor(ColorDorado).Padding(12).Column(data =>
                        {
                            data.Spacing(6);
                            data.Item().Row(r =>
                            {
                                r.RelativeItem().Element(c => ComposeField(c,
                                    "Proveedor", dto.Supplier));
                                r.ConstantItem(12);
                                r.RelativeItem().Element(c => ComposeField(c,
                                    "RFC", dto.SupplierRFC));
                            });
                            data.Item().Row(r =>
                            {
                                r.RelativeItem().Element(c => ComposeField(c,
                                    "Póliza de Pago", dto.PaymentPolicy));
                                r.ConstantItem(12);
                                r.RelativeItem().Element(c => ComposeField(c,
                                    "CFDI", dto.CFDI));
                            });
                        });
                }

                // ── Validación ────────────────────────────
                col.Item().PaddingTop(4).Background(ColorVerdeFondo).Border(1)
                    .BorderColor("#86EFAC").Padding(10).Row(r =>
                    {
                        r.ConstantItem(16).AlignMiddle().Text("✔")
                            .FontSize(12).FontColor(ColorVerde).Bold();
                        r.ConstantItem(6);
                        r.RelativeItem().AlignMiddle().Column(c2 =>
                        {
                            c2.Item().Text("Documento Validado")
                                .Bold().FontSize(10).FontColor(ColorVerde);
                            c2.Item().Text(
                                $"Generado el {dto.CreatedAt:dd/MM/yyyy} a las {dto.CreatedAt:HH:mm}" +
                                (string.IsNullOrWhiteSpace(dto.CreatedByName)
                                    ? ""
                                    : $"  •  Creado por: {dto.CreatedByName}"))
                                .FontSize(8).FontColor(ColorVerde);
                        });
                    });

                // ── Secciones contables ───────────────────
                col.Item().PaddingTop(6).Element(c => ComposeEmptySections(c, dto));
            });
        }

        // ─────────────────────────────────────────
        // HELPERS VISUALES
        // ─────────────────────────────────────────

        void ComposeSectionTitle(IContainer container, string title)
        {
            container.Row(row =>
            {
                row.ConstantItem(4).Background(ColorSecundario);
                row.ConstantItem(8);
                row.RelativeItem().AlignMiddle()
                    .Text(title).Bold().FontSize(10).FontColor(ColorPrimario);
            });
        }

        // Campo etiqueta + valor
        void ComposeField(IContainer container, string label, string? value)
        {
            container.Column(col =>
            {
                col.Item().Text(label).FontSize(8).FontColor(ColorTextoSuave).Bold();
                col.Item().Text(value ?? "—").FontSize(10);
            });
        }

        // Badge de estatus
        void ComposeStatusBadge(IContainer container, string status)
        {
            var (bg, fg) = status.ToUpperInvariant() switch
            {
                "AUTORIZADO" => (ColorVerdeFondo, ColorVerde),
                "PENDIENTE" => (ColorNaranjaFondo, ColorNaranja),
                "RECHAZADO" => ("#FEE2E2", "#991B1B"),
                _ => (ColorGrisClaro, ColorTextoSuave)
            };

            container.AlignRight().Background(bg).Border(1).BorderColor(fg)
                .PaddingHorizontal(12).PaddingVertical(5)
                .Text($"Estado: {status}").Bold().FontSize(9).FontColor(fg);
        }

        // ─────────────────────────────────────────
        // TABLA
        // ─────────────────────────────────────────
        void ComposeTable(IContainer container,
                          List<AcquisitionRequestPdfItemDto> items,
                          decimal totalAmount)
        {
            var culture = new CultureInfo("es-MX");
            bool hasCog = items.Any(i => !string.IsNullOrWhiteSpace(i.CogKey));

            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    if (hasCog) columns.ConstantColumn(55); // COG
                    columns.ConstantColumn(45);             // Cant.
                    columns.ConstantColumn(65);             // Unidad
                    columns.RelativeColumn();               // Descripción
                    columns.ConstantColumn(75);             // Precio
                    columns.ConstantColumn(85);             // Total
                });

                table.Header(header =>
                {
                    if (hasCog)
                        header.Cell().Background(ColorPrimario).Padding(7)
                            .Text("COG").Bold().FontSize(8).FontColor(Colors.White);

                    foreach (var cell in new[] { "Cant.", "Unidad", "Descripción", "Precio Unit.", "Total" })
                        header.Cell().Background(ColorPrimario).Padding(7)
                            .Text(cell).Bold().FontSize(9).FontColor(Colors.White);
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
                        table.Cell().Background(rowBg).Padding(5).AlignMiddle()
                            .Text(item.CogKey ?? "").FontSize(8).FontColor(ColorTextoSuave);

                    table.Cell().Background(rowBg).Padding(6).AlignMiddle()
                        .Text(item.Quantity.ToString("G29")).FontSize(9);
                    table.Cell().Background(rowBg).Padding(6).AlignMiddle()
                        .Text(item.UnitMeasure).FontSize(9);
                    table.Cell().Background(rowBg).Padding(6).AlignMiddle()
                        .Column(c =>
                        {
                            c.Item().Text(item.Description).FontSize(9);
                            if (!string.IsNullOrWhiteSpace(item.CogName))
                                c.Item().Text(item.CogName).FontSize(7)
                                    .FontColor(ColorTextoSuave);
                        });
                    table.Cell().Background(rowBg).Padding(6).AlignMiddle().AlignRight()
                        .Text(item.UnitPrice.ToString("C", culture)).FontSize(9);
                    table.Cell().Background(rowBg).Padding(6).AlignMiddle().AlignRight()
                        .Text(rowTotal.ToString("C", culture)).FontSize(9);
                }

                // Fila total
                int spanCount = hasCog ? 5 : 4;
                decimal displayTotal = totalAmount > 0 ? totalAmount : calculatedTotal;

                table.Cell().ColumnSpan((uint)spanCount).Background(ColorAcento)
                    .Padding(7).AlignRight()
                    .Text("TOTAL GENERAL").Bold().FontSize(9).FontColor(ColorPrimario);
                table.Cell().Background(ColorAcento).Padding(7).AlignRight()
                    .Text(displayTotal.ToString("C", culture))
                    .Bold().FontSize(10).FontColor(ColorPrimario);
            });
        }

        // ─────────────────────────────────────────
        // SECCIONES CONTABLES
        // ─────────────────────────────────────────
        void ComposeEmptySections(IContainer container, AcquisitionRequestPdfDto dto)
        {
            container.Row(row =>
            {
                // COMPROMETIDO
                row.RelativeItem().Border(1).BorderColor("#E5E7EB").Padding(10).Column(col =>
                {
                    col.Item().Text("COMPROMETIDO").Bold().FontSize(9).FontColor(ColorPrimario);
                    col.Item().PaddingTop(8)
                        .Text("Fecha de pedido: _______________")
                        .FontSize(9).FontColor(ColorTextoSuave);
                });

                row.ConstantItem(8);

                // DEVENGADO
                row.RelativeItem().Border(1).BorderColor("#E5E7EB").Padding(10).Column(col =>
                {
                    col.Item().Text("DEVENGADO").Bold().FontSize(9).FontColor(ColorPrimario);
                    col.Item().PaddingTop(8)
                        .Text("Fecha de recibido: _______________")
                        .FontSize(9).FontColor(ColorTextoSuave);
                });

                row.ConstantItem(8);

                // EJERCIDO / PAGADO — pre-relleno si hay datos
                row.RelativeItem().Border(1).BorderColor("#E5E7EB").Padding(10).Column(col =>
                {
                    col.Item().Text("EJERCIDO / PAGADO").Bold().FontSize(9).FontColor(ColorPrimario);
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