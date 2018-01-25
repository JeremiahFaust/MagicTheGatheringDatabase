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
                new Sets{ ID= 0, SetAbbr="AER", SetFullName="Aether Revolt"},
            };

            ctxt.Sets.AddRange(Sets);

            var types = new Types[]
            {
                new Types{ ID=0, TypeName="Artifact Creature" },
                new Types{ ID=1, TypeName="Construct"}
            };

            ctxt.Types.AddRange(types);

            var CardTypes = new CardTypes[]
            {
                new MagicDbContext.Models.CardTypes{ ID=0, TypeID=0, CardID=423808 },
                new CardTypes { ID=1, TypeID=1, CardID=423808 }
            };

            ctxt.CardTypes.AddRange(CardTypes);

            var Rulings = new Rulings[]
            {
                new Rulings{ CardID=423808, ID=0, Ruling=""}
            };

            ctxt.Rulings.AddRange(Rulings);

            var abilities = new Abilities[]
            {
                new Abilities{ ID =0, Ability="{4}{W}: Return another target creature you control to its owner's hand."}
            };

            ctxt.Abilities.AddRange(abilities);

            var cardAbilities = new CardAbilities[]
            {
                new CardAbilities{ ID=0, AbilityID=0, CardID=423808}
            };

            ctxt.CardAbilities.AddRange(cardAbilities);

            var colors = new Color[]
            {
                new Color{ ID=0, ColorName="Two Colorless", ColorSymbol='2', ColorValue=2 }
            };

            ctxt.Color.AddRange(colors);

            var manaCosts = new ManaCosts[]
            {
                new ManaCosts { ID=0, ColorID=0, CardID=423808 }
            };

            ctxt.ManaCosts.AddRange(manaCosts);

            var Cards = new Cards[]
            {
                new Cards { MultiverseID=423808, Artist="Kieran Yanner", CardName="Aegis Automaton", FlavorText="#_The streets of Ghirapur have become dangerous. It's good to have a dependable companion._#", HighPrice=0.95, LowPrice=0.01, MidPrice=0.1, Power= 0, Toughness=3, Rarity="C", Rating=5, SetID=0, CardType="Artifact Creature — Construct"}
            };

            ctxt.Cards.AddRange(Cards);
            ctxt.SaveChanges(true);
        }

    }
}
