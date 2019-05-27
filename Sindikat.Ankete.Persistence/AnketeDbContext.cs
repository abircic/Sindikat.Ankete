
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SindikatAnkete.Entity;

namespace Sindikat.Ankete.Persistence
{
    public class AnketeDbContext : DbContext
    {
        public AnketeDbContext(DbContextOptions<AnketeDbContext> options) : base(options)
        {

        }


        public virtual DbSet<AnketaEntity> Ankete { get; set; }
        public virtual DbSet<PopunjenaAnketaEntity> PopunjeneAnkete { get; set; }
        public virtual DbSet<PitanjeEntity> Pitanja { get; set; }
        public virtual DbSet<OdgovorEntity> Odgovori { get; set; }
        public virtual DbSet<TipPitanjaEntity> TipoviPitanja { get; set; }
        public virtual DbSet<PonudeniOdgovorEntity> PonudeniOdgovori { get; set; }

        //public override int SaveChanges()
        //{
        //    var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

        //    foreach (var entity in entities)
        //    {
        //        if (entity.State == EntityState.Added)
        //        {
        //            ((BaseEntity)entity.Entity).CreatedAt = DateTime.UtcNow;
        //        }

        //        ((BaseEntity)entity.Entity).UpdatedAt = DateTime.UtcNow;
        //    }

        //    return base.SaveChanges();
        //}

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AnketeDbContext).Assembly);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PopunjenaAnketaEntity>().HasKey(c => new { c.AnketaId, c.KorisnikId });
            modelBuilder.Entity<OdgovorEntity>().HasKey(o => new { o.PitanjeId, o.KorisnikId});

        }

    }
}
