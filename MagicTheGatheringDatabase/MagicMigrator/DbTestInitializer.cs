using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using MagicMigrator;
using MagicDbContext;
using MagicDbContext.Models;
using System.Linq;
using System.Collections;

namespace MagicMigrator
{
    public class DbTestInitializer
    {
        MagicContext ctxt;
        public DbTestInitializer()
        {
            DbContextOptionsBuilder<MagicContext> dbContextOptions = new DbContextOptionsBuilder<MagicContext>();

            dbContextOptions.EnableSensitiveDataLogging();
            dbContextOptions.UseSqlite("Data Source=MagicDB.db", providerOptions => providerOptions.CommandTimeout(60));

            ctxt = new MagicContext(dbContextOptions.Options);


            var car = ctxt.Cards
                .Include(c => c.CardTypes)
                    .ThenInclude(ct => ct.Type)
                .Include(c => c.Set)
                .Include(c => c.ManaCosts)
                    .ThenInclude(mc => mc.Color);
            foreach (Card c in car)
            {
                Console.WriteLine(c.CardName);
                //var s = c.Set.SetFullName;
                //ctxt.Entry(c).Collection(p => p.CardTypes).Load();
                foreach (CardTypes ty in c.CardTypes)
                {
                    //ctxt.Entry(ty).Reference(t => t.Type).Load();
                    Console.WriteLine(ty.Type.Name);
                }
            }
            Console.ReadLine();


            //ctxt.Database.EnsureDeleted();
            //ctxt.Database.EnsureCreated();
            //ini();

        }

        private void ini()
        {
            if(ctxt.Cards.Any())
            {
                return;
            }

            var _colors = SeedColors();
            Color FindColor(string name) => _colors.Where(f => f.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            var _types = SeedTypes();
            Types FindType(string name) => _types.Where(f => f.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();


            var _sets = SeedSets();
            Sets FindSets(string name) => _sets.Where(f => f.SetAbbr.Equals(name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            var _abilities = SeedAbilities();
            Abilities FindAbility(string ability) => _abilities.Where(a => a.Ability.Equals(ability, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();


            

            //var Sets = new Sets[]
            //{
            //    new Sets{ SetAbbr="AER", SetFullName="Aether Revolt"},
            //};

            //ctxt.Sets.AddRange(Sets);
            //ctxt.SaveChanges(true);

            //var types = new Types[]
            //{
            //    new Types{ ID=1, Name="Artifact Creature" },
            //    new Types{ ID=2, Name="Construct"}
            //};
            
            //ctxt.Types.AddRange(types);
            //ctxt.SaveChanges(true);

            //var abilities = new Abilities[]
            //{
            //    new Abilities{ AbilityID =1, Ability="{4}{W}: Return another target creature you control to its owner's hand."}
            //};

            //ctxt.Abilities.AddRange(abilities);
            //ctxt.SaveChanges(true);

            //var colors = new Color[]
            //{
            //    new Color{ Name="Colorless" }
            //};

            //ctxt.Color.AddRange(colors);
            //ctxt.SaveChanges(true);

            var Cards = new Card[]
            {
                new Card { MultiverseID="423808", Artist="Kieran Yanner", CardName="Aegis Automaton", FlavorText="#_The streets of Ghirapur have become dangerous. It's good to have a dependable companion._#", HighPrice=0.95, LowPrice=0.01, MidPrice=0.1, Power= 0, Toughness=3, Rarity="C", Rating=5, SetID=FindSets("AER").SetAbbr}
            };

            ctxt.Cards.AddRange(Cards);
            ctxt.SaveChanges(true);
            
            var CardTypes = new CardTypes[]
            {
                new MagicDbContext.Models.CardTypes{ TypeID=FindType("Artifact").ID, CardID="423808" },
                new CardTypes { TypeID=FindType("Creature").ID, CardID="423808" },
                new CardTypes { TypeID=FindType("Construct").ID, CardID="423808" }
            };

            ctxt.CardTypes.AddRange(CardTypes);
            ctxt.SaveChanges(true);

            var Rulings = new Rulings[]
            {
                new Rulings{ CardID="423808", Ruling=""}
            };

            ctxt.Rulings.AddRange(Rulings);
            ctxt.SaveChanges(true);

            var cardAbilities = new CardAbilities[]
            {
                new CardAbilities{ AbilityID=FindAbility("{4}{W}: Return another target creature you control to its owner's hand.").AbilityID, CardID="423808"}
            };

            ctxt.CardAbilities.AddRange(cardAbilities);
            ctxt.SaveChanges(true);

            var manaCosts = new ManaCosts[]
            {
                new ManaCosts { Quantity=2, ColorID=FindColor("Colorless").ID, CardID="423808" }
            };

            ctxt.ManaCosts.AddRange(manaCosts);
            ctxt.SaveChanges(true);

            //ctxt.Cards.Single(s=> s.MultiverseID==423808).SetID=0;

            //ctxt.Cards.AddRange(Cards);
            ctxt.SaveChanges(true);
        }

        private IEnumerable<Abilities> SeedAbilities()
        {
            HashSet<string> types = new HashSet<string>();
            types.Add("{4}{W}: Return another target creature you control to its owner's hand.");
            types.Add("Flying");
            ctxt.Abilities.AddRange(types.Select(f => new Abilities { Ability = f }));
            ctxt.SaveChanges();
            return ctxt.Abilities.ToArray();
        }

        private IEnumerable<Sets> SeedSets()
        {
            ctxt.Sets.AddRange(
            new Sets { SetAbbr = "AER", SetFullName= "Aether Revolt" }
            );
            ctxt.SaveChanges();
            return ctxt.Sets.ToArray();
        }

        private IEnumerable<Types> SeedTypes()
        {
            ctxt.Types.AddRange(
            new Types { Name = "Artifact" },
            new Types { Name = "Creature" },
            new Types { Name = "Construct" }
            );
            ctxt.SaveChanges();
            return ctxt.Types.ToArray();

            //HashSet<string> types = new HashSet<string>();

            //ctxt.AddRange(types.Select(s => new Types { Name = s }));
        }

        private IEnumerable<Color> SeedColors()
        {
            ctxt.Color.AddRange(
                new Color { Name = "White" } ,
                new Color { Name = "Black" },
                new Color{ Name="Colorless" }
                );
            ctxt.SaveChanges();
            return ctxt.Color.ToArray();
        }
    }
}
