using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

public class EquipeProfissionalConfiguration : IEntityTypeConfiguration<EquipeProfissional>
{
    public void Configure(EntityTypeBuilder<EquipeProfissional> builder) {
        builder.HasKey(ep => new { ep.EquipeId, ep.ProfissionalId });

        builder
            .HasOne(ep => ep.Equipe)
            .WithMany(e => e.Profissionais)
            .HasForeignKey(ep => ep.EquipeId);

        builder
            .HasOne(ep => ep.Profissional)
            .WithMany(p => p.EquipesProfissional)
            .HasForeignKey(ep => ep.ProfissionalId);
    }
}
