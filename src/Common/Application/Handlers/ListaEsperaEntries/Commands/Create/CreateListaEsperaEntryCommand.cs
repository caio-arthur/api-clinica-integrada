using Application.DTOs;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.ListaEsperaEntries.Commands.Create
{
    public class CreateListaEsperaEntryCommand : ListaEsperaEntryCommand, IRequest<ServiceResult>
    {
    }

    public class CreateListaEsperaEntryCommandHandler : IRequestHandler<CreateListaEsperaEntryCommand, ServiceResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateListaEsperaEntryCommandHandler(IApplicationDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResult> Handle(CreateListaEsperaEntryCommand request, CancellationToken cancellationToken) {
            var paciente = await _context.Pacientes.FindAsync(request.PacienteId);
            if (paciente == null) {
                throw new Exception("Paciente não encontrado.");
            }

            var entity = new Domain.Entities.ListaEspera {
                DataEntrada = request.DataEntrada,
                DataSaida = request.DataSaida,
                Status = request.Status,
                Prioridade = request.Prioridade,
                PacienteId = request.PacienteId,
                Especialidade = request.Especialidade
            };

            await ValidarListaEsperaAsync(request.PacienteId, request.Especialidade, cancellationToken);

            paciente.Etapa = PacienteEtapa.ListaEspera;

            await _context.ListaEspera.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var result = _mapper.Map<ListaEsperaEntryDTO>(entity);

            return ServiceResult.Success(result.Id);
        }

        private async Task ValidarListaEsperaAsync(Guid id, Especialidade especialidade, CancellationToken cancellationToken) {
            var listaEspera = await _context.ListaEspera.FirstOrDefaultAsync(
                x => x.PacienteId == id &&
                x.Especialidade == especialidade &&
                x.Status == ListaStatus.Aguardando
                , cancellationToken);
            if (listaEspera != null) {
                throw new Exception($"Paciente tem um registro ativo na lista de espera de {especialidade}.");
            }
        }
    }

}
