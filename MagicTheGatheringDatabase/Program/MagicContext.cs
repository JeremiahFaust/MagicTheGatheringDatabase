using Microsoft.EntityFrameworkCore;
using Program.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Program
{
    class MagicContext : DbContext
    {

        public DbSet<Cards> Cards { get; set; }
        public DbSet<Sets> Sets { get; set; }
        public DbSet<Types> Types { get; set; }
        public DbSet<ManaCosts> ManaCosts { get; set; }
        public DbSet<Abilities> Abilities { get; set; }
        public DbSet<Rulings> Rulings { get; set; }
        public DbSet<Color> Color { get; set; }
        public DbSet<CardAbilities> CardAbilities { get; set; }
        public DbSet<CardAbilities> CardTypes { get; set; }

        public MagicContext(DbContextOptions<MagicContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cards>().ToTable("Cards");
            modelBuilder.Entity<Sets>().ToTable("Sets");
            modelBuilder.Entity<Types>().ToTable("Types");
            modelBuilder.Entity<ManaCosts>().ToTable("ManaCosts");
            modelBuilder.Entity<Abilities>().ToTable("Abilities");
            modelBuilder.Entity<Rulings>().ToTable("Rulings");
            modelBuilder.Entity<Color>().ToTable("Color");
            modelBuilder.Entity<CardAbilities>().ToTable("CardAbilities");
            modelBuilder.Entity<CardTypes>().ToTable("CardTypes");


        }
    }
}
