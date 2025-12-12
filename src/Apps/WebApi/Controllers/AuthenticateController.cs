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
        public async Task<ActionResult> Login([FromBody] AutenticacaoViewModel autenticacaoViewModel) {
            ValidationResult validationResult = await _validator.ValidateAsync(autenticacaoViewModel);

            if (!validationResult.IsValid) {
                return BadRequest(validationResult.Errors);
            }

            var result = await _autenticacaoService.AutenticarUsuario(autenticacaoViewModel.Email, autenticacaoViewModel.Senha);

            if (result) {
                var perfil = _autenticacaoService.GetPerfilUsuario(autenticacaoViewModel.Email);
                return Ok(GeraToken(autenticacaoViewModel, perfil.Result.ElementAt(0)));
            } else {
                return BadRequest("Tentativa login inválida.");
            }
        }

        private UsuarioToken GeraToken(AutenticacaoViewModel autenticacaoViewModel, string perfil) {
            //define declarações do usuário
            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.UniqueName, autenticacaoViewModel.Email),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 new Claim(ClaimsIdentity.DefaultNameClaimType, autenticacaoViewModel.Email),
                 new Claim(ClaimsIdentity.DefaultRoleClaimType, perfil)
             };

            //gera uma chave com base em um algoritmo simetrico
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
            //gera a assinatura digital do token usando o algoritmo Hmac e a chave privada
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
            return new UsuarioToken() {
                Autenticado = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                DataExpiracao = dataExpiracao,
                Mensagem = "Token JWT OK"
            };
        }


    }
}
