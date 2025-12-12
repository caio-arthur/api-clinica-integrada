using Domain.Enums;

namespace Application.Handlers.Agendamentos.Commands
{
    public class AgendamentoCommand
    {
        public DateTime DataHoraInicio { get; set; }
        public DateTime DataHoraFim { get; set; }
        public AgendamentoTipo Tipo { get; set; }
        public AgendamentoStatus Status { get; set; }
        public Guid? PacienteId { get; set; }
        public string? NomeAluno { get; set; }
        public string? NomeEquipe { get; set; }
        public Guid? SalaId { get; set; }
        public Guid? ConsultaId { get; set; }
    }
}
