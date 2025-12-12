using Domain.Enums;

namespace Application.Handlers.Consultas.Commands
{
    public class ConsultaCommand
    {
        public string Observacao { get; set; }
        public DateTime DataHoraInicio { get; set; }
        public DateTime? DataHoraFim { get; set; }
        public Especialidade Especialidade { get; set; }
        public ConsultaStatus Status { get; set; }
        public Guid AgendamentoId { get; set; }
        public Guid EquipeId { get; set; }

    }
}
