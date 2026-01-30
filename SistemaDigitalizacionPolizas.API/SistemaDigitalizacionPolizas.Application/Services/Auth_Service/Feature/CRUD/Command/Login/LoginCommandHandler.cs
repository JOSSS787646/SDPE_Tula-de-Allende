
public class LoginCommandHandler
    : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<LoginResponse> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository
            .GetUserWithRolesAndPermissionsAsync(request.Email);

        if (user is null)
            throw new UnauthorizedAccessException("Credenciales inválidas");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            throw new UnauthorizedAccessException("Credenciales inválidas");

        if (!user.Asset)
            throw new UnauthorizedAccessException("Usuario inactivo");

        if (!user.Role.Asset)
            throw new UnauthorizedAccessException("El rol del usuario está inactivo");

        await _userRepository.UpdateLastAccessAsync(user.IdUser);

        var tokenResult = _jwtService.GenerateToken(user);

        return LoginResponseMapper.Map(user, tokenResult);
    }
}
