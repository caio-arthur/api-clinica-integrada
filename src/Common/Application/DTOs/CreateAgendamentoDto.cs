using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateAgendamentoDto
    {
        public Guid AgendamentoId { get; set; }
        public Guid ConsultaId { get; set; }
    }
}
