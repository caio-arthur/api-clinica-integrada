using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class Profissional : AuditableEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; }
        public string? RA { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public TipoProfissional Tipo { get; set; }
        public Especialidade Especialidade { get; set; }

        //Relacionamentos
        public IList<EquipeProfissional> EquipesProfissional { get; set; } 

    }
}
