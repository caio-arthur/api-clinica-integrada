using Domain.Entities;

namespace Infrastructure.Identity.Services.Interfaces
{
    public interface IUsuarioService
    {
        //Task<bool> InserirUsuarioCliente(string nome, string sobreNome, string email, string telefone, string senha);
        Task<bool> InserirUsuario(string nome, string sobreNome, string email, string telefone, IEnumerable<string> perfis, string senha);
        Task<bool> VerificaUsuarioCadastrado(string nomeUsuario);

        Task<IEnumerable<Usuario>> GetAll();
        Task<Usuario> GetById(Guid usuarioId);
        Task<IList<string>> GetPerfisPorUsuario(Usuario usuario);
    }
}
