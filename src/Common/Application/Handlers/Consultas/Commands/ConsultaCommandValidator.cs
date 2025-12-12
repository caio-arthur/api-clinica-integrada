using Application.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.Consultas.Commands
{
    public class ConsultaCommandValidator<T> : AbstractValidator<T> where T : ConsultaCommand
    {
        protected readonly IApplicationDbContext _context;
        public ConsultaCommandValidator(IApplicationDbContext context) {
            _context = context;

            RuleFor(v => v.DataHoraInicio)
                .NotEmpty().WithMessage("DataHoraInicio é obrigatório.");
            RuleFor(v => v.Especialidade)
                .NotEmpty().WithMessage("Especialidade é obrigatório.");
            //RuleFor(v => v.Status)
            //    .NotEmpty().WithMessage("Status é obrigatório.");
            //RuleFor(v => v.AgendamentoId)
            //    .NotEmpty().WithMessage("AgendamentoId é obrigatório.");
            RuleFor(v => v.EquipeId)
                .NotEmpty().WithMessage("EquipeId é obrigatório.");
        }
    }
    
}
