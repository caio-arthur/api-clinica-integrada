using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ProfissionalConfiguration : IEntityTypeConfiguration<Profissional>
    {
        public void Configure(EntityTypeBuilder<Profissional> builder) {
            builder.HasKey(p => p.Id); 
 
            builder.Property(p => p.Nome)
                .IsRequired();



            builder.Property(p => p.Tipo)
                .IsRequired(); 
            builder.Property(p => p.Especialidade)
                .IsRequired(); 

            // Configurar relacionamento muitos-para-muitos
            builder.HasMany(p => p.EquipesProfissional) 
                .WithOne(ep => ep.Profissional) 
                .HasForeignKey(ep => ep.ProfissionalId); 
        
        }
    }
}
