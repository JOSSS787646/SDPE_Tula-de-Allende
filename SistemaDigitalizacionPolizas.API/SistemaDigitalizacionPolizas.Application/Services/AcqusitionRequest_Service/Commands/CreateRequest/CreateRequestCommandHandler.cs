using MediatR;
using SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.CreateRequest;
using SistemaDigitalizacionPolizas.Domain.Entities.AcquisitionRequest_Entities;
using SistemaDigitalizacionPolizas.Domain.Enums;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;

public class CreateRequestCommandHandler
    : IRequestHandler<CreateRequestCommand, int>
{
    private readonly IAcquisitionRequest _repository;
    private readonly ICurrentUserService _currentUser;
    private readonly IUnitOfWorkService _unitOfWork;

    public CreateRequestCommandHandler(
        IAcquisitionRequest repository,
        ICurrentUserService currentUser,
        IUnitOfWorkService unitOfWork)
    {
        _repository = repository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(
        CreateRequestCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        // ==========================================
        // Validar si el código ya existe
        // ==========================================
        var exists = await _repository.ExistsByRequestNumberAsync(dto.RequestNumber);

        if (exists)
            throw new Exception($"Ya existe una solicitud con el código '{dto.RequestNumber}'.");

        // ==========================================
        // Validar que tenga detalles 🔥
        // ==========================================
        if (dto.Details == null || !dto.Details.Any())
            throw new Exception("La solicitud debe tener al menos un detalle.");

        // ==========================================
        // Crear entidad
        // ==========================================
        var entity = new AcquisitionRequest
        {
            RequestNumber = dto.RequestNumber,
            RequestDate = dto.RequestDate ?? DateTime.UtcNow,
            Justification = dto.Justification,
            AuthorizationDate = dto.AuthorizationDate,
            Observations = dto.Observations,
            CFDI = dto.CFDI,
            CompleteMaximeDate = dto.CompleteMaximeDate,

            IdAdministrativeUnit = dto.IdAdministrativeUnit,
            IdProject = dto.IdProject,
            IdAcquisitionType = dto.IdAcquisitionType,
            IdSupplier = dto.IdSupplier,
            IdApplicationStatus = (int)RequestStatusEnum.EnRevision,
            IdFundingSource = dto.IdFundingSource,
            IdAcquisitionClassification = dto.IdAcquisitionClassification,
            IdProgram = dto.IdProgram,
            IdCommunity = dto.IdCommunity,
            IdBeneficiary = dto.IdBeneficiary,
            IdPaymentPolicy = dto.IdPayementPolicy,

            CreatedBy = _currentUser.UserId,
            CreatedAt = DateTime.UtcNow,
            Active = true
        };

        // ==========================================
        // 🔥 AGREGAR DETALLES (AQUÍ ESTÁ LA CLAVE)
        // ==========================================
        foreach (var d in dto.Details)
        {
            var detail = new ApplicationDetail
            {
                // FK se asigna automáticamente por EF
                CogId = d.IdCog,
                Quantity = d.Quantity,
                UnitMeasure = d.UnitMeasure,
                Description = d.Description,
                UnitAmount = d.UnitAmount,

                // 🔥 cálculo backend (obligatorio)
                TotalAmount = d.Quantity * d.UnitAmount,

                CreatedBy = _currentUser.UserId,
                CreatedDate = DateTime.UtcNow,
                Active = true
            };

            entity.Details.Add(detail);
        }

        // ==========================================
        // Guardar (guarda TODO: solicitud + detalles)
        // ==========================================
        await _repository.AddAsync(entity);

        return entity.IdRequest;
    }
}