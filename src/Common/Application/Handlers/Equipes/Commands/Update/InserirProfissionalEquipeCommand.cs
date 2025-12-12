using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.Equipes.Commands.Update
{
    public class InserirProfissionalEquipeCommand : IRequest<ServiceResult>
    {
        public Guid EquipeId { get; set; }
        public Guid ProfissionalId { get; set; }
    }

    public class InserirProfissionalEquipeCommandHandler : IRequestHandler<InserirProfissionalEquipeCommand, ServiceResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDateTime _dateTime;

        public InserirProfissionalEquipeCommandHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IDateTime dateTime
            ) {
            _context = context;
            _mapper = mapper;
            _dateTime = dateTime;

        }

        public async Task<ServiceResult> Handle(InserirProfissionalEquipeCommand request, CancellationToken cancellationToken) {
            var profissional = _context.Profissionais.FirstOrDefault(p => p.Id == request.ProfissionalId);
            if (profissional == null) {
                return ServiceResult.Failed(ServiceError.CustomMessage("Profissional não encontrado"));
            }

            var equipe = _context.Equipes.FirstOrDefault(e => e.Id == request.EquipeId);
            if (equipe == null) {
                return ServiceResult.Failed(ServiceError.CustomMessage("Equipe não encontrada"));
            }

            var entity = new EquipeProfissional {
                EquipeId = request.EquipeId,
                ProfissionalId = request.ProfissionalId
            };

            _context.EquipeProfissional.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success("Ok");
        }
    }
}
