using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ListaEsperaConfiguration : IEntityTypeConfiguration<ListaEspera>
    {
        public void Configure(EntityTypeBuilder<ListaEspera> builder) {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.PacienteId).IsRequired();
            builder.Property(p => p.DataEntrada).IsRequired();
                

        }
    }
}
