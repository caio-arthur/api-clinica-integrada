using Domain.Entities;
using Infrastructure.Identity.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity.Services
{
    public class UsuarioService : IUsuarioService
    {

        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;



        public UsuarioService(SignInManager<Usuario> signInManager, UserManager<Usuario> userManager) {
            _signInManager = signInManager;
            _userManager = userManager;
        }


        public async Task<IEnumerable<Usuario>> GetAll() {
            return await _userManager.Users.AsNoTracking().ToListAsync();
        }


        public async Task<Usuario> GetById(Guid usuarioId) {
            return await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == usuarioId);
        }


        public async Task<IList<string>> GetPerfisPorUsuario(Usuario usuario) {
            return await _userManager.GetRolesAsync(usuario);
        }


        //public async Task<bool> InserirUsuarioCliente(string nome, string sobreNome, string email, string telefone, string senha) {
        //    var usuario = new Usuario {
        //        Name = nome,
        //        UserName = email,
        //        NormalizedUserName = email.ToUpper(),
        //        Email = email,
        //        NormalizedEmail = email.ToUpper(),
        //        PhoneNumber = telefone,
        //    };

        //    var resultado = await _userManager.CreateAsync(usuario, senha);

        //    if (resultado.Succeeded) {
        //        List<string> perfis = new()
        //        {
        //            "cliente"
        //        };

        //        await _userManager.AddToRolesAsync(usuario, perfis);
        //    }

        //    return resultado.Succeeded;
        //}


        public async Task<bool> InserirUsuario(string nome, string sobreNome, string email, string telefone, IEnumerable<string> perfis, string senha) {
            var usuario = new Usuario {
                Name = nome,
                UserName = email,
                NormalizedUserName = email.ToUpper(),
                Email = email,
                NormalizedEmail = email.ToUpper(),
                PhoneNumber = telefone,
            };

            var resultado = await _userManager.CreateAsync(usuario, senha);

            if (resultado.Succeeded) {

                await _userManager.AddToRolesAsync(usuario, perfis);
            }

            return resultado.Succeeded;
        }

        public async Task<bool> VerificaUsuarioCadastrado(string nomeUsuario) {
            if (await _userManager.FindByNameAsync(nomeUsuario) != null) {
                return true;
            }

            return false;
        }
    }
}
