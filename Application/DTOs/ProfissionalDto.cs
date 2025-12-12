
using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.DTOs
{
    public class ProfissionalDTO : IMapFrom<Profissional>
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string? RA { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Tipo { get; set; }
        public string Especialidade { get; set; }

        public void Mapping(Profile profile) {
            profile.CreateMap<Profissional, ProfissionalDTO>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Nome, opt => opt.MapFrom(s => s.Nome))
                .ForMember(d => d.RA, opt => opt.MapFrom(s => s.RA))
                .ForMember(d => d.Telefone, opt => opt.MapFrom(s => s.Telefone))
                .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Email))
                .ForMember(d => d.Tipo, opt => opt.MapFrom(s => s.Tipo.ToString()))
                .ForMember(d => d.Especialidade, opt => opt.MapFrom(s => s.Especialidade.ToString()))
                ;
        }
    }
}
