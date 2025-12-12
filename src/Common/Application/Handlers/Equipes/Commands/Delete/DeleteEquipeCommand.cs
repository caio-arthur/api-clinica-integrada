using Application.Interfaces;
using Application.Models;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.Equipes.Commands.Delete
{
    public class DeleteEquipeCommand : IRequest<ServiceResult>
    {
        public Guid Id { get; set; }
    }

    public class DeleteEquipeCommandHandler : IRequestHandler<DeleteEquipeCommand, ServiceResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDateTime _dateTime;

        public DeleteEquipeCommandHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IDateTime dateTime
            ) {
            _context = context;
            _mapper = mapper;
            _dateTime = dateTime;

        }

        public async Task<ServiceResult> Handle(DeleteEquipeCommand request, CancellationToken cancellationToken) {
            var entity = _context.Equipes.FirstOrDefault(e => e.Id == request.Id);
            if (entity == null) {
                return ServiceResult.Failed(ServiceError.CustomMessage("Equipe não encontrada"));
            }

            entity.ExcludedAt = _dateTime.Now;
            entity.IsDeleted = true;
            _context.Equipes.Update(entity);

            //_context.Equipes.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success("Ok");
        }
    }



}
