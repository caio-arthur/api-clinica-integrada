using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.DTOs
{
    public class PacienteDTO : IMapFrom<Paciente>
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public int Idade { get; set; }
        public string? NomeResponsavel { get; set; }
        public string? ParentescoResponsavel { get; set; }
        public string? Observacao { get; set; }
        public bool RecebeuAlta { get; set; }


        public void Mapping(Profile profile) {
            profile.CreateMap<Paciente, PacienteDTO>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Nome, opt => opt.MapFrom(s => s.Nome))
                .ForMember(d => d.Telefone, opt => opt.MapFrom(s => s.Telefone))
                .ForMember(d => d.Idade, opt => opt.MapFrom(s => s.Idade))
                .ForMember(d => d.NomeResponsavel, opt => opt.MapFrom(s => s.NomeResponsavel))
                .ForMember(d => d.ParentescoResponsavel, opt => opt.MapFrom(s => s.ParentescoResponsavel))
                .ForMember(d => d.Observacao, opt => opt.MapFrom(s => s.Observacao))
                .ForMember(d => d.RecebeuAlta, opt => opt.MapFrom(s => s.RecebeuAlta))
                ;
        }
    }
}
