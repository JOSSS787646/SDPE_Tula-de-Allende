using MediatR;
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

        // ==========================================
        // Validar si el código de adquisición ya existe
        // ==========================================

        var exists = await _repository.ExistsByRequestNumberAsync(dto.RequestNumber);

        if (exists)
            throw new Exception($"Ya existe una solicitud con el código '{dto.RequestNumber}'.");

        // ==========================================
        // Obtener estado inicial (Borrador)
        // ==========================================


        var initialStatus = await _statusRepository.GetByCodeAsync(1);

        if (initialStatus == null)
            throw new Exception("No existe estado inicial configurado.");

        // ==========================================
        // Crear entidad
        // ==========================================

        var entity = new AcquisitionRequest
        {
            // ===============================
            // Información del negocio
            // ===============================

            RequestNumber = dto.RequestNumber,
            RequestDate = dto.RequestDate,
            Justification = dto.Justification,
            AuthorizationDate = dto.AuthorizationDate,
            Observations = dto.Observations,
            CFDI = dto.CFDI,

            // ===============================
            // Foreign Keys
            // ===============================

            IdAdministrativeUnit = dto.IdAdministrativeUnit,
            IdProject = dto.IdProject,
            IdAcquisitionType = dto.IdAcquisitionType,
            IdSupplier = dto.IdSupplier,
            IdApplicationStatus = initialStatus.IdApplicationStatus,
            IdFundingSource = dto.IdFundingSource,
            IdAcquisitionClassification = dto.IdAcquisitionClassification,
            IdProgram = dto.IdProgram,
            IdCommunity = dto.IdCommunity,
            IdBeneficiary = dto.IdBeneficiary,
            IdPaymentPolicy = dto.IdPayementPolicy,

            // ===============================
            // Auditoría
            // ===============================

            CreatedBy = _currentUser.UserId,
            CreatedAt = DateTime.UtcNow,
            Active = true
        };

        // ==========================================
        // Guardar
        // ==========================================

        await _repository.AddAsync(entity);

        return entity.IdRequest;
    }
}