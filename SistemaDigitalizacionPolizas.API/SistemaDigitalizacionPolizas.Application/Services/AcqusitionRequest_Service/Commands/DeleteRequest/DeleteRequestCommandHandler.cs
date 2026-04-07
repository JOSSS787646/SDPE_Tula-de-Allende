using MediatR;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using BCrypt.Net;
using SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.DeleteRequest;


public class DeleteRequestCommandHandler
    : IRequestHandler<DeleteRequestCommand, bool>
{
    private readonly IAcquisitionRequest _requestRepository;
    private readonly IDocumentExpedientRepository _documentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly ICurrentUserService _currentUser;

    public DeleteRequestCommandHandler(
        IAcquisitionRequest requestRepository,
        IDocumentExpedientRepository documentRepository,
        IUserRepository userRepository,
        IFileStorageService fileStorageService,
        ICurrentUserService currentUser)
    {
        _requestRepository = requestRepository;
        _documentRepository = documentRepository;
        _userRepository = userRepository;
        _fileStorageService = fileStorageService;
        _currentUser = currentUser;
    }

    public async Task<bool> Handle(
        DeleteRequestCommand request,
        CancellationToken cancellationToken)
    {
        // 1️⃣ Obtener usuario actual
        var userId = _currentUser.UserId;

        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
            throw new Exception("Usuario no encontrado");

        // 2️⃣ Validar contraseña
        var validPassword = BCrypt.Net.BCrypt.Verify(
            request.Password,
            user.Password
        );

        if (!validPassword)
            throw new Exception("Contraseña incorrecta");

        // 3️⃣ Obtener documentos
        var documents = await _documentRepository
            .GetActiveByRequestId(request.RequestId);

        // 4️⃣ eliminar archivos en paralelo
        var deleteTasks = documents
            .Where(d => !string.IsNullOrEmpty(d.FilePath))
            .Select(d => _fileStorageService.DeleteFileAsync(d.FilePath));

        await Task.WhenAll(deleteTasks);

        // 5️⃣ eliminar en cascada en BD
        await _requestRepository.DeleteCascadeAsync(request.RequestId);

        return true;
    }
}