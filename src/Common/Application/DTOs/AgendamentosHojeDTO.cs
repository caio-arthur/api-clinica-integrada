using Application.Mappings;
using Domain.Entities;

namespace Application.DTOs
{
    public class AgendamentosHojeDTO : IMapFrom<Agendamento> //Deve receber informações de paciente e sala
    {
        public Guid Id { get; set; }
        public DateTime DataHora { get; set; }
        public string Tipo { get; set; }
        public string Status { get; set; }
        public string? Paciente { get; set; }
        public Guid? PacienteId { get; set; }
        public string? Sala { get; set; }
        public Guid? SalaId { get; set; }
        public Guid? ConsultaId { get; set; }

        public void Mapping(MappingProfile profile) {
            profile.CreateMap<Agendamento, AgendamentosHojeDTO>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.DataHora, opt => opt.MapFrom(s => s.DataHoraInicio))
                .ForMember(d => d.Tipo, opt => opt.MapFrom(s => s.Tipo))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status))
                .ForMember(d => d.PacienteId, opt => opt.MapFrom(s => s.PacienteId))
                .ForMember(d => d.Sala, opt => opt.MapFrom(s => s.Sala.Nome))
                .ForMember(d => d.Paciente, opt => opt.MapFrom(s => s.Paciente.Nome))
                .ForMember(d => d.SalaId, opt => opt.MapFrom(s => s.SalaId))
                .ForMember(d => d.ConsultaId, opt => opt.MapFrom(s => s.ConsultaId));
        }
    }
}
