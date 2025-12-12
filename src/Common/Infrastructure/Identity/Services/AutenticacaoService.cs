using Domain.Entities;
using Infrastructure.Identity.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
namespace Infrastructure.Identity.Services
{
    public class AutenticacaoService : IAutenticacaoService
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;



        public AutenticacaoService(SignInManager<Usuario> signInManager, UserManager<Usuario> userManager) {
            _signInManager = signInManager;
            _userManager = userManager;
        }



        public async Task<bool> AutenticarUsuario(string nomeUsuario, string senha) {
            var resultado = await _signInManager.PasswordSignInAsync(nomeUsuario, senha, isPersistent: false, lockoutOnFailure: false);
            return resultado.Succeeded;
        }

        public async Task<IList<string>> GetPerfilUsuario(string nomeUsuario) {
            var usuario = await _userManager.FindByNameAsync(nomeUsuario);

            return await _userManager.GetRolesAsync(usuario);
        }


        public async Task Logout() {
            await _signInManager.SignOutAsync();
        }

    }
}
