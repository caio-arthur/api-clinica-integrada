using Domain.Enums;

namespace Application.Handlers.Profissionais
{
    public class ProfissionalCommand
    {
        public string Nome { get; set; }
        public string? RA { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public TipoProfissional Tipo { get; set; }
        public Especialidade Especialidade { get; set; }
    }
}
