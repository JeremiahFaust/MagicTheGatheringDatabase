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
            ctxt.Database.EnsureDeleted();
            ctxt.Database.EnsureCreated();
            ini();
        }

        private void ini()
        {
            if(ctxt.Cards.Any())
            {
                return;
            }

            var Sets = new Sets[]
            {
                new Sets{ SetID= 1, SetAbbr="AER", SetFullName="Aether Revolt"},
            };

            ctxt.Sets.AddRange(Sets);
            ctxt.SaveChanges(true);

            var types = new Types[]
            {
                new Types{ TypeID=1, TypeName="Artifact Creature" },
                new Types{ TypeID=2, TypeName="Construct"}
            };
            
            ctxt.Types.AddRange(types);
            ctxt.SaveChanges(true);

            var abilities = new Abilities[]
            {
                new Abilities{ AbilityID =1, Ability="{4}{W}: Return another target creature you control to its owner's hand."}
            };

            ctxt.Abilities.AddRange(abilities);
            ctxt.SaveChanges(true);

            var colors = new Color[]
            {
                new Color{ ColorID=1, ColorName="Two Colorless", ColorSymbol='2', ColorValue=2 }
            };

            ctxt.Color.AddRange(colors);
            ctxt.SaveChanges(true);

            var Cards = new Cards[]
            {
                new Cards { MultiverseID=423808, Artist="Kieran Yanner", CardName="Aegis Automaton", FlavorText="#_The streets of Ghirapur have become dangerous. It's good to have a dependable companion._#", HighPrice=0.95, LowPrice=0.01, MidPrice=0.1, Power= 0, Toughness=3, Rarity="C", Rating=5, SetID=1}
            };

            ctxt.Cards.AddRange(Cards);
            ctxt.SaveChanges(true);
            
            var CardTypes = new CardTypes[]
            {
                new MagicDbContext.Models.CardTypes{ CardTypeID=1, TypeID=1, CardID=423808 },
                new CardTypes { CardTypeID=2, TypeID=2, CardID=423808 }
            };

            ctxt.CardTypes.AddRange(CardTypes);
            ctxt.SaveChanges(true);

            var Rulings = new Rulings[]
            {
                new Rulings{ CardID=423808, RulingsID=1, Ruling=""}
            };

            ctxt.Rulings.AddRange(Rulings);
            ctxt.SaveChanges(true);

            var cardAbilities = new CardAbilities[]
            {
                new CardAbilities{ CardAbilitiesID=1, AbilityID=1, CardID=423808}
            };

            ctxt.CardAbilities.AddRange(cardAbilities);
            ctxt.SaveChanges(true);

            var manaCosts = new ManaCosts[]
            {
                new ManaCosts { ManaCostID=1, ColorID=1, CardID=423808 }
            };

            ctxt.ManaCosts.AddRange(manaCosts);
            ctxt.SaveChanges(true);

            //ctxt.Cards.Single(s=> s.MultiverseID==423808).SetID=0;

            //ctxt.Cards.AddRange(Cards);
            ctxt.SaveChanges(true);
        }

    }
}
