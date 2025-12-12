namespace Domain.Entities
{
    public class EquipeProfissional
    {
        public Guid EquipeId { get; set; }
        public Equipe Equipe { get; set; }
        public Guid ProfissionalId { get; set; }
        public Profissional Profissional { get; set; }
        
    }
}
