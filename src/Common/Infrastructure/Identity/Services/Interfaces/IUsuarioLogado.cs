using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Services.Interfaces
{
    public interface IUsuarioLogado
    {
        string NomeUsuario { get; }
        Guid Id { get; }
        bool IsAuthenticated();
    }
}
