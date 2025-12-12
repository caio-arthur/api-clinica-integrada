using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Handlers.Equipes.Commands.Create
{
    public class CreateEquipeCommand : IRequest<ServiceResult>
    {
        public string Nome { get; set; }
        public Especialidade Especialidade { get; set; }
        public IList<Guid> Estagiarios { get; set; }
        public IList<Guid> Professores { get; set; }
    }

    public class CreateEquipeCommandHandler : IRequestHandler<CreateEquipeCommand, ServiceResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDateTime _dateTime;

        public CreateEquipeCommandHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IDateTime dateTime
            ) {
            _context = context;
            _mapper = mapper;
            _dateTime = dateTime;

        }


        public async Task<ServiceResult> Handle(CreateEquipeCommand request, CancellationToken cancellationToken) {
            var equipe = new Equipe {
                Id = Guid.NewGuid(),
                Nome = request.Nome,
                Especialidade = request.Especialidade, // Defina a especialidade conforme necessário
                Profissionais = new List<EquipeProfissional>()
            };

            if (request.Estagiarios != null && request.Estagiarios.Any())
            {
                foreach (var estagiarioId in request.Estagiarios)
                {
                    equipe.Profissionais.Add(new EquipeProfissional
                    {
                        EquipeId = equipe.Id,
                        ProfissionalId = estagiarioId
                    });
                }
            }

            if (request.Professores != null && request.Professores.Any())
            {
                foreach (var professorId in request.Professores)
                {
                    equipe.Profissionais.Add(new EquipeProfissional
                    {
                        EquipeId = equipe.Id,
                        ProfissionalId = professorId
                    });
                }
            }

            _context.Equipes.Add(equipe);
            _context.EquipeProfissional.AddRange(equipe.Profissionais);
            await _context.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success(equipe.Id);
        }
    }
}
