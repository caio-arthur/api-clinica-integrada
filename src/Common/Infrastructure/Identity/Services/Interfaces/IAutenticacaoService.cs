using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Services.Interfaces
{
    public interface IAutenticacaoService
    {
        Task<bool> AutenticarUsuario(string userName, string senha);
        Task Logout();
        Task<IList<string>> GetPerfilUsuario(string nomeUsuario);
    }
}
