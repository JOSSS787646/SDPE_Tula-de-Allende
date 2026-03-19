using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.Pdf_Service.Query.GetAcquisitionRequestPdf
{
    //public class GetAcquisitionRequestPdfHandler
    // : IRequestHandler<GetAcquisitionRequestPdfQuery, byte[]>
    //{
    //    private readonly ApplicationDbContext _context;
    //    private readonly IPdfService _pdfService;

    //    public GetAcquisitionRequestPdfHandler(
    //        ApplicationDbContext context,
    //        IPdfService pdfService)
    //    {
    //        _context = context;
    //        _pdfService = pdfService;
    //    }

    //    public async Task<byte[]> Handle(
    //        GetAcquisitionRequestPdfQuery request,
    //        CancellationToken cancellationToken)
    //    {
    //        var entity = await _context.AcquisitionRequests
    //            .Include(x => x.AdministrativeUnit)
    //            .Include(x => x.Program)
    //            .Include(x => x.Project)
    //            .Include(x => x.FundingSource)
    //            .Include(x => x.AcquisitionType)
    //            .Include(x => x.AcquisitionClassification)
    //            .Include(x => x.Supplier)
    //            .Include(x => x.PaymentPolicy)
    //            .Include(x => x.Community)
    //            .Include(x => x.Beneficiary)
    //            .Include(x => x.ApplicationStatus)
    //            .Include(x => x.Details)
    //                .ThenInclude(d => d.Cog)
    //            .FirstOrDefaultAsync(x => x.IdRequest == request.RequestId, cancellationToken);

    //        if (entity == null)
    //            throw new Exception("Request not found");

    //        // 🔥 MAPEO
    //        var dto = AcquisitionRequestMapper.ToPdfDto(entity);

    //        // 🔥 GENERAR PDF
    //        var pdfBytes = _pdfService.GenerateAcquisitionRequestPdf(dto);

    //        return pdfBytes;
    //    }
    //}

}