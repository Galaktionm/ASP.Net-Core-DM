namespace UserService.Services
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    public class JWTService
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<User> userManager;

        public JWTService() { }

        public JWTService(
            IConfiguration configuration,
            UserManager<User> userManager
            )
        {
            this.configuration = configuration;
            this.userManager = userManager;
        }

        public async Task<JwtSecurityToken> GetTokenAsync(User user)
        {
            var token = new JwtSecurityToken(
                issuer: configuration["JWTParams:Issuer"],
                audience: configuration["JWTParams:Audience"],
                claims: await GetClaimsAsync(user),
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(configuration["JWTParams:ExpiresIn"])),
                signingCredentials: GetSigningCredentials());
            return token;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(
                configuration["JWTParams:SecurityKey"]);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret,
                SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaimsAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email)
            };

            foreach (var role in await userManager.GetRolesAsync(user))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }
    }
}
