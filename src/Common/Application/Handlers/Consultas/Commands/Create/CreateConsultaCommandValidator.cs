using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.Consultas.Commands.Create
{
    public class CreateConsultaCommandValidator : ConsultaCommandValidator<CreateConsultaCommand>
    {
        public CreateConsultaCommandValidator(IApplicationDbContext context)
            : base(context) {
        }
    }
    
}
