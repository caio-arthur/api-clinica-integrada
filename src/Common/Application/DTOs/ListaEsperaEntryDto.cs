using Application.Mappings;
using Domain.Entities;
using Domain.Enums;

namespace Application.DTOs
{
    public class ListaEsperaEntryDTO : IMapFrom<ListaEspera>
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataEntrada { get; set; }
        public DateTime? DataSaida { get; set; }
        public string Status { get; set; }
        public ListaStatus StatusInt { get; set; }
        public string Prioridade { get; set; }
        public Prioridade PrioridadeInt { get; set; }
        public string Especialidade { get; set; }
        public Prioridade EspecialidadeInt { get; set; }
        public Guid PacienteId { get; set; }

        public void Mapping(AutoMapper.Profile profile) {
            profile.CreateMap<ListaEspera, ListaEsperaEntryDTO>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.DataEntrada, opt => opt.MapFrom(s => s.DataEntrada))
                .ForMember(d => d.DataSaida, opt => opt.MapFrom(s => s.DataSaida))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status))
                .ForMember(d => d.StatusInt, opt => opt.MapFrom(s => s.Status))
                .ForMember(d => d.Prioridade, opt => opt.MapFrom(s => s.Prioridade))
                .ForMember(d => d.Nome, Nome => Nome.MapFrom(s => s.Paciente.Nome))
                .ForMember(d => d.PacienteId, opt => opt.MapFrom(s => s.PacienteId))
                .ForMember(d => d.Especialidade, opt => opt.MapFrom(s => s.Especialidade.ToString()))
                .ForMember(d => d.EspecialidadeInt, opt => opt.MapFrom(s => s.Especialidade))
                .ForMember(d => d.PrioridadeInt, opt => opt.MapFrom(s => s.Prioridade))
                ;
        }
    }
}
