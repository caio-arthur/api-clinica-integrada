using Application.DTOs;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Handlers.Salas.Commands.Update
{
    public class UpdateSalaCommand : SalaCommand, IRequest<ServiceResult<SalaDTO>>
    {
        [JsonIgnore]
        public Guid Id { get; set; }
    }

    public class UpdateSalaCommandHandler : IRequestHandler<UpdateSalaCommand, ServiceResult<SalaDTO>>
    {
        private readonly ISender _mediator;
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateSalaCommandHandler(IApplicationDbContext context,
            IMapper mapper,
            ISender mediator
            ) {
            _mediator = mediator;
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResult<SalaDTO>> Handle(UpdateSalaCommand request, CancellationToken cancellationToken) {
            try {
                var entidadeAlterado = await _context.Salas.FindAsync(request.Id);
                //var entidadeOriginal = (Sala)entidadeAlterado.Clone();

                if (entidadeAlterado == null) {
                    throw new Exception(nameof(Sala));
                }

                entidadeAlterado.Nome = request.Nome;
                entidadeAlterado.Especialidade = request.Especialidade;

                //var historico = entidadeOriginal.GerarHistoricoDiferenca(entidadeAlterado, entidadeAlterado.Id, _currentUserService.UserId);
                //await _context.Historicos.AddAsync(historico, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<SalaDTO>(entidadeAlterado);

                return ServiceResult.Success(result);
            } catch (Exception ex) {
                throw;
            }
        }
    }
}
