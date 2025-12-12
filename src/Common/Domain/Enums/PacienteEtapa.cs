using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum PacienteEtapa
    {
        Cadastrado = 1,
        ListaEspera = 2, // Etapa 1
        TriagemConsulta = 3,
        ConsultaConcluida = 4,
        ConsultaCancelada = 5
    }
}
