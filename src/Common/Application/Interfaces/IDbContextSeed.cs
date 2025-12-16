namespace Application.Interfaces
{
    public interface IDbContextSeed
    {
        void GerarUsuarios(); // development
        public Task GerarProfissionaisEEquipesAsync(); // development
        public Task GerarSalasAsync(); // development
        void GerarPerfis();
        void GerarAcessoInicial();
    }
}
