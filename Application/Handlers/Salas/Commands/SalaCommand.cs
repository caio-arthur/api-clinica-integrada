using Domain.Enums;

namespace Application.Handlers.Salas.Commands
{
    public class SalaCommand
    {
        public string Nome { get; set; }
        public Especialidade Especialidade { get; set; }
        public int Capacidade { get; set; }
        //public bool IsDisponivel { get; set; }
    }
}
