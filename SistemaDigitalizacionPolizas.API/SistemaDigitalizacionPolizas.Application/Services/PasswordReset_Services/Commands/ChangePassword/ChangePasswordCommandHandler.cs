namespace SistemaDigitalizacionPolizas.Application.Services.PasswordReset_Services.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler
    : IRequestHandler<ChangePasswordCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordResetRepository _resetRepository;

        public ChangePasswordCommandHandler(
            IUserRepository userRepository,
            IPasswordResetRepository resetRepository)
        {
            _userRepository = userRepository;
            _resetRepository = resetRepository;
        }

        public async Task<bool> Handle(
            ChangePasswordCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Data;

            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null)
                return false;

            var token = await _resetRepository
                .GetValidTokenAsync(user.IdUser, dto.Code);

            if (token == null)
                return false;

            // 🔐 Hash seguro
            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            await _userRepository.UpdatePasswordAsync(
                user.IdUser,
                user.Password
            );

            // Invalidar token
            token.Used = true;
            await _resetRepository.InvalidateAsync(token.IdToken);

            return true;
        }
    }


}
