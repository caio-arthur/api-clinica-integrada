using Domain.Entities;
using Infrastructure.Identity.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;


namespace Infrastructure.Identity.Services
{
    public class UsuarioLogado : IUsuarioLogado
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly UserManager<Usuario> _userManager;

        public UsuarioLogado(IHttpContextAccessor accessor, UserManager<Usuario> userManager) {
            _accessor = accessor;
            _userManager = userManager;
        }


        public string NomeUsuario => _accessor.HttpContext.User.Identity.Name;
        public Guid Id => _userManager.FindByNameAsync(NomeUsuario).Result.Id;


        public bool IsAuthenticated() {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }

    }
}
