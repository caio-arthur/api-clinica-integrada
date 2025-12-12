using Application.DTOs;
using Application.Handlers.Equipes.Queries.GetEquipeById;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Profissionais.Commands.Delete
{
    public class DeleteProfissionalCommand : IRequestWrapper<string>
    {
        public Guid Id { get; set; }
    }
    
    public class DeleteProfissionalCommandHandler : IRequestHandlerWrapper<DeleteProfissionalCommand, string>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDateTime _dateTime;

        public DeleteProfissionalCommandHandler(IApplicationDbContext context,
                                                IMapper mapper,
                                                IDateTime dateTime
                                                ) {
            _context = context;
            _mapper = mapper;
            _dateTime = dateTime;
        }

        public async Task<ServiceResult<string>> Handle(DeleteProfissionalCommand request, CancellationToken cancellationToken) {
            try {
                var entity = await _context.Profissionais
                    .Where(p => !p.IsDeleted)
                    .FirstOrDefaultAsync(p => p.Id == request.Id);

                if (entity == null) {
                    throw new Exception("Profissional não encontrado");
                }

                entity.ExcludedAt = _dateTime.Now;
                entity.IsDeleted = true;
                _context.Profissionais.Update(entity);

                await _context.SaveChangesAsync(cancellationToken);

                return ServiceResult.Success("Ok");
            } catch (Exception e) {
                throw;
            }
        }
    }
}
