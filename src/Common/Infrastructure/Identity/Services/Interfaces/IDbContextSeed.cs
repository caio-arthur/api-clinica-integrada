using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Services.Interfaces
{
    public interface IDbContextSeed
    {
        void GerarUsuarios();
        void GerarPerfis();
        public Task GerarProfissionaisEEquipesAsync();
        public Task GerarSalasAsync();
    }
}
