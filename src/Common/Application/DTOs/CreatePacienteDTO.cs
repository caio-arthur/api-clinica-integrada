using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreatePacienteDTO
    {
        public Guid PacienteId { get; set; }
        public Guid? ListaEsperaId { get; set; }
    }
}
