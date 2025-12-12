using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Identity.Services.Interfaces;
using Microsoft.AspNetCore.Identity;


namespace Infrastructure.Identity.Services
{
    public class DbContextSeed : IDbContextSeed
    {

        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<Perfil> _roleManager;
        private readonly IApplicationDbContext _context;

        public DbContextSeed(RoleManager<Perfil> roleManager,
              UserManager<Usuario> userManager,
              IApplicationDbContext context) {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        // para especialidade, gerar uma equipe com 2 estagiários e 1 professor
        public async Task GerarProfissionaisEEquipesAsync() {
            Especialidade[] especialidades = { Especialidade.Psicologia, Especialidade.Fisioterapia, Especialidade.Odontologia, Especialidade.Nutricao };

            foreach (var especialidade in especialidades) {
                if (!_context.Equipes.Any(e => e.Especialidade == especialidade)) {
                    var equipe = new Equipe { Especialidade = especialidade, Nome = "Equipe de " + especialidade.ToString() };

                    var estagiario1 = new Profissional { Especialidade = especialidade, Nome = "Estagiario 1", RA = "922222222", Email = "email1@example.com", Telefone = "31999999999", Tipo = TipoProfissional.Estagiario };
                    var estagiario2 = new Profissional { Especialidade = especialidade, Nome = "Estagiario 2", RA = "922222223", Email = "email2@example.com", Telefone = "31999999999", Tipo = TipoProfissional.Estagiario };
                    var professor = new Profissional { Especialidade = especialidade, Nome = "Professor", Email = "email3@example.com", Telefone = "31999999999", Tipo = TipoProfissional.Professor };

                    // Adicionando profissionais à equipe
                    equipe.Profissionais = new List<EquipeProfissional>
                    {
                        new EquipeProfissional { Profissional = estagiario1 },
                        new EquipeProfissional { Profissional = estagiario2 },
                        new EquipeProfissional { Profissional = professor }
                    };

                    // Adicionando entidades ao contexto
                    _context.Equipes.Add(equipe);
                    _context.Profissionais.AddRange(estagiario1, estagiario2, professor);
                }
            }

            // Salvando as alterações no banco de dados
            await _context.SaveChangesAsync();
        }


        public async Task GerarSalasAsync() {
            Especialidade[] especialidades = { Especialidade.Psicologia, Especialidade.Fisioterapia, Especialidade.Odontologia, Especialidade.Nutricao };

            foreach (var especialidade in especialidades) {
                // Verificar se há pelo menos duas salas para a especialidade
                var salasExistentes = _context.Salas.Where(s => s.Especialidade == especialidade).Count();
                if (salasExistentes < 2) {
                    // Criar as salas que faltam
                    if (!_context.Salas.Any(s => s.Nome == $"Sala de {especialidade} 1")) {
                        var sala1 = new Sala {
                            Especialidade = especialidade,
                            Nome = $"Sala de {especialidade} 1",
                            IsDisponivel = true
                        };
                        _context.Salas.Add(sala1);
                    }

                    if (!_context.Salas.Any(s => s.Nome == $"Sala de {especialidade} 2")) {
                        var sala2 = new Sala {
                            Especialidade = especialidade,
                            Nome = $"Sala de {especialidade} 2",
                            IsDisponivel = true
                        };
                        _context.Salas.Add(sala2);
                    }
                }
            }

            // Salvar as alterações no banco de dados
            await _context.SaveChangesAsync();
        }



        public void GerarPerfis() {
            string[] perfis = { "atendente", "cliente", "estagiario", "professor" };

            foreach (var perfilNome in perfis) {
                if (!_roleManager.RoleExistsAsync(perfilNome).Result) {
                    Perfil perfil = new();
                    perfil.Name = perfilNome;
                    perfil.NormalizedName = perfil.Name.ToUpper();
                    IdentityResult roleResult = _roleManager.CreateAsync(perfil).Result;
                }
            }
        }

        public void GerarUsuarios() {
            string[] perfis = { "atendente", "cliente", "estagiario", "professor" };

            foreach (var perfilNome in perfis) {
                string email = $"{perfilNome}@user.com.br";
                if (_userManager.FindByNameAsync(email).Result == null) {
                    Usuario usuario = new();
                    usuario.Name = perfilNome.First().ToString().ToUpper() + perfilNome.Substring(1);
                    usuario.UserName = email;
                    usuario.NormalizedUserName = usuario.UserName.ToUpper();
                    usuario.Email = usuario.UserName;
                    usuario.NormalizedEmail = usuario.Email.ToUpper();
                    usuario.LockoutEnabled = false;
                    usuario.SecurityStamp = Guid.NewGuid().ToString();
                    usuario.PhoneNumber = "38123456789";

                    IdentityResult result = _userManager.CreateAsync(usuario, "Teste1@").Result;

                    if (result.Succeeded) {
                        _userManager.AddToRoleAsync(usuario, perfilNome).Wait();
                    }
                }
            }
        }

    }
}
