using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.DTOs
{
    public class ListUsuarioDTO : IMapFrom<Usuario>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public IList<string>? Roles { get; set; }

        public void Mapping(Profile profile) {
            profile.CreateMap<Usuario, ListUsuarioDTO>()
                 .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                 .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                 .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Email))
                 .ForMember(d => d.Telefone, opt => opt.MapFrom(s => s.PhoneNumber))
                 .ForMember(d => d.Roles, opt => opt.MapFrom(s => s.Roles));
        }
    }
}
