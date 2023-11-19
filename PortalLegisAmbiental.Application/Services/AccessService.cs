using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PortalLegisAmbiental.Application.Services.Interfaces;
using PortalLegisAmbiental.Domain.Dtos;
using PortalLegisAmbiental.Domain.Dtos.Requests;
using PortalLegisAmbiental.Domain.Dtos.Responses;
using PortalLegisAmbiental.Domain.IRepositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using BC = BCrypt.Net.BCrypt;

namespace PortalLegisAmbiental.Application.Services
{
    public class AccessService : IAccessService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfigurationRoot _configuration;
        private readonly IMapper _mapper;

        public AccessService(IUsuarioRepository usuarioRepository, IMapper mapper, IConfigurationRoot configuration)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<AccessTokenDto> AuthenticateUser(LoginRequest userLogin)
        {
            var user = await _usuarioRepository.GetByEmail(userLogin.Email, includeGroups: true);
            if (user == null)
                throw new UnauthorizedAccessException("Credenciais inválidas.");

            bool verified = BC.Verify(userLogin.Senha, user.Senha);
            if (!verified)
                throw new UnauthorizedAccessException("Credenciais inválidas.");

            var userData = _mapper.Map<UsuarioResponse>(user);
            var accessToken = GenerateToken(userData);

            return new AccessTokenDto { AccessToken = accessToken, };
        }

        public UsuarioResponse GetLoggedUser(ClaimsPrincipal user)
        {
            var jsonData = user.Identity?.Name;
            if (jsonData == null)
                throw new UnauthorizedAccessException("Token inválido.");

            var userData = Newtonsoft.Json.JsonConvert.DeserializeObject<UsuarioResponse>(jsonData);
            if (userData == null)
                throw new UnauthorizedAccessException("Usuário não encontrado");

            return userData;

        }

        private string GenerateToken(UsuarioResponse user)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            var name = JsonSerializer.Serialize(user, jsonOptions);
            var grupos = JsonSerializer.Serialize(user.Grupos, jsonOptions);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWT:SecretKey"));

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, grupos)
            };

            var expiresStr = _configuration.GetValue<string>("JWT:TokenValidityInHours") ?? "24";
            _ = int.TryParse(expiresStr, out int tokenExpiresInHours);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(authClaims),
                Expires = DateTime.UtcNow.AddHours(tokenExpiresInHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
