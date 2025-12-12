using Application.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ListProfissionalDTO : IMapFrom<Profissional>
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }

        public void Mapping(MappingProfile profile) {
            profile.CreateMap<Profissional, ListProfissionalDTO>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Nome, opt => opt.MapFrom(s => s.Nome))
                ;
        }
    }
}
