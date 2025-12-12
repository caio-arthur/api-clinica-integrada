using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class AgendamentoConfiguration : IEntityTypeConfiguration<Agendamento>
    {
        public void Configure(EntityTypeBuilder<Agendamento> builder) {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.DataHoraInicio).IsRequired();
            builder.Property(p => p.Status).IsRequired();
            builder.Property(p => p.Tipo).IsRequired();

            builder.Property(p => p.NomeAluno).HasMaxLength(200);
            builder.Property(p => p.NomeEquipe).HasMaxLength(200);

            builder.HasOne(p => p.Paciente)
                .WithMany(p => p.Agendamentos)
                .HasForeignKey(p => p.PacienteId)
                .IsRequired(false);

            builder.HasOne(a => a.Consulta)
                .WithOne(b => b.Agendamento)
                .HasForeignKey<Agendamento>(a => a.ConsultaId);


        }
    }
}
