using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class PacienteConfiguration : IEntityTypeConfiguration<Paciente>
    {
        public void Configure(EntityTypeBuilder<Paciente> builder) {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Nome).IsRequired();

            builder.HasMany(p => p.RegistrosListaEspera)
                .WithOne(p => p.Paciente)
                .HasForeignKey(p => p.PacienteId);

        }
    }
    
}
