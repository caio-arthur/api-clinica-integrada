using Application.DTOs;
using Application.Handlers.Equipes.Queries.GetEquipeById;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Pacientes.Commands.Delete
{
    public class DeletePacienteCommand : IRequestWrapper<string>
    {
        public Guid Id { get; set; }
    }

    public class DeletePacienteCommandHandler : IRequestHandlerWrapper<DeletePacienteCommand, string>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper; 
        private readonly IDateTime _dateTime;

        public DeletePacienteCommandHandler(IApplicationDbContext context,
                                            IMapper mapper,
                                            IDateTime dateTime

                                            ) 
        {
            _context = context;
            _mapper = mapper;
            _dateTime = dateTime;

        }

        public async Task<ServiceResult<string>> Handle(DeletePacienteCommand request, CancellationToken cancellationToken) {
            var entity = await _context.Pacientes
                .FirstOrDefaultAsync(p => p.Id == request.Id);

            if (entity == null || entity.IsDeleted) {
                throw new Exception($"Paciente com ID {request.Id} não encontrado ou já excluído.");
            }

            var registrosRelacionados = new {
                ListaEspera = await _context.ListaEspera
                    .Where(p => p.PacienteId == entity.Id)
                    .ToListAsync(),
                Agendamentos = await _context.Agendamentos
                    .Where(p => p.PacienteId == entity.Id)
                    .ToListAsync(),
                Consultas = await _context.Consultas
                    .Include(p => p.Agendamento)
                    .Where(p => p.Agendamento.PacienteId == entity.Id)
                    .ToListAsync()
            };

            MarcarComoExcluido(registrosRelacionados.ListaEspera);
            MarcarComoExcluido(registrosRelacionados.Agendamentos);
            MarcarComoExcluido(registrosRelacionados.Consultas);

            entity.ExcludedAt = _dateTime.Now;
            entity.IsDeleted = true;

            _context.Pacientes.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success($"Paciente com ID {entity.Id} e registros associados foram excluídos com sucesso.");
        }

        private void MarcarComoExcluido<TEntity>(IEnumerable<TEntity> entidades) where TEntity : AuditableEntity {
            foreach (var entidade in entidades) {
                entidade.ExcludedAt = _dateTime.Now;
                entidade.IsDeleted = true;
            }
        }

    }

}
