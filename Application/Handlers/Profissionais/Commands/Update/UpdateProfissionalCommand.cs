using Application.DTOs;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Handlers.Profissionais.Commands.Update
{
    public class UpdateProfissionalCommand : ProfissionalCommand, IRequest<ServiceResult<ProfissionalDTO>>
    {
        [JsonIgnore]
        public Guid Id { get; set; }
    }

    public class UpdateProfissionalCommandHandler : IRequestHandler<UpdateProfissionalCommand, ServiceResult<ProfissionalDTO>>
    {

        private readonly ISender _mediator;
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateProfissionalCommandHandler(IApplicationDbContext context,
            IMapper mapper,
            ISender mediator
            ) {
            _mediator = mediator;
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResult<ProfissionalDTO>> Handle(UpdateProfissionalCommand request, CancellationToken cancellationToken) {
            try {
                var entidadeAlterado = await _context.Profissionais.FindAsync(request.Id);
                //var entidadeOriginal = (Profissional)entidadeAlterado.Clone();

                if (entidadeAlterado == null) {
                    throw new Exception(nameof(Profissional));
                }

                entidadeAlterado.Nome = request.Nome;
                entidadeAlterado.RA = request.RA;
                entidadeAlterado.Telefone = request.Telefone;
                entidadeAlterado.Email = request.Email;
                entidadeAlterado.Tipo = request.Tipo;
                entidadeAlterado.Especialidade = request.Especialidade;

                //var historico = entidadeOriginal.GerarHistoricoDiferenca(entidadeAlterado, entidadeAlterado.Id, _currentUserService.UserId);
                //await _context.Historicos.AddAsync(historico, cancellationToken);
                
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<ProfissionalDTO>(entidadeAlterado);
                return ServiceResult.Success(result);
            } catch (Exception ex) {
                throw;
            }
        }
    }
}
