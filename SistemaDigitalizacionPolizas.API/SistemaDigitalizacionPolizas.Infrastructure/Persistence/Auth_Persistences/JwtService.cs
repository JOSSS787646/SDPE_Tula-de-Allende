namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Auth_Persistences
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _settings;

        public JwtService(IOptions<JwtSettings> options)
        {
            _settings = options.Value;
        }

        public JwtResult GenerateToken(User user)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.RolName)
        };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_settings.Key)
            );

            var creds = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256
            );

            var expires = DateTime.UtcNow.AddHours(_settings.ExpiresInHours);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtResult
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = expires
            };
        }
    }

}
