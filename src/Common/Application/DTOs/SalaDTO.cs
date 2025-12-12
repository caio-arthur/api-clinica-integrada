using Application.Mappings;
using Domain.Entities;

namespace Application.DTOs
{
    public class SalaDTO : IMapFrom<Sala>
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Especialidade { get; set; }
        public int Capacidade { get; set; }
        public bool IsDisponivel { get; set; }

        public void Mapping(MappingProfile profile) {
            profile.CreateMap<Sala, SalaDTO>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Nome, opt => opt.MapFrom(s => s.Nome))
                .ForMember(d => d.Especialidade, opt => opt.MapFrom(s => s.Especialidade.ToString()))
                .ForMember(d => d.Capacidade, opt => opt.MapFrom(s => s.Capacidade))
                .ForMember(d => d.IsDisponivel, opt => opt.MapFrom(s => s.IsDisponivel));
        }
    }
}
