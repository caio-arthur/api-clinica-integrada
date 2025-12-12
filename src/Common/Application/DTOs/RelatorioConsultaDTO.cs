using Application.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class RelatorioConsultaDTO : IMapFrom<Consulta>
    {
        public Guid Id { get; set; }
        public string Observacao { get; set; }
        public DateTime? DataHoraInicio { get; set; }
        public DateTime? DataHoraFim { get; set; }
        public string Especialidade { get; set; }
        public string Status { get; set; }
        public AgendamentoDTO Agendamento { get; set; }
        public EquipeDTO Equipe { get; set; }
        public SalaDTO? Sala { get; set; }
        public PacienteDTO Paciente { get; set; }


        public void Mapping(MappingProfile profile) {
            profile.CreateMap<Consulta, RelatorioConsultaDTO>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Observacao, opt => opt.MapFrom(s => s.Observacao))
                .ForMember(d => d.DataHoraInicio, opt => opt.MapFrom(s => s.DataHoraInicio))
                .ForMember(d => d.DataHoraFim, opt => opt.MapFrom(s => s.DataHoraFim))
                .ForMember(d => d.Especialidade, opt => opt.MapFrom(s => s.Especialidade))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status))
                .ForMember(d => d.Agendamento, opt => opt.MapFrom(s => s.Agendamento))
                .ForMember(d => d.Equipe, opt => opt.MapFrom(s => s.Equipe))
                .ForMember(d => d.Sala, opt => opt.MapFrom(s => s.Agendamento.Sala))
                .ForMember(d => d.Paciente, opt => opt.MapFrom(s => s.Agendamento.Paciente))
                ;
        }
    }
}
