using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Reflection;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<Usuario, Perfil, Guid>, IApplicationDbContext
    {

        private readonly IDomainEventService _domainEventService;
        private readonly IDateTime _dateTime;

        protected IDbContextTransaction _contextoTransaction { get; set; }


        public ApplicationDbContext(
            DbContextOptions options,
            IDomainEventService domainEventService,
            IDateTime dateTime
            ) : base(options)
        {
            _domainEventService = domainEventService;
            _dateTime = dateTime;
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<ListaEspera> ListaEspera { get; set; }
        public DbSet<Profissional> Profissionais { get; set; }
        public DbSet<Equipe> Equipes { get; set; }
        public DbSet<EquipeProfissional> EquipeProfissional { get; set; }
        public DbSet<Agendamento> Agendamentos { get; set; }
        public DbSet<Consulta> Consultas { get; set; }
        public DbSet<Sala> Salas { get; set; }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }

        public async Task RollBack() {
            if (_contextoTransaction != null) {
                await _contextoTransaction.RollbackAsync();
            }
        }

        private async Task Commit() {
            if (_contextoTransaction != null) {
                await _contextoTransaction.CommitAsync();
                await _contextoTransaction.DisposeAsync();
                _contextoTransaction = null;
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken()) {
            try {

                //Guid? _userId = _currentUserService.UserId != Guid.Empty ? _currentUserService.UserId : null;

                foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<AuditableEntity> entry in ChangeTracker.Entries<AuditableEntity>()) {
                    switch (entry.State) {
                        case EntityState.Added:
                            //entry.Entity.CreatedBy = _userId;
                            entry.Entity.Created = _dateTime.Now;
                            //entry.Entity.GerarHistoricoPersonalizado("Registro Incluído", TipoAcaoHistorico.Incluido, _userId);
                            break;

                        case EntityState.Modified:
                            //entry.Entity.LastModifiedBy = _userId;
                            entry.Entity.LastModified = _dateTime.Now;
                            break;
                        case EntityState.Deleted:
                            //entry.Entity.LastModifiedBy = _userId;
                            entry.Entity.LastModified = _dateTime.Now;
                            entry.Entity.ExcludedAt = _dateTime.Now;
                            //entry.Entity.GerarHistoricoPersonalizado("Registro Excluído", TipoAcaoHistorico.Excluido, _userId);
                            break;
                    }
                }

                var result = await base.SaveChangesAsync(cancellationToken);

                if (_contextoTransaction != null)
                    await Commit();

                await DispatchEvents();

                return result;
            } catch (Exception ex) {
                await RollBack();
                throw;
            }
        }

        private async Task DispatchEvents() {
            while (true) {
                var domainEventEntity = ChangeTracker.Entries<IHasDomainEvent>()
                    .Select(x => x.Entity.DomainEvents)
                    .SelectMany(x => x)
                    .Where(domainEvent => !domainEvent.IsPublished)
                    .FirstOrDefault();
                if (domainEventEntity == null) break;

                domainEventEntity.IsPublished = true;
                await _domainEventService.Publish(domainEventEntity);
            }
        }



    }
}
