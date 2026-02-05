using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.User_Service.Feature.CRUD.Command.UpdateUserData
{
    internal class UpdateUserDataCommandHandler
         : IRequestHandler<UpdateUserDataCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserDataCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(
            UpdateUserDataCommand request,
            CancellationToken cancellationToken)
        {
            // 👉 Llamada CORRECTA al repository
            var result = await _userRepository.UpdateUserDataAsync(
                request.IdUser,
                request.Email,
                request.IdRole,
                request.IdAdministrativeUnit
            );

            return result;
        }
    }
}
