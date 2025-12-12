using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.Agendamentos.Commands
{
    public class ConsultaReducedCommand
    {
        public string Observacao { get; set; }
        public Especialidade Especialidade { get; set; }
        public Guid? EquipeId { get; set; }
    }
}
