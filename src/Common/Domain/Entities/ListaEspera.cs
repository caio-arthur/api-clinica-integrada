using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ListaEspera : AuditableEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        public DateTime DataEntrada { get; set; }
        public DateTime? DataSaida { get; set; }
        public ListaStatus Status { get; set; }
        public Prioridade Prioridade { get; set; }
        public Especialidade Especialidade { get; set; }
        //Relacionamentos
        public Guid PacienteId { get; set; }
        public Paciente Paciente { get; set; }
    }
}
