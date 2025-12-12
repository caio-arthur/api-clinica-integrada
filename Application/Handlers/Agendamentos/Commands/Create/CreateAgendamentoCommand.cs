using Application.DTOs;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Agendamentos.Commands.Create
{
    public class CreateAgendamentoCommand : IRequest<ServiceResult<CreateAgendamentoDto>> {
        public AgendamentoCommand Agendamento { get; set; }
        public ConsultaReducedCommand Consulta { get; set; }
    }

    public class CreateAgendamentoCommandHandler : IRequestHandler<CreateAgendamentoCommand, ServiceResult<CreateAgendamentoDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateAgendamentoCommandHandler(
            IApplicationDbContext context,
            IMapper mapper
            ) {
            _context = context;
            _mapper = mapper;

        }

        public async Task<ServiceResult<CreateAgendamentoDto>> Handle(CreateAgendamentoCommand request, CancellationToken cancellationToken) {
            try {

                await ValidarEntidadesAsync(request.Agendamento.SalaId, request.Consulta.EquipeId, request.Agendamento.PacienteId, cancellationToken);

                if (request.Agendamento.SalaId.HasValue) {
                    await VerificarDisponibilidadeSala(request.Agendamento.SalaId, request.Agendamento.DataHoraInicio, request.Agendamento.DataHoraFim, cancellationToken);
                }

                var agendamentoEntity = new Agendamento {
                    DataHoraInicio = request.Agendamento.DataHoraInicio,
                    DataHoraFim = request.Agendamento.DataHoraFim,
                    Tipo = request.Agendamento.Tipo,
                    Status = request.Agendamento.Status,
                    PacienteId = request.Agendamento.PacienteId,
                    NomeAluno = request.Agendamento.NomeAluno,
                    NomeEquipe = request.Agendamento.NomeEquipe,
                    SalaId = request.Agendamento.SalaId,
                };

                var consultaEntity = new Consulta {
                    Observacao = request.Consulta.Observacao,
                    Especialidade = request.Consulta.Especialidade,
                    Status = ConsultaStatus.Agendada,
                    AgendamentoId = agendamentoEntity.Id,
                    EquipeId = request.Consulta.EquipeId,
                };

                agendamentoEntity.ConsultaId = consultaEntity.Id;

                //Atualizar Status Lista de Espera para Atendido
                if (request.Agendamento.PacienteId.HasValue) {
                    await AtualizarStatusListaEspera(request.Agendamento.PacienteId.Value, cancellationToken);
                    //Atualizar Etapa Paciente para TriagemConsultaAgendada
                    await AtualizarEtapaPaciente(request.Agendamento.PacienteId.Value, cancellationToken);
                }

                await _context.Agendamentos.AddAsync(agendamentoEntity, cancellationToken);
                await _context.Consultas.AddAsync(consultaEntity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                var result = new CreateAgendamentoDto {
                    AgendamentoId = agendamentoEntity.Id,
                    ConsultaId = consultaEntity.Id
                };

                return ServiceResult.Success(result);
            } catch (Exception ex) {
                await _context.RollBack();
                throw;
            }
        }

        private async Task VerificarDisponibilidadeSala(Guid? SalaId, DateTime DataHoraInicio, DateTime DataHoraFim, CancellationToken cancellationToken) {
            
            var sala = await _context.Salas.FindAsync(new object[] { SalaId }, cancellationToken);
            if (sala == null) return; // Should be caught by ValidarEntidadesAsync but safe to have

            // Validar se o novo agendamento tem interseção com algum existente na mesma sala
            var agendamentosCount = await _context.Agendamentos
                .Where(a => a.SalaId == SalaId &&
                            a.DataHoraInicio < DataHoraFim && // Começa antes do término do novo agendamento
                            a.DataHoraFim > DataHoraInicio && // Termina após o início do novo agendamento
                            !a.IsDeleted) // Apenas agendamentos não deletados
                .CountAsync(cancellationToken);

            if (agendamentosCount >= sala.Capacidade) {
                throw new Exception($"Sala atingiu a capacidade máxima de {sala.Capacidade} agendamentos para o horário informado.");
            }
        }


        public async Task<ServiceResult> ValidarEntidadesAsync(
            Guid? salaId,
            Guid? equipeId,
            Guid? pacienteId,
            CancellationToken cancellationToken) {
                if (salaId.HasValue) {
                    var sala = await _context.Salas.FirstOrDefaultAsync(s => s.Id == salaId, cancellationToken);
                    if (sala == null) {
                        return ServiceResult.Failed(ServiceError.CustomMessage("Sala não existe"));
                    }
                }

                if (equipeId.HasValue) {
                    var equipe = await _context.Equipes.FirstOrDefaultAsync(e => e.Id == equipeId, cancellationToken);
                    if (equipe == null) {
                        return ServiceResult.Failed(ServiceError.CustomMessage("Equipe não existe"));
                    }
                }

                if (pacienteId.HasValue) {
                    var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.Id == pacienteId, cancellationToken);
                    if (paciente == null) {
                        return ServiceResult.Failed(ServiceError.CustomMessage("Paciente não existe"));
                    }
                }

            return ServiceResult.Success("Ok");
        }

        private async Task AtualizarStatusListaEspera(Guid pacienteId, CancellationToken cancellationToken) {
            var listaEspera = await _context.ListaEspera
                .Where(x => x.PacienteId == pacienteId && x.Status == ListaStatus.Aguardando)
                .OrderByDescending(x => x.DataEntrada)
                .FirstOrDefaultAsync(cancellationToken);

            if (listaEspera != null) {
                listaEspera.Status = ListaStatus.Atendido;
            }
        }

        private async Task AtualizarEtapaPaciente(Guid pacienteId, CancellationToken cancellationToken) {
            var paciente = await _context.Pacientes
                .Where(p => p.Id == pacienteId)
                .FirstOrDefaultAsync(cancellationToken);
            if (paciente != null) {
                paciente.Etapa = PacienteEtapa.TriagemConsulta;
            }
        }


    }
}

