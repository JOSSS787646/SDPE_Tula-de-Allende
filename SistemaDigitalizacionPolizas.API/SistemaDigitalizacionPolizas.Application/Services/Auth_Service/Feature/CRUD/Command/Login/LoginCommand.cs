
namespace SistemaDigitalizacionPolizas.Application.Services.Auth_Service.Feature.CRUD.Command.Login
{
    public class LoginCommand : IRequest<LoginResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public LoginCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
