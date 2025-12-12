using Application.DTOs;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Handlers.Consultas.Commands.Update
{
    public class UpdateConsultaCommand : ConsultaCommand, IRequest<ServiceResult<ConsultaDTO>>
    {
        [JsonIgnore]
        public Guid Id { get; set; }
    }

    public class UpdateConsultaCommandHandler : IRequestHandler<UpdateConsultaCommand, ServiceResult<ConsultaDTO>>
    {
        private readonly ISender _mediator;
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateConsultaCommandHandler(IApplicationDbContext context,
            IMapper mapper,
            ISender mediator
            ) {
            _mediator = mediator;
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResult<ConsultaDTO>> Handle(UpdateConsultaCommand request, CancellationToken cancellationToken) {
            try {
                var entidadeAlterado = await _context.Consultas.FindAsync(request.Id);
                //var entidadeOriginal = (Consulta)entidadeAlterado.Clone();

                if (entidadeAlterado == null) {
                    throw new Exception(nameof(Consulta));
                }

                entidadeAlterado.DataHoraInicio = request.DataHoraInicio;
                entidadeAlterado.DataHoraFim = request.DataHoraFim;
                entidadeAlterado.Status = request.Status;
                entidadeAlterado.EquipeId = request.EquipeId;
                entidadeAlterado.Observacao = request.Observacao;
                entidadeAlterado.Especialidade = request.Especialidade;
                entidadeAlterado.Status = request.Status;
                entidadeAlterado.AgendamentoId = request.AgendamentoId;

                //var historico = entidadeOriginal.GerarHistoricoDiferenca(entidadeAlterado, entidadeAlterado.Id, _currentUserService.UserId);
                //await _context.Historicos.AddAsync(historico, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<ConsultaDTO>(entidadeAlterado);

                return ServiceResult.Success(result);
            } catch (Exception ex) {
                throw;
            }
        }
    }

}
