using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class Equipe : AuditableEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Especialidade Especialidade { get; set; }
        public string Nome { get; set; }

        //Relacionamentos
        public IList<EquipeProfissional> Profissionais { get; set; }
        public IList<Consulta> Consultas { get; set; }

    }
}
