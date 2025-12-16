using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Handlers.Usuarios.Commands
{
    public class LoginCommand : IRequestWrapper<UsuarioToken>
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }

    public class LoginCommandHandler : IRequestHandlerWrapper<LoginCommand, UsuarioToken>
    {
        private readonly IAutenticacaoService _autenticacaoService;
        private readonly IConfiguration _configuration;

        public LoginCommandHandler(IAutenticacaoService autenticacaoService, IConfiguration configuration)
        {
            _autenticacaoService = autenticacaoService;
            _configuration = configuration;
        }

        public async Task<ServiceResult<UsuarioToken>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var isAuthenticated = await _autenticacaoService.AutenticarUsuario(request.Email, request.Senha);

            if (!isAuthenticated)
            {
                return ServiceResult.Failed<UsuarioToken>(ServiceError.InccrrectUsernameOrPassword);
            }

            // 1. Buscamos o usuário completo
            var usuario = await _autenticacaoService.GetUsuario(request.Email);

            // 2. Buscamos o perfil (Role)
            var perfis = await _autenticacaoService.GetPerfilUsuario(request.Email);
            var perfil = perfis.FirstOrDefault();

            // 3. Geramos o token
            var token = GeraToken(request.Email, perfil, usuario);

            return ServiceResult.Success(token);
        }

        private UsuarioToken GeraToken(string email, string perfil, Usuario usuario)
        {
            //define declarações do usuário
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimsIdentity.DefaultNameClaimType, email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, perfil ?? string.Empty),
                new Claim("usuario_nome", usuario.Name ?? "")
            };

            //gera uma chave com base em um algoritmo simetrico
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));

            //gera a assinatura digital do token
            var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //Tempo de expiracão do token.
            var expiracao = _configuration["TokenConfiguration:ExpireHours"];
            var dataExpiracao = DateTime.UtcNow.AddHours(double.Parse(expiracao));

            // classe que representa um token JWT e gera o token
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["TokenConfiguration:Issuer"],
                audience: _configuration["TokenConfiguration:Audience"],
                claims: claims,
                expires: dataExpiracao,
                signingCredentials: credenciais);

            //retorna os dados com o token e informacoes
            return new UsuarioToken()
            {
                Autenticado = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                DataExpiracao = dataExpiracao,
                Mensagem = "Token JWT OK"
            };
        }
    }
}
