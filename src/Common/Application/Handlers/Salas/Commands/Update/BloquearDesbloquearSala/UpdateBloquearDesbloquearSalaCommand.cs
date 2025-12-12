using Application.DTOs;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Handlers.Salas.Commands.Update.BloquearDesbloquearSala
{
    public class UpdateBloquearDesbloquearSalaCommand : IRequest<ServiceResult<SalaDTO>>
    {
        [JsonIgnore]
        public Guid Id { get; set; }
    }

    public class UpdateBloquearDesbloquearSalaCommandHandler : IRequestHandler<UpdateBloquearDesbloquearSalaCommand, ServiceResult<SalaDTO>>
    {
        private readonly ISender _mediator;
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public UpdateBloquearDesbloquearSalaCommandHandler(IApplicationDbContext context,
            IMapper mapper,
            ISender mediator
            ) {
            _mediator = mediator;
            _context = context;
            _mapper = mapper;
        }
        public async Task<ServiceResult<SalaDTO>> Handle(UpdateBloquearDesbloquearSalaCommand request, CancellationToken cancellationToken) {
            try {
                var entidadeAlterado = await _context.Salas.FindAsync(request.Id);
                if (entidadeAlterado == null) {
                    throw new Exception(nameof(Sala));
                }
                entidadeAlterado.IsDisponivel = !entidadeAlterado.IsDisponivel;
                await _context.SaveChangesAsync(cancellationToken);
                var result = _mapper.Map<SalaDTO>(entidadeAlterado);
                return ServiceResult.Success(result);
            } catch (Exception ex) {
                throw;
            }
        }
    }
}
