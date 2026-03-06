using SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.CreateRequest;
using SistemaDigitalizacionPolizas.Domain.Entities.AcquisitionRequest_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using SSistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.StatusRequest;

public class CreateRequestCommandHandler
   : IRequestHandler<CreateRequestCommand, int>
{
    private readonly IAcquisitionRequest _repository;
    private readonly IApplicationStatusRepository _statusRepository;
    private readonly ICurrentUserService _currentUser;
    private readonly IUnitOfWorkService _unitOfWork;

    public CreateRequestCommandHandler(
        IAcquisitionRequest repository,
        IApplicationStatusRepository statusRepository,
        ICurrentUserService currentUser,
        IUnitOfWorkService unitOfWork)
    {
        _repository = repository;
        _statusRepository = statusRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(
        CreateRequestCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        // 🔥 Estado inicial automático (Borrador)
        var borradorStatus = await _statusRepository.GetByCodeAsync(1);

        if (borradorStatus == null)
            throw new Exception("No existe estado 'Borrador' configurado.");

        var entity = new AcquisitionRequest
        {
            // ===============================
            // Información del Negocio
            // ===============================

            RequestNumber = dto.RequestNumber,
            RequestDate = dto.RequestDate,
            Justification = dto.Justification,
            AuthorizationDate = dto.AuthorizationDate,
            Observations = dto.Observations,

            // ===============================
            // Foreign Keys
            // ===============================

            IdAdministrativeUnit = dto.IdAdministrativeUnit,
            IdProject = dto.IdProject,
            IdAcquisitionType = dto.IdAcquisitionType,
            IdSupplier = dto.IdSupplier,
            IdApplicationStatus = borradorStatus.IdApplicationStatus,
            IdFundingSource = dto.IdFundingSource,
            IdAcquisitionClassification = dto.IdAcquisitionClassification,
            IdProgram = dto.IdProgram,
            IdCommunity = dto.IdCommunity,
            IdBeneficiary = dto.IdBeneficiary,
            IdPaymentPolicy=dto.IdPayementPolicy,

            // ===============================
            // Auditoría
            // ===============================

            CreatedBy = _currentUser.UserId,
            CreatedAt = DateTime.UtcNow,
            Active = true
        };

        await _repository.AddAsync(entity);

       

        return entity.IdRequest;
    }
}