using Application.Mappings;
using Domain.Entities;

namespace Application.DTOs
{
    public class ConsultaDTO : IMapFrom<Consulta>
    {
        public Guid Id { get; set; }
        public string Observacao { get; set; }
        public DateTime? DataHoraInicio { get; set; }
        public DateTime? DataHoraFim { get; set; }
        public string Especialidade { get; set; }
        public string Status { get; set; }
        public PacienteConsultaDTO Paciente { get; set; }
        public AgendamentoConsultaDTO Agendamento { get; set; }
        public EquipeConsultaDTO Equipe{ get; set; }
        public SalaConsultaDTO Sala { get; set; }

        public static void Mapping(MappingProfile profile) {
            profile.CreateMap<Consulta, ConsultaDTO>()
                .ForMember(d => d.Paciente, opt => opt.MapFrom(s => s.Agendamento.Paciente))
                .ForMember(d => d.Equipe, opt => opt.MapFrom(s => s.Equipe))
                .ForMember(d => d.Agendamento, opt => opt.MapFrom(s => s.Agendamento))
                .ForMember(d => d.Sala, opt => opt.MapFrom(s => s.Agendamento.Sala))
                ;
        }

        public class EquipeConsultaDTO : IMapFrom<Equipe>
        {
            public Guid Id { get; set; }
            public string Nome { get; set; }

            public static void Mapping(MappingProfile profile) {
                profile.CreateMap<Equipe, EquipeConsultaDTO>()
                    ;
            }
        }

        public class AgendamentoConsultaDTO : IMapFrom<Agendamento>
        {
            public Guid Id { get; set; }
            public DateTime DataHoraInicio { get; set; }
            public string Tipo { get; set; }
            public static void Mapping(MappingProfile profile) {
                profile.CreateMap<Agendamento, AgendamentoConsultaDTO>()
                    ;
            }
        }   

        public class PacienteConsultaDTO : IMapFrom<Paciente>
        {
            public Guid Id { get; set; }
            public string Nome { get; set; }
            public static void Mapping(MappingProfile profile) {
                profile.CreateMap<Paciente, PacienteConsultaDTO>()
                    ;
            }
        }

        public class SalaConsultaDTO : IMapFrom<Sala>
        {
            public Guid Id { get; set; }
            public string Nome { get; set; }
            public static void Mapping(MappingProfile profile) {
                profile.CreateMap<Sala, SalaConsultaDTO>()
                    ;
            }
        }

    }
}
