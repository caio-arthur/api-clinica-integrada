using Application.Interfaces;
using Application.Models;

namespace Application.Handlers.Usuarios.Commands.CreateUsuario
{
    public class CreateUsuarioAtendenteCommand : IRequestWrapper<bool>
    {
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
    }

    public class CreateUsuarioAtendenteCommandHandler : IRequestHandlerWrapper<CreateUsuarioAtendenteCommand, bool>
    {
        private readonly IUsuarioService _usuarioService;
        public CreateUsuarioAtendenteCommandHandler(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }
        public async Task<ServiceResult<bool>> Handle(CreateUsuarioAtendenteCommand request, CancellationToken cancellationToken)
        {
            var succeeded = await _usuarioService.InserirUsuario(request.Nome, request.Email, request.Telefone, ["atendente"], request.Senha);
            if (!succeeded)
            {
                throw new ApplicationException("Erro ao criar usuário atendente.");
            }

            return ServiceResult.Success(true);
        }
    }
}
