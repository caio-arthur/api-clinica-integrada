using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class Sala : AuditableEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; }
        public Especialidade Especialidade { get; set; }
        public bool IsDisponivel { get; set; }
        public int Capacidade { get; set; } = 1;

        //Relacionamentos
        public IList<Agendamento> Reservas { get; set; }

    }
}
