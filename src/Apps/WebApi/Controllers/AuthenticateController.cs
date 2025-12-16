using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Identity.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.ViewModels;

namespace WebApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {

        private readonly IAutenticacaoService _autenticacaoService;
        private readonly IValidator<AutenticacaoViewModel> _validator;
        private readonly IConfiguration _configuration;

        public AuthenticateController(IAutenticacaoService autenticacao, IValidator<AutenticacaoViewModel> validator, IConfiguration configuration) {
            _autenticacaoService = autenticacao;
            _validator = validator;
            _configuration = configuration;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] AutenticacaoViewModel autenticacaoViewModel)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(autenticacaoViewModel);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result = await _autenticacaoService.AutenticarUsuario(autenticacaoViewModel.Email, autenticacaoViewModel.Senha);

            if (result)
            {
                // 1. Buscamos o usuário completo
                var usuario = await _autenticacaoService.GetUsuario(autenticacaoViewModel.Email);

                // 2. Buscamos o perfil (Role)
                // Nota: Usei await aqui para evitar o .Result (bloqueante) que estava no seu código original
                var perfis = await _autenticacaoService.GetPerfilUsuario(autenticacaoViewModel.Email);
                var perfil = perfis.FirstOrDefault();

                // 3. Passamos o objeto 'usuario' para o método GeraToken
                return Ok(GeraToken(autenticacaoViewModel, perfil, usuario));
            }
            else
            {
                return BadRequest("Tentativa login inválida.");
            }
        }

        // Atualize a assinatura para receber o objeto Usuario
        private UsuarioToken GeraToken(AutenticacaoViewModel autenticacaoViewModel, string perfil, Usuario usuario)
        {

            //define declarações do usuário
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, autenticacaoViewModel.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimsIdentity.DefaultNameClaimType, autenticacaoViewModel.Email),
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
