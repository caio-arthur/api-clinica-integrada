using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class Agendamento : AuditableEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime DataHoraInicio { get; set; }
        public DateTime? DataHoraFim { get; set; }
        public AgendamentoTipo Tipo { get; set; }
        public AgendamentoStatus Status { get; set; }

        public string? NomeAluno { get; set; }
        public string? NomeEquipe { get; set; }

        //Relacionamentos
        public Guid? PacienteId { get; set; }
        public Paciente? Paciente { get; set; }

        public Guid? SalaId { get; set; }
        public Sala Sala { get; set; }
        public Guid? ConsultaId { get; set; }
        public Consulta Consulta { get; set; }
    }
}
