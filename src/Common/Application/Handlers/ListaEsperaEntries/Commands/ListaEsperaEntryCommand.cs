using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Handlers.ListaEsperaEntries.Commands
{
    public class ListaEsperaEntryCommand
    {
        public DateTime DataEntrada { get; set; }
        public DateTime? DataSaida { get; set; }
        public ListaStatus Status { get; set; }
        public Especialidade Especialidade { get; set; }
        public Prioridade Prioridade { get; set; }
        public Guid PacienteId { get; set; }

    }
}
