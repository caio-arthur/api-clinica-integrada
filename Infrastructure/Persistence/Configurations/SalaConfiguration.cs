using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class SalaConfiguration : IEntityTypeConfiguration<Sala>
    {
        public void Configure(EntityTypeBuilder<Sala> builder) {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Nome).IsRequired();
            builder.Property(p => p.Especialidade).IsRequired();
            builder.Property(p => p.Capacidade).HasDefaultValue(1);

            builder.HasMany(p => p.Reservas)
                .WithOne(p => p.Sala)
                .HasForeignKey(p => p.SalaId);
        }
    }
}
