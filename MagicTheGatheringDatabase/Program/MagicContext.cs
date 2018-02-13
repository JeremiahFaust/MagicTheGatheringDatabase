using Microsoft.EntityFrameworkCore;
using MagicDbContext.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagicDbContext
{
    public class MagicContext : DbContext
    {
        public DbSet<MultiverseCard> MultiverseCards { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Sets> Sets { get; set; }
        public DbSet<Types> Types { get; set; }
        public DbSet<ManaCosts> ManaCosts { get; set; }
        public DbSet<Abilities> Abilities { get; set; }
        public DbSet<Rulings> Rulings { get; set; }
        public DbSet<Color> Color { get; set; }
        public DbSet<CardAbilities> CardAbilities { get; set; }
        public DbSet<CardTypes> CardTypes { get; set; }

        public MagicContext(DbContextOptions<MagicContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ManaCosts>()
                .HasKey(cc => new { cc.CardID, cc.CardNumber, cc.ColorID });
            modelBuilder.Entity<ManaCosts>().HasOne(cc => cc.Cards).WithMany(c => c.ManaCosts);

            modelBuilder.Entity<CardAbilities>()
                .HasKey(cc => new { cc.CardID, cc.CardNumber, cc.AbilityID });
            modelBuilder.Entity<CardAbilities>().HasOne(cc => cc.Card).WithMany(c => c.CardAbilities);

            modelBuilder.Entity<CardTypes>()
                .HasKey(cc => new { cc.CardID, cc.CardNumber, cc.TypeID });
            modelBuilder.Entity<CardTypes>().HasOne(cc => cc.Card).WithMany(c => c.CardTypes);

            modelBuilder.Entity<Rulings>()
                .HasKey(cc => new { cc.CardID, cc.CardNumber, cc.Ruling });
            modelBuilder.Entity<Rulings>().HasOne(cc => cc.Cards).WithMany(c => c.Rulings);

            modelBuilder.Entity<MultiverseCard>().HasMany(cc => cc.cards).WithOne(c => c.MultiverseCard);

            modelBuilder.Entity<Card>()
                .HasKey(cc => new { cc.MultiverseID, cc.CardNumber });

            modelBuilder.Entity<MultiverseCard>().ToTable("MultiverseCards");
            modelBuilder.Entity<Card>().ToTable("Cards");
            modelBuilder.Entity<Sets>().ToTable("Sets");
            modelBuilder.Entity<Types>().ToTable("Types");
            modelBuilder.Entity<ManaCosts>().ToTable("ManaCosts");
            modelBuilder.Entity<Abilities>().ToTable("Abilities");
            modelBuilder.Entity<Rulings>().ToTable("Rulings");
            modelBuilder.Entity<Color>().ToTable("Color");
            modelBuilder.Entity<CardAbilities>().ToTable("CardAbilities");
            modelBuilder.Entity<CardTypes>().ToTable("CardTypes");

            //modelBuilder.Entity<Cards>().HasMany(c => c.CardTypes);
            //modelBuilder.Entity<Cards>().HasMany(m => m.ManaCosts);
            //modelBuilder.Entity<Cards>().HasMany(a => a.CardAbilities);
            //modelBuilder.Entity<Cards>().HasMany(r => r.Rulings).WithOne(c => c.Cards);
        }
    }
}
