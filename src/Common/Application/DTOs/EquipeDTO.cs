using Application.Mappings;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class EquipeDTO : IMapFrom<Equipe>

    {

        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Especialidade { get; set; }
        public IList<ProfissionalDTO> Estagiarios { get; set; }
        public IList<ProfissionalDTO> Professores { get; set; }

        public void Mapping(MappingProfile profile) {
            profile.CreateMap<Equipe, EquipeDTO>()

                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Nome, opt => opt.MapFrom(s => s.Nome))
                .ForMember(d => d.Especialidade, opt => opt.MapFrom(s => s.Especialidade))
                .ForMember(d => d.Estagiarios, opt => opt.MapFrom(s => s.Profissionais.Where(ep => ep.Profissional.Tipo == TipoProfissional.Estagiario).Select(ep => ep.Profissional)))
                .ForMember(d => d.Professores, opt => opt.MapFrom(s => s.Profissionais.Where(ep => ep.Profissional.Tipo == TipoProfissional.Professor).Select(ep => ep.Profissional)))
                ;
        }
    }    

}
