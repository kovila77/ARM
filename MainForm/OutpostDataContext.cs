namespace MainForm
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class OutpostDataContext : DbContext
    {
        public OutpostDataContext()
            : base("name=OutpostDataContext")
        {
        }

        public virtual DbSet<building> buildings { get; set; }
        public virtual DbSet<buildings_resources_consume> buildings_resources_consume { get; set; }
        public virtual DbSet<buildings_resources_produce> buildings_resources_produce { get; set; }
        public virtual DbSet<machine> machines { get; set; }
        public virtual DbSet<machines_resources_consume> machines_resources_consume { get; set; }
        public virtual DbSet<outpost_missions> outpost_missions { get; set; }
        public virtual DbSet<outpost> outposts { get; set; }
        public virtual DbSet<resource> resources { get; set; }
        public virtual DbSet<storage_resources> storage_resources { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<building>()
                .HasMany(e => e.buildings_resources_consume)
                .WithRequired(e => e.building)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<building>()
                .HasMany(e => e.buildings_resources_produce)
                .WithRequired(e => e.building)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<machine>()
                .HasMany(e => e.machines_resources_consume)
                .WithRequired(e => e.machine)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<outpost>()
                .HasMany(e => e.outpost_missions)
                .WithRequired(e => e.outpost)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<outpost>()
                .HasMany(e => e.storage_resources)
                .WithRequired(e => e.outpost)
                .WillCascadeOnDelete(true);

        }
    }
}
