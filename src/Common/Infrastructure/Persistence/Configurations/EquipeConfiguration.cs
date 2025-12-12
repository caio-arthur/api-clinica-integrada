using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class EquipeProfissionalConfiguration : IEntityTypeConfiguration<Equipe>
    {
        public void Configure(EntityTypeBuilder<Equipe> builder) {
            builder.HasKey(e => e.Id); ; 

            builder.Property(e => e.Especialidade)
                .IsRequired();

            builder.HasMany(e => e.Profissionais)
                .WithOne(ep => ep.Equipe)
                .HasForeignKey(ep => ep.EquipeId);
        }
    }
}
