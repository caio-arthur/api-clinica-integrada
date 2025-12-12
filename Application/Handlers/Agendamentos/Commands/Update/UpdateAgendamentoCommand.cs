using Application.DTOs;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Handlers.Agendamentos.Commands.Update
{
    public class UpdateAgendamentoCommand : AgendamentoCommand, IRequest<ServiceResult<AgendamentoDTO>>
    {
        [JsonIgnore]
        public Guid Id { get; set; }
    }

    public class UpdateAgendamentoCommandHandler : IRequestHandler<UpdateAgendamentoCommand, ServiceResult<AgendamentoDTO>>
    {
        private readonly ISender _mediator;
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateAgendamentoCommandHandler(IApplicationDbContext context,
            IMapper mapper,
            ISender mediator
            ) {
            _mediator = mediator;
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResult<AgendamentoDTO>> Handle(UpdateAgendamentoCommand request, CancellationToken cancellationToken) {
            try {
                var entidadeAlterado = await _context.Agendamentos.FindAsync(request.Id);
                //var entidadeOriginal = (Agendamento)entidadeAlterado.Clone();

                if (entidadeAlterado == null) {
                    throw new Exception(nameof(Agendamento));
                }

                entidadeAlterado.DataHoraInicio = request.DataHoraInicio;
                entidadeAlterado.DataHoraFim = request.DataHoraFim;
                entidadeAlterado.Tipo = request.Tipo;
                entidadeAlterado.Status = request.Status;
                entidadeAlterado.PacienteId = request.PacienteId;
                entidadeAlterado.NomeAluno = request.NomeAluno;
                entidadeAlterado.NomeEquipe = request.NomeEquipe;
                entidadeAlterado.SalaId = request.SalaId;
                entidadeAlterado.ConsultaId = request.ConsultaId;

                //var historico = entidadeOriginal.GerarHistoricoDiferenca(entidadeAlterado, entidadeAlterado.Id, _currentUserService.UserId);
                //await _context.Historicos.AddAsync(historico, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<AgendamentoDTO>(entidadeAlterado);

                return ServiceResult.Success(result);
            } catch (Exception ex) {
                throw;
            }
        }
    }

}
