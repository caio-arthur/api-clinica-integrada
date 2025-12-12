using Application.DTOs;
using Application.Handlers.Equipes.Queries.GetEquipeById;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Gridify;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Usuarios.Queries.GetUsuarios
{
    public class GetUsuariosQuery : GridifyQuery, IRequestWrapper<PaginatedList<ListUsuarioDTO>>
    {
        public string? Perfil { get; set; }
    }

    public class GetUsuariosQueryHandler : IRequestHandlerWrapper<GetUsuariosQuery, PaginatedList<ListUsuarioDTO>>
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly IMapper _mapper;

        public GetUsuariosQueryHandler(UserManager<Usuario> userManager, IMapper mapper) {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ServiceResult<PaginatedList<ListUsuarioDTO>>> Handle(GetUsuariosQuery request, CancellationToken cancellationToken) {
            var gridifyQueryable = request.Perfil != null ?
                (await GetUsersInRoleQueryableAsync(request.Perfil)).GridifyQueryable(request) :
                _userManager.Users.GridifyQueryable(request);

            var query = gridifyQueryable.Query;
            var result = query.AsNoTracking().ToList();

            foreach (var usuario in result) {
                usuario.Roles = await usuario.GetRolesAsync(_userManager);
            }

            var resultDTO = _mapper.Map<List<ListUsuarioDTO>>(result);

            PaginatedList<ListUsuarioDTO> paginatedList = new PaginatedList<ListUsuarioDTO>(resultDTO, gridifyQueryable.Count, request.Page, request.PageSize);
            return ServiceResult.Success(paginatedList);
        }


        public async Task<IQueryable<Usuario>> GetUsersInRoleQueryableAsync(string role) {
            var usersInRole = await _userManager.GetUsersInRoleAsync(role);
            return usersInRole.AsQueryable();
        }


    }

}
