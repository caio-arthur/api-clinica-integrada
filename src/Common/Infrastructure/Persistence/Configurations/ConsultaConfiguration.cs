using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ConsultaConfiguration : IEntityTypeConfiguration<Consulta>
    {
        public void Configure(EntityTypeBuilder<Consulta> builder) {
            builder.HasKey(p => p.Id);
            //builder.Property(p => p.DataHoraInicio).IsRequired();
            builder.Property(p => p.Status).IsRequired();

            builder.HasOne(b => b.Agendamento)
                .WithOne(a => a.Consulta)
                .HasForeignKey<Consulta>(b => b.AgendamentoId);

            builder.HasOne(p => p.Equipe)
                .WithMany(p => p.Consultas)
                .HasForeignKey(p => p.EquipeId);


        }
    }
}
