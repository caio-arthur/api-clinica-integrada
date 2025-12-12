using Application.Mappings;
using Domain.Entities;
using Domain.Enums;

namespace Application.DTOs
{
    public class ConsultaDTO : IMapFrom<Consulta>
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Observacao { get; set; }
        public DateTime? DataHoraInicio { get; set; }
        public DateTime? DataHoraFim { get; set; }
        public string Especialidade { get; set; }
        public string Status { get; set; }
        public AgendamentoTipo Tipo { get; set; }
        public Guid AgendamentoId { get; set; }
        public Guid EquipeId { get; set; }

        public void Mapping(MappingProfile profile) {
            profile.CreateMap<Consulta, ConsultaDTO>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Nome, opt => opt.MapFrom(s => s.Agendamento.Paciente.Nome))
                .ForMember(d => d.Tipo, opt => opt.MapFrom(s => s.Agendamento.Tipo))
                .ForMember(d => d.Observacao, opt => opt.MapFrom(s => s.Observacao))
                .ForMember(d => d.DataHoraInicio, opt => opt.MapFrom(s => s.DataHoraInicio))
                .ForMember(d => d.DataHoraFim, opt => opt.MapFrom(s => s.DataHoraFim))
                .ForMember(d => d.Especialidade, opt => opt.MapFrom(s => s.Especialidade))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status))
                .ForMember(d => d.AgendamentoId, opt => opt.MapFrom(s => s.AgendamentoId))
                .ForMember(d => d.EquipeId, opt => opt.MapFrom(s => s.EquipeId))
                ;
        }
    }
}
