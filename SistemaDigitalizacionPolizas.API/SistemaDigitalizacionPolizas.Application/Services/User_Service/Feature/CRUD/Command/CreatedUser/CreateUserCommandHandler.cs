using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.User_Service.Feature.CRUD.Command.CreatedUser
{
    public class CreateUserCommandHandler
        : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<int> Handle(
            CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            var user = new User
            {
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                IdAdministrativeUnit = request.IdAdministrativeUnit,
                IdRole = request.IdRole,
                Asset = true,
                CreationDate = DateTime.Now
            };

            await _userRepository.AddAsync(user);
            return user.IdUser;
        }
    }
}
