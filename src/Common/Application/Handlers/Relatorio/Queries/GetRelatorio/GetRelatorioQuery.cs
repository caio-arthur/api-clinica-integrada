using Application.DTOs;
using Application.Interfaces;
using Application.Models;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Relatorio.Queries.GetRelatorio
{
    public class GetRelatorioQuery : IRequestWrapper<RelatorioDTO>
    {

    }

    public class GetRelatorioQueryHandler : IRequestHandlerWrapper<GetRelatorioQuery, RelatorioDTO>
    {
        private readonly IApplicationDbContext _context;
        public GetRelatorioQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ServiceResult<RelatorioDTO>> Handle(GetRelatorioQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var relatorio = new RelatorioDTO
                {
                    PacientesCadastrados = await _context.Pacientes
                        .Where(p => !p.IsDeleted)
                        .CountAsync(cancellationToken),
                    PacientesCadastradosEsteMes = await _context.Pacientes
                        .Where(p => p.Created.Month == DateTime.Now.Month &&
                                    p.Created.Year == DateTime.Now.Year &&
                                    !p.IsDeleted                                    )
                        .CountAsync(cancellationToken),
                    AgendamentosRealizados = await _context.Agendamentos
                        .Where(a => !a.IsDeleted)
                        .CountAsync(cancellationToken),
                    AgendamentosRealizadosEsteMes = await _context.Agendamentos
                        .Where(a => a.Created.Month == DateTime.Now.Month && a.Created.Year == DateTime.Now.Year && !a.IsDeleted)
                        .CountAsync(cancellationToken),
                    ConsultasConcluidas = await _context.Consultas
                        .Where(c => c.Status == Domain.Enums.ConsultaStatus.Concluida && !c.IsDeleted)
                        .CountAsync(cancellationToken),
                    ConsultasConcluidasEsteMes = await _context.Consultas
                        .Where(c =>
                        !c.IsDeleted &&
                        c.Status == Domain.Enums.ConsultaStatus.Concluida &&
                        c.LastModified.HasValue &&
                        c.LastModified.Value.Month == DateTime.Now.Month &&
                        c.LastModified.Value.Year == DateTime.Now.Year
                    )
                    .CountAsync(cancellationToken),
                    EstagiariosCadastrados = await _context.Profissionais
                        .Where(p => !p.IsDeleted && p.Tipo == TipoProfissional.Estagiario)
                        .CountAsync(cancellationToken),
                    ProfessoresCadastrados = await _context.Profissionais
                        .Where(p => !p.IsDeleted && p.Tipo == TipoProfissional.Professor)
                        .CountAsync(cancellationToken),
                    EquipesCadastradas = await _context.Equipes
                        .Where(e => !e.IsDeleted)
                        .CountAsync(cancellationToken)
                };
                return ServiceResult.Success(relatorio);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failed<RelatorioDTO>(new ServiceError($"Erro ao gerar relatório.", 500));
            }
        }
    }
}
