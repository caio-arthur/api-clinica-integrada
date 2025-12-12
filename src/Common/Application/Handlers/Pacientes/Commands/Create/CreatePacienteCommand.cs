using Application.DTOs;
using Application.Handlers.ListaEsperaEntries.Commands;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Pacientes.Commands.Create
{
    public class CreatePacienteCommand : IRequest<ServiceResult>
    {
        public PacienteCommand Paciente { get; set; }
        public ListaEsperaEntryCommand? ListaEspera { get; set; }
    }

    public class CreatePacienteCommandHandler : IRequestHandler<CreatePacienteCommand, ServiceResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDateTime _dateTime;

        public CreatePacienteCommandHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IDateTime dateTime
            ) {
            _context = context;
            _mapper = mapper;
            _dateTime = dateTime;

        }

        public async Task<ServiceResult> Handle(CreatePacienteCommand request, CancellationToken cancellationToken) {
            try {
                var entity = new Paciente {
                    Nome = request.Paciente.Nome,
                    Telefone = request.Paciente.Telefone,
                    Idade = request.Paciente.Idade,
                    NomeResponsavel = request.Paciente.NomeResponsavel,
                    ParentescoResponsavel = request.Paciente.ParentescoResponsavel,
                    Observacao = request.Paciente.Observacao,
                    RecebeuAlta = request.Paciente.RecebeuAlta,
                    Etapa = PacienteEtapa.Cadastrado
                };

                var result = new CreatePacienteDTO {
                    PacienteId = entity.Id,
                    ListaEsperaId = null
                };

                await _context.Pacientes.AddAsync(entity, cancellationToken);

                if (request.ListaEspera != null) {

                    entity.Etapa = PacienteEtapa.ListaEspera;

                    var listaEsperaEntity = new ListaEspera {
                        DataEntrada = _dateTime.Now,
                        DataSaida = request.ListaEspera.DataSaida,
                        Status = ListaStatus.Aguardando,
                        Prioridade = request.ListaEspera.Prioridade,
                        Especialidade = request.ListaEspera.Especialidade,
                        PacienteId = entity.Id
                    };

                    result.ListaEsperaId = listaEsperaEntity.Id;
                    await _context.ListaEspera.AddAsync(listaEsperaEntity, cancellationToken);
                }

                await _context.SaveChangesAsync(cancellationToken);

                return ServiceResult.Success(result);
            } catch (Exception ex) {
                await _context.RollBack();
                throw;
            }
        }
    }

}
