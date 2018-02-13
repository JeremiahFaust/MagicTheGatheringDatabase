using MagicDbContext;
using MagicDbContext.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace MagicMigrator
{
    class DbInitializer
    {
        MagicContext ctxt;

        private IEnumerable<Color> _colors;
        private IEnumerable<Types> _types;
        private IEnumerable<Sets> _sets;
        private IEnumerable<Abilities> _abilities;

        Color FindColor(string name, string symbol) => _colors.Where(f => f.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && f.Symbol.Equals(symbol)).FirstOrDefault();

        Types FindType(string name) => _types.Where(f => f.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();


        Sets FindSets(string name) => _sets.Where(f => f.SetAbbr.Equals(name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

        Abilities FindAbility(string ability) => _abilities.Where(a => a.Ability.Equals(ability, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();


        private char[] alphabet = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p' };

        


        public DbInitializer(MagicContext context)
        {
            this.ctxt = context;
            ctxt.Database.EnsureDeleted();
            ctxt.Database.EnsureCreated();

            BeginMigration();
            
        }



        public void BeginMigration()
        {
            FileInfo file = new FileInfo("MTGDatabase.xlsx");
            BeginMigration(file);
        }

        public void BeginMigration(FileInfo fileInfo)
        {
            Console.WriteLine("StartTime: " + DateTime.Now);

            if (!fileInfo.Exists)
                throw new FileNotFoundException();

            using (ExcelPackage pkg = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet wksht = pkg.Workbook.Worksheets[1];
                //ExcelWorksheet wksht = pkg.Workbook.Worksheets[2];
                int rows = wksht.Dimension.Rows;

                if (!ctxt.Sets.Any()) IniSetsColorsTypesAbilities(wksht, rows);
                if(!ctxt.Cards.Any()) IniCards(wksht, rows);

            }
        }

        private void IniSetsColorsTypesAbilities(ExcelWorksheet wksht, int rows)
        {
            HashSet<string> abilities = new HashSet<string>();
            HashSet<string> colors = new HashSet<string>();
            HashSet<string> types = new HashSet<string>();

            for (int row = 2; row < rows+1; row++)
            {
                getAbilities(abilities, row, wksht);
                getColorlessMana(colors, row, wksht);
                getTypes(types, row, wksht);
            }
            this._abilities = SeedAbilities(abilities);
            this._colors = SeedColors(colors);
            this._sets = SeedSets();
            this._types = SeedTypes(types);
        }

        private void IniCards(ExcelWorksheet wksht, int rows)
        {
            //List<MultiverseCard> mcards = new List<MultiverseCard>();
            //List<Card> cards = new List<Card>();
            //List<CardTypes> cardTypes = new List<CardTypes>();
            //List<Rulings> rulings = new List<Rulings>();
            //List<CardAbilities> cardAbilities = new List<CardAbilities>();
            //List<ManaCosts> manaCosts = new List<ManaCosts>();

            for (int row = 2; row<rows+1; row++)
            {
                string set = wksht.Cells[row, 3].Text;
                string multiverseID = wksht.Cells[row, 1].Text;
                
                string imgRelPath = set + "\\" + multiverseID + ".full.jpg";

                MultiverseCard mc = new MultiverseCard { ImagePath = imgRelPath, MultiverseId = multiverseID, SetID = FindSets(set).SetAbbr };

                ctxt.MultiverseCards.Add(mc);

                ctxt.Cards.AddRange(ReadCard(wksht, row, multiverseID));
            }
            
            //ctxt.AddRange(mcards);
            //ctxt.AddRange(cards);
            //ctxt.AddRange(cardTypes);
            //ctxt.AddRange(rulings);
            //ctxt.AddRange(cardAbilities);
            //ctxt.AddRange(manaCosts);
            ctxt.SaveChanges();

            Console.WriteLine("CompletionTime: " + DateTime.Now);
            Console.ReadLine();

        }

        private IEnumerable<Card> ReadCard(ExcelWorksheet wksht, int row, string multiverseID)
        {
            string cardname = wksht.Cells[row, 2].Text;

            string[] cnames = cardname.Split("//");
            
            if(cnames.Length==1)
            {
                return getCard(wksht, row, multiverseID);
            }
            else
            {
                return getDualCard(wksht, row, multiverseID, cnames);
            }
            
        }

        private IEnumerable<Card> getDualCard(ExcelWorksheet wksht, int row, string multiverseID, string[] cnames)
        {
            List<Card> cards = new List<Card>();

            string cardnumber = wksht.Cells[row, 15].Text;
            string artist = wksht.Cells[row, 14].Text.Split("//")[0].Trim();
            string[] flavortext = wksht.Cells[row, 12].Text.Split("//");
            double highprice = wksht.Cells[row, 22].Text.AsOrDefault<Double>();
            double lowprice = wksht.Cells[row, 20].Text.AsOrDefault<Double>();
            double midprice = wksht.Cells[row, 21].Text.AsOrDefault<Double>();
            string[] power = wksht.Cells[row, 8].Text.Split("//");
            string[] toughness = wksht.Cells[row, 9].Text.Split("//");
            string rarity = wksht.Cells[row, 5].Text.Split("//")[0].Trim();
            int rating = wksht.Cells[row, 16].Text.AsOrDefault<Int32>();
            string[] convertedmanacost = wksht.Cells[row, 7].Text.Split("//");

            for (int i = 0; i<cnames.Length; i++)
            {
                Card c = new Card { MultiverseID = multiverseID, CardNumber = cardnumber + alphabet[i], Artist = artist, CardName = (cnames.Length>i)?cnames[i].Trim():"", FlavorText = (flavortext.Length>i)?flavortext[i].Trim():"", HighPrice = highprice, LowPrice = lowprice, MidPrice = midprice, Power = (power.Length>i)?power[i].Trim().AsOrDefault<int>():0, Toughness = (toughness.Length>i)?toughness[i].Trim().AsOrDefault<int>():0, Rarity = rarity, Rating = rating, ConvertedManaCost = (convertedmanacost.Length>i)?convertedmanacost[i].Trim().AsOrDefault<int>():0, IsDualCard = true };

                cards.Add(c);
                iniCardSubAttributesDual(wksht, row, multiverseID, cardnumber + alphabet[i], i);
                Console.WriteLine(cnames[i]);

            }

            return cards;
        }

        private IEnumerable<Card> getCard(ExcelWorksheet wksht, int row, string multiverseID)
        {
            List<Card> cards = new List<Card>();
            
            string cardnumber = wksht.Cells[row, 15].Text;
            string artist = wksht.Cells[row, 14].Text;
            string cardname = wksht.Cells[row, 2].Text;
            string flabortext = wksht.Cells[row, 12].Text;
            double highprice = wksht.Cells[row, 22].Text.AsOrDefault<Double>();
            double lowprice = wksht.Cells[row, 20].Text.AsOrDefault<Double>();
            double midprice = wksht.Cells[row, 21].Text.AsOrDefault<Double>();
            int power = wksht.Cells[row, 8].Text.AsOrDefault<Int32>();
            int toughness = wksht.Cells[row, 9].Text.AsOrDefault<Int32>();
            string rarity = wksht.Cells[row, 5].Text;
            int rating = wksht.Cells[row, 16].Text.AsOrDefault<Int32>();
            int convertedmanacost = wksht.Cells[row, 7].Text.AsOrDefault<Int32>();
            bool isDual = false;

            Card c = new Card { MultiverseID = multiverseID, CardNumber = cardnumber, Artist = artist, CardName = cardname, FlavorText = wksht.Cells[row, 12].Text, HighPrice = highprice, LowPrice = lowprice, MidPrice = midprice, Power = power, Toughness = toughness, Rarity = rarity, Rating = rating, ConvertedManaCost = convertedmanacost, IsDualCard = isDual };

            cards.Add(c);

            iniCardSubAttributes(wksht, row, multiverseID, cardnumber);


            Console.WriteLine(cardname);
            return cards;
        }
        
        private void iniCardSubAttributesDual(ExcelWorksheet wksht, int row, string multiverseID, string cardNumber, int i)
        {
            HashSet<string> types = new HashSet<string>();
            string[] unsplit = wksht.Cells[row, 4].Text.Split("//");
            string tmp = (unsplit.Length > i) ? unsplit[i] : "";
            tmp = tmp.Trim();
            getTypesDual(types, tmp);


            ctxt.CardTypes.AddRange(types.Select(t => new MagicDbContext.Models.CardTypes { TypeID = FindType(t).ID, CardID = multiverseID, CardNumber=cardNumber }));

            HashSet<string> ruling = new HashSet<string>();
            getRulings(ruling, row, wksht);

            ctxt.Rulings.AddRange(ruling.Select(r => new MagicDbContext.Models.Rulings { CardID = multiverseID, Ruling = getRuling(r), Date = getRulingDate(r), CardNumber = cardNumber }));

            HashSet<string> ab = new HashSet<string>();
            string[] abilities = wksht.Cells[row, 11].Text.Split("//");
            tmp = (abilities.Length > i) ? abilities[i] : "";
            tmp = tmp.Trim();
            getAbilitiesDual(ab, tmp);

            ctxt.CardAbilities.AddRange(ab.Select(a => new CardAbilities { AbilityID = FindAbility(a).AbilityID, CardID = multiverseID, CardNumber =  cardNumber}));

            string[] mana = wksht.Cells[row, 6].Text.Split("//");
            tmp = (mana.Length > i) ? mana[i] : "";
            tmp = tmp.Trim();
            getManaCostDual(row, tmp, multiverseID, cardNumber);
        }

        private void iniCardSubAttributes(ExcelWorksheet wksht, int row, string multiverseID, string cardNumber)
        {
            HashSet<string> types = new HashSet<string>();
            getTypes(types, row, wksht);
            

            ctxt.CardTypes.AddRange(types.Select(t => new MagicDbContext.Models.CardTypes { TypeID = FindType(t).ID, CardID = multiverseID, CardNumber=cardNumber }));

            HashSet<string> ruling = new HashSet<string>();
            getRulings(ruling, row, wksht);

            ctxt.Rulings.AddRange(ruling.Select(r => new MagicDbContext.Models.Rulings { CardID = multiverseID, Ruling = getRuling(r), Date = getRulingDate(r), CardNumber =cardNumber }));

            HashSet<string> ab = new HashSet<string>();
            getAbilities(ab, row, wksht);

            ctxt.CardAbilities.AddRange(ab.Select(a => new CardAbilities { AbilityID = FindAbility(a).AbilityID, CardID = multiverseID, CardNumber = cardNumber }));

            getManaCost(row, wksht, multiverseID, cardNumber);
        }

        private void getTypesDual(HashSet<string> types, string creatureTypes)
        {
            string[] typesarr = creatureTypes.Split(' ');

            foreach (string s in typesarr)
            {
                if (String.IsNullOrEmpty(s)) continue;
                if (s.Equals("—")) continue;
                types.Add(s);
            }
        }

        private void getTypes(HashSet<string> types, int row, ExcelWorksheet wksht)
        {
            string unsplit = wksht.Cells[row, 4].Text;

            string[] typesarr = unsplit.Split(' ');

            foreach( string s in typesarr)
            {
                if (String.IsNullOrEmpty(s)) continue;
                if (s.Equals("—")) continue;
                types.Add(s);
            }
        }

        private void getColorlessMana(HashSet<string> colors, int row, ExcelWorksheet wksht)
        {
            string col = wksht.Cells[row, 6].Text;

            string[] color = col.Split('{','}');

            foreach (string s in color)
            {
                int i = 0;
                if (String.IsNullOrEmpty(s)) continue;
                if(Int32.TryParse(s,out i)) colors.Add(s); //if mana in a number than it is a colorless mana. all other mana types are already added into database.
            }
        }

        private void getAbilitiesDual(HashSet<string> abilities, string abi)
        {
            string[] abili = abi.Split('£');

            foreach (string s in abili)
            {
                if (String.IsNullOrEmpty(s) || String.IsNullOrWhiteSpace(s)) continue;
                abilities.Add(s);
            }
        }

        private void getAbilities(HashSet<string> abilities, int row, ExcelWorksheet wksht)
        {
            string abi = wksht.Cells[row, 11].Text;

            string[] abili = abi.Split('£');

            foreach(string s in abili)
            {
                if (String.IsNullOrEmpty(s) || String.IsNullOrWhiteSpace(s)) continue;
                abilities.Add(s);
            }
        }

        private void getManaCostDual(int row, string col, string multiverseID, string cardNumber)
        {
            //if (multiverseID.Equals("426917"))
            //{
            //    Console.WriteLine("break");
            //}

            int white = 0;
            int black = 0;
            int green = 0;
            int blue = 0;
            int colorless = 0;
            int red = 0;

            string[] color = col.Split('{', '}');
            List<ManaCosts> manaCosts = new List<ManaCosts>();

            foreach (string s in color)
            {
                int i = 0;
                if (String.IsNullOrEmpty(s)) continue;
                if (Int32.TryParse(s, out i))
                {
                    ManaCosts mc = new ManaCosts { CardID = multiverseID, ColorID = FindColor("Uncolored", s).ID, Quantity = i, CardNumber = cardNumber };
                    if (!manaCosts.Contains(mc)) ctxt.ManaCosts.Add(mc);
                    continue;
                }
                if (s.Equals("W", StringComparison.OrdinalIgnoreCase)) { white++; continue; }
                if (s.Equals("B", StringComparison.OrdinalIgnoreCase)) { black++; continue; }
                if (s.Equals("U", StringComparison.OrdinalIgnoreCase)) { blue++; continue; }
                if (s.Equals("G", StringComparison.OrdinalIgnoreCase)) { green++; continue; }
                if (s.Equals("R", StringComparison.OrdinalIgnoreCase)) { red++; continue; }
                if (s.Equals("X", StringComparison.OrdinalIgnoreCase)) { colorless++; continue; }
            }
            if (white > 0)
            {
                ManaCosts mc = new ManaCosts { CardID = multiverseID, ColorID = FindColor("White", "W").ID, Quantity = white, CardNumber = cardNumber };
                if (!manaCosts.Contains(mc)) ctxt.ManaCosts.Add(mc);
            }
            if (black > 0)
            {
                ManaCosts mc = new ManaCosts { CardID = multiverseID, ColorID = FindColor("Black", "B").ID, Quantity = black, CardNumber = cardNumber };
                if (!manaCosts.Contains(mc)) ctxt.ManaCosts.Add(mc);
            }

            if (green > 0)
            {
                ManaCosts mc = new ManaCosts { CardID = multiverseID, ColorID = FindColor("Green", "G").ID, Quantity = green, CardNumber = cardNumber };
                if (!manaCosts.Contains(mc)) ctxt.ManaCosts.Add(mc);
            }

            if (blue > 0)
            {
                ManaCosts mc = new ManaCosts { CardID = multiverseID, ColorID = FindColor("Blue", "U").ID, Quantity = blue, CardNumber = cardNumber };
                if (!manaCosts.Contains(mc)) ctxt.ManaCosts.Add(mc);
            }

            if (colorless > 0)
            {
                ManaCosts mc = new ManaCosts { CardID = multiverseID, ColorID = FindColor("X", "X").ID, Quantity = colorless, CardNumber = cardNumber };
                if (!manaCosts.Contains(mc)) ctxt.ManaCosts.Add(mc);
            }

            if (red > 0)
            {
                ManaCosts mc = new ManaCosts { CardID = multiverseID, ColorID = FindColor("Red", "R").ID, Quantity = red, CardNumber = cardNumber };
                if (!manaCosts.Contains(mc)) ctxt.ManaCosts.Add(mc);
            }

        }


        private void getManaCost(int row, ExcelWorksheet wksht, string multiverseID, string cardNumber)
        {
            //if (multiverseID.Equals("426917"))
            //{
            //    Console.WriteLine("break");
            //}
            string col = wksht.Cells[row, 6].Text;

            int white = 0;
            int black = 0;
            int green = 0;
            int blue = 0;
            int colorless = 0;
            int red = 0;

            string[] color = col.Split('{', '}');
            List<ManaCosts> manaCosts = new List<ManaCosts>();

            foreach (string s in color)
            {
                int i = 0;
                if (String.IsNullOrEmpty(s)) continue;
                if (Int32.TryParse(s, out i))
                {
                    ManaCosts mc = new ManaCosts { CardID = multiverseID, ColorID = FindColor("Uncolored", s).ID, Quantity = i, CardNumber = cardNumber };
                    if (!manaCosts.Contains(mc)) ctxt.ManaCosts.Add(mc);
                    continue;
                }
                if (s.Equals("W", StringComparison.OrdinalIgnoreCase)) { white++; continue; }
                if (s.Equals("B", StringComparison.OrdinalIgnoreCase)) { black++; continue; }
                if (s.Equals("U", StringComparison.OrdinalIgnoreCase)) { blue++; continue; }
                if (s.Equals("G", StringComparison.OrdinalIgnoreCase)) { green++; continue; }
                if (s.Equals("R", StringComparison.OrdinalIgnoreCase)) { red++; continue; }
                if (s.Equals("X", StringComparison.OrdinalIgnoreCase)) { colorless++; continue; }
            }
            if (white > 0)
            {
                ManaCosts mc = new ManaCosts { CardID = multiverseID, ColorID = FindColor("White", "W").ID, Quantity = white, CardNumber = cardNumber };
                if (!manaCosts.Contains(mc)) ctxt.ManaCosts.Add(mc);
            } 
            if (black > 0)
            {
                ManaCosts mc = new ManaCosts { CardID = multiverseID, ColorID = FindColor("Black", "B").ID, Quantity = black, CardNumber = cardNumber };
                if (!manaCosts.Contains(mc)) ctxt.ManaCosts.Add(mc);
            }
            
            if (green > 0)
            {
                ManaCosts mc = new ManaCosts { CardID = multiverseID, ColorID = FindColor("Green", "G").ID, Quantity = green, CardNumber = cardNumber };
                if (!manaCosts.Contains(mc)) ctxt.ManaCosts.Add(mc);
            }
            
            if (blue > 0)
            {
                ManaCosts mc = new ManaCosts { CardID = multiverseID, ColorID = FindColor("Blue", "U").ID, Quantity = blue, CardNumber = cardNumber };
                if (!manaCosts.Contains(mc)) ctxt.ManaCosts.Add(mc);
            }
            
            if (colorless > 0)
            {
                ManaCosts mc = new ManaCosts { CardID = multiverseID, ColorID = FindColor("X", "X").ID, Quantity = colorless, CardNumber = cardNumber };
                if (!manaCosts.Contains(mc)) ctxt.ManaCosts.Add(mc);
            }
           
            if (red > 0)
            {
                ManaCosts mc = new ManaCosts { CardID = multiverseID, ColorID = FindColor("Red", "R").ID, Quantity = red, CardNumber = cardNumber };
                if (!manaCosts.Contains(mc)) ctxt.ManaCosts.Add(mc);
            }
            
        }



        private DateTime getRulingDate(string r)
        {
            if (String.IsNullOrEmpty(r)) return new DateTime();
            string[] split = r.Split(':');
            if (split.Length < 2) return new DateTime();
            string[] date = split[0].Split('/');
            DateTime dt = new DateTime(date[2].AsOrDefault<Int32>(), date[0].AsOrDefault<Int32>(), date[1].AsOrDefault<Int32>());
            return dt;
        }

        private string getRuling(string r)
        {
            if (String.IsNullOrEmpty(r)) return "";
            string[] s = r.Split(':');
            if (s.Length < 2) return s[0];

            return r.Split(':')[1];
        }

        private void getRulings(HashSet<string> ruling, int row, ExcelWorksheet wksht)
        {
            string unsplit = wksht.Cells[row, 17].Text;
            if (String.IsNullOrEmpty(unsplit)) return;
            string[] rulings = unsplit.Split('£');
            foreach (string s in rulings)
            {
                ruling.Add(s);
            }
        }

        private IEnumerable<Abilities> SeedAbilities(HashSet<string> abilities)
        {
            ctxt.Abilities.AddRange(abilities.Select(f => new Abilities { Ability = f }));
            ctxt.SaveChanges();
            return ctxt.Abilities.ToArray();
        }

        private IEnumerable<Types> SeedTypes(HashSet<string> types)
        {
            ctxt.AddRange(types.Select(t => new Types { Name = t }));
            ctxt.SaveChanges();
            return ctxt.Types.ToArray();

            //HashSet<string> types = new HashSet<string>();

            //ctxt.AddRange(types.Select(s => new Types { Name = s }));
        }

        private IEnumerable<Color> SeedColors(HashSet<string> colors)
        {
            Color[] cols = new Color[]
            {
                new Color{ Symbol="W", Name="White" },
                new Color { Symbol="B", Name="Black"},
                new Color { Symbol="G", Name="Green"},
                new Color { Symbol="R", Name="Red"},
                new Color { Symbol="U", Name="Blue"},
                new Color { Symbol="X", Name="X"},
                new Color { Symbol="E", Name="Energy"}
            };
            ctxt.AddRange(colors.Select(c => new Color { Name = "Uncolored", Symbol=c }));
            ctxt.AddRange(cols);
            ctxt.SaveChanges();
            return ctxt.Color.ToArray();
        }

        private IEnumerable<Sets> SeedSets()
        {
            ctxt.Sets.AddRange(
            new Sets { SetAbbr = "AER", SetFullName = "Aether Revolt" },
            new Sets { SetAbbr = "ARB", SetFullName = "Alara Reborn" },
            new Sets { SetAbbr = "ALL", SetFullName = "Alliances" },
            new Sets { SetAbbr = "AKH", SetFullName = "Amonkhet" },
            new Sets { SetAbbr = "ATQ", SetFullName = "Antiquities" },
            new Sets { SetAbbr = "APC", SetFullName = "Apocalypse" },
            new Sets { SetAbbr = "ARN", SetFullName = "Arabian Nights" },
            new Sets { SetAbbr = "ARC", SetFullName = "Archenemy" },
            new Sets { SetAbbr = "E01", SetFullName = "Archenemy: Nicol Bolas" },
            new Sets { SetAbbr = "AVR", SetFullName = "Avacyn Restored" },
            new Sets { SetAbbr = "BFZ", SetFullName = "Battle for Zendikar" },
            new Sets { SetAbbr = "BRB", SetFullName = "Battle Royale" },
            new Sets { SetAbbr = "BTD", SetFullName = "Beatdown" },
            new Sets { SetAbbr = "BOK", SetFullName = "Betrayers of Kamigawa" },
            new Sets { SetAbbr = "BNG", SetFullName = "Born of the Gods" },
            new Sets { SetAbbr = "CHK", SetFullName = "Champions of Kamigawa" },
            new Sets { SetAbbr = "CHR", SetFullName = "Chronicles" },
            new Sets { SetAbbr = "6ED", SetFullName = "6th Edition" },
            new Sets { SetAbbr = "CSP", SetFullName = "Coldsnap" },
            new Sets { SetAbbr = "C13", SetFullName = "Commander 2013" },
            new Sets { SetAbbr = "C14", SetFullName = "Commander 2014" },
            new Sets { SetAbbr = "C15", SetFullName = "Commander 2015" },
            new Sets { SetAbbr = "C16", SetFullName = "Commander 2016" },
            new Sets { SetAbbr = "C17", SetFullName = "Commander 2017" },
            new Sets { SetAbbr = "CMA", SetFullName = "Commander Anthology" },
            new Sets { SetAbbr = "CM1", SetFullName = "Commander's Arsenal" },
            new Sets { SetAbbr = "CFX", SetFullName = "" },
            new Sets { SetAbbr = "CN2", SetFullName = "Conspiracy: Take the Crown" },
            new Sets { SetAbbr = "DKA", SetFullName = "Dark Ascension" },
            new Sets { SetAbbr = "DST", SetFullName = "Darksteel" },
            new Sets { SetAbbr = "DIS", SetFullName = "Dissension" },
            new Sets { SetAbbr = "DGM", SetFullName = "Dragon's Maze" },
            new Sets { SetAbbr = "DTK", SetFullName = "Dragons of Tarkir" },
            new Sets { SetAbbr = "DD3D", SetFullName = "" },
            new Sets { SetAbbr = "DD3E", SetFullName = "" },
            new Sets { SetAbbr = "DD3G", SetFullName = "" },
            new Sets { SetAbbr = "DD3J", SetFullName = "" },
            new Sets { SetAbbr = "DDH", SetFullName = "Duel Decks: Ajani vs. Nicol Bolas" },
            new Sets { SetAbbr = "DDQ", SetFullName = "Duel Decks: Blessed vs. Cursed" },
            new Sets { SetAbbr = "DDC", SetFullName = "Duel Decks: Divine vs. Demonic" },
            new Sets { SetAbbr = "DDO", SetFullName = "Duel Decks: Elspeth vs. Kiora" },
            new Sets { SetAbbr = "DDF", SetFullName = "Duel Decks: Elspeth vs. Tezzeret" },
            new Sets { SetAbbr = "EVG", SetFullName = "Duel Decks: Elves vs. Goblins" },
            new Sets { SetAbbr = "DDD", SetFullName = "Duel Decks: Garruk vs. Liliana" },
            new Sets { SetAbbr = "DDL", SetFullName = "Duel Decks: Heroes vs. Monsters" },
            new Sets { SetAbbr = "DDJ", SetFullName = "Duel Decks: Izzet vs. Golgari" },
            new Sets { SetAbbr = "DD2", SetFullName = "Duel Decks: Jace vs. Chandra" },
            new Sets { SetAbbr = "DDM", SetFullName = "Duel Decks: Jace vs. Vraska" },
            new Sets { SetAbbr = "DDG", SetFullName = "Duel Decks: Knights vs. Dragons" },
            new Sets { SetAbbr = "DDT", SetFullName = "Duel Decks: Merfolk vs. Goblins" },
            new Sets { SetAbbr = "DDS", SetFullName = "Duel Decks: Mind vs. Might" },
            new Sets { SetAbbr = "DDR", SetFullName = "Duel Decks: Nissa vs. Ob Nixilis" },
            new Sets { SetAbbr = "DDE", SetFullName = "Duel Decks: Phyrexia vs. The Coalition" },
            new Sets { SetAbbr = "DDK", SetFullName = "Duel Decks: Sorin vs. Tibalt" },
            new Sets { SetAbbr = "DDN", SetFullName = "Duel Decks: Speed vs. Cunning" },
            new Sets { SetAbbr = "DDI", SetFullName = "Duel Decks: Venser vs. Koth" },
            new Sets { SetAbbr = "DDP", SetFullName = "Duel Decks: Zendikar vs. Eldrazi" },
            new Sets { SetAbbr = "8ED", SetFullName = "8th Edition" },
            new Sets { SetAbbr = "EMN", SetFullName = "Eldritch Moon" },
            new Sets { SetAbbr = "EMA", SetFullName = "Eternal Masters" },
            new Sets { SetAbbr = "EVE", SetFullName = "Eventide" },
            new Sets { SetAbbr = "EXO", SetFullName = "Exodus" },
            new Sets { SetAbbr = "E02", SetFullName = "Explorers of Ixalan" },
            new Sets { SetAbbr = "FEM", SetFullName = "Fallen Empires" },
            new Sets { SetAbbr = "FRF", SetFullName = "Fate Reforged" },
            new Sets { SetAbbr = "5DN", SetFullName = "Fifth Dawn" },
            new Sets { SetAbbr = "5ED", SetFullName = "5th Edition" },
            new Sets { SetAbbr = "4ED", SetFullName = "4th Edition" },
            new Sets { SetAbbr = "V15", SetFullName = "From the Vault: Angels" },
            new Sets { SetAbbr = "V14", SetFullName = "From the Vault: Annihilation" },
            new Sets { SetAbbr = "DRB", SetFullName = "From the Vault: Dragons" },
            new Sets { SetAbbr = "V09", SetFullName = "From the Vault: Exiled" },
            new Sets { SetAbbr = "V11", SetFullName = "From the Vault: Legends" },
            new Sets { SetAbbr = "V16", SetFullName = "From the Vault: Lore" },
            new Sets { SetAbbr = "V12", SetFullName = "From the Vault: Realms" },
            new Sets { SetAbbr = "V10", SetFullName = "From the Vault: Relics" },
            new Sets { SetAbbr = "V17", SetFullName = "From the Vault: Transform" },
            new Sets { SetAbbr = "V13", SetFullName = "From the Vault: Twenty" },
            new Sets { SetAbbr = "FUT", SetFullName = "Future Sight" },
            new Sets { SetAbbr = "GTC", SetFullName = "Gatecrash" },
            new Sets { SetAbbr = "GPT", SetFullName = "Guildpact" },
            new Sets { SetAbbr = "HLM", SetFullName = "" },
            new Sets { SetAbbr = "HOU", SetFullName = "Hour of Devastation" },
            new Sets { SetAbbr = "ICE", SetFullName = "Ice Age" },
            new Sets { SetAbbr = "IMA", SetFullName = "Iconic Masters" },
            new Sets { SetAbbr = "ISD", SetFullName = "Innistrad" },
            new Sets { SetAbbr = "INV", SetFullName = "Invasion" },
            new Sets { SetAbbr = "XLN", SetFullName = "Ixalan" },
            new Sets { SetAbbr = "JOU", SetFullName = "Journey into Nyx" },
            new Sets { SetAbbr = "JUD", SetFullName = "Judgment" },
            new Sets { SetAbbr = "KLD", SetFullName = "Kaladesh" },
            new Sets { SetAbbr = "KTK", SetFullName = "Khans of Tarkir" },
            new Sets { SetAbbr = "LEG", SetFullName = "Legends" },
            new Sets { SetAbbr = "LGN", SetFullName = "Legions" },
            new Sets { SetAbbr = "LEA", SetFullName = "Alpha (Limited Edition)" },
            new Sets { SetAbbr = "LEB", SetFullName = "Beta (Limited Edition)" },
            new Sets { SetAbbr = "LRW", SetFullName = "Lorwyn" },
            new Sets { SetAbbr = "M10", SetFullName = "Magic 2010" },
            new Sets { SetAbbr = "M11", SetFullName = "Magic 2011" },
            new Sets { SetAbbr = "M12", SetFullName = "Magic 2012" },
            new Sets { SetAbbr = "M13", SetFullName = "Magic 2013" },
            new Sets { SetAbbr = "M14", SetFullName = "Magic 2014" },
            new Sets { SetAbbr = "M15", SetFullName = "Magic 2015" },
            new Sets { SetAbbr = "ORI", SetFullName = "Magic Origins" },
            new Sets { SetAbbr = "CMD", SetFullName = "Commander (2011)" },
            new Sets { SetAbbr = "CNS", SetFullName = "Conspiracy" },
            new Sets { SetAbbr = "MPSAKH", SetFullName = "" },
            new Sets { SetAbbr = "MPSKLD", SetFullName = "" },
            new Sets { SetAbbr = "MED", SetFullName = "Masters Edition" },
            new Sets { SetAbbr = "ME2", SetFullName = "Masters Edition II" },
            new Sets { SetAbbr = "ME3", SetFullName = "Masters Edition III" },
            new Sets { SetAbbr = "ME4", SetFullName = "Masters Edition IV" },
            new Sets { SetAbbr = "MMQ", SetFullName = "Mercadian Masques" },
            new Sets { SetAbbr = "MIR", SetFullName = "Mirage" },
            new Sets { SetAbbr = "MRD", SetFullName = "Mirrodin" },
            new Sets { SetAbbr = "MBS", SetFullName = "Mirrodin Besieged" },
            new Sets { SetAbbr = "MD1", SetFullName = "Modern Event Deck" },
            new Sets { SetAbbr = "MMA", SetFullName = "Modern Masters (2013)" },
            new Sets { SetAbbr = "MM2", SetFullName = "Modern Masters 2015" },
            new Sets { SetAbbr = "MM3", SetFullName = "Modern Masters 2017" },
            new Sets { SetAbbr = "MOR", SetFullName = "Morningtide" },
            new Sets { SetAbbr = "NMS", SetFullName = "" },
            new Sets { SetAbbr = "NPH", SetFullName = "New Phyrexia" },
            new Sets { SetAbbr = "9ED", SetFullName = "9th Edition" },
            new Sets { SetAbbr = "OGW", SetFullName = "Oath of the Gatewatch" },
            new Sets { SetAbbr = "ODY", SetFullName = "Odyssey" },
            new Sets { SetAbbr = "ONS", SetFullName = "Onslaught" },
            new Sets { SetAbbr = "PLC", SetFullName = "Planar Chaos" },
            new Sets { SetAbbr = "HOP", SetFullName = "Planechase (2009)" },
            new Sets { SetAbbr = "PC2", SetFullName = "Planechase 2012" },
            new Sets { SetAbbr = "PCA", SetFullName = "Planechase Anthology" },
            new Sets { SetAbbr = "PLS", SetFullName = "Planeshift" },
            new Sets { SetAbbr = "POR", SetFullName = "Portal" },

            new Sets { SetAbbr = "PO2", SetFullName = "Portal Second Age" },

            new Sets { SetAbbr = "PTK", SetFullName = "Portal Three Kingdoms" },
            new Sets { SetAbbr = "PD2", SetFullName = "Premium Deck Series: Fire & Lightning" },
            new Sets { SetAbbr = "PD3", SetFullName = "Premium Deck Series: Graveborn" },
            new Sets { SetAbbr = "H09", SetFullName = "Premium Deck Series: Slivers" },
            new Sets { SetAbbr = "PCY", SetFullName = "Prophecy" },
            new Sets { SetAbbr = "RAV", SetFullName = "Ravnica: City of Guilds" },
            new Sets { SetAbbr = "RTR", SetFullName = "Return to Ravnica" },
            new Sets { SetAbbr = "3ED", SetFullName = "Revised Edition" },
            new Sets { SetAbbr = "ROE", SetFullName = "Rise of the Eldrazi" },
            new Sets { SetAbbr = "RIX", SetFullName = "Rivals of Ixalan" },
            new Sets { SetAbbr = "SOK", SetFullName = "Saviors of Kamigawa" },
            new Sets { SetAbbr = "SOM", SetFullName = "Scars of Mirrodin" },
            new Sets { SetAbbr = "SCG", SetFullName = "Scourge" },
            new Sets { SetAbbr = "7ED", SetFullName = "7th Edition" },
            new Sets { SetAbbr = "SHM", SetFullName = "Shadowmoor" },
            new Sets { SetAbbr = "SOI", SetFullName = "Shadows over Innistrad" },
            new Sets { SetAbbr = "ALA", SetFullName = "Shards of Alara" },
            new Sets { SetAbbr = "S99", SetFullName = "Starter 1999" },
            new Sets { SetAbbr = "S00", SetFullName = "Starter 2000" },
            new Sets { SetAbbr = "STH", SetFullName = "Stronghold" },
            new Sets { SetAbbr = "TMP", SetFullName = "Tempest" },
            new Sets { SetAbbr = "TPR", SetFullName = "Tempest Remastered" },
            new Sets { SetAbbr = "10E", SetFullName = "10th Edition" },
            new Sets { SetAbbr = "DRK", SetFullName = "The Dark" },
            new Sets { SetAbbr = "THS", SetFullName = "Theros" },
            new Sets { SetAbbr = "TSP", SetFullName = "Time Spiral" },
            new Sets { SetAbbr = "TSB", SetFullName = "Time Spiral" },
            new Sets { SetAbbr = "TOR", SetFullName = "Torment" },
            new Sets { SetAbbr = "UGIN", SetFullName = "" },
            new Sets { SetAbbr = "UGL", SetFullName = "Unglued" },
            new Sets { SetAbbr = "UNH", SetFullName = "Unhinged" },
            new Sets { SetAbbr = "2ED", SetFullName = "Unlimited Edition" },
            new Sets { SetAbbr = "UST", SetFullName = "Unstable" },
            new Sets { SetAbbr = "UDS", SetFullName = "Urza's Destiny" },
            new Sets { SetAbbr = "UDL", SetFullName = "" },
            new Sets { SetAbbr = "VMA", SetFullName = "Vintage Masters" },
            new Sets { SetAbbr = "VIS", SetFullName = "Visions" },
            new Sets { SetAbbr = "WTH", SetFullName = "Weatherlight" },
            new Sets { SetAbbr = "W16", SetFullName = "Welcome Deck 2016" },
            new Sets { SetAbbr = "W17", SetFullName = "Welcome Deck 2017" },
            new Sets { SetAbbr = "WWK", SetFullName = "Worldwake" },
            new Sets { SetAbbr = "ZEN", SetFullName = "Zendikar" },
            new Sets { SetAbbr = "MPSZEN", SetFullName = "" },
            new Sets { SetAbbr = "ARCS", SetFullName = "" },
            new Sets { SetAbbr = "HOPP", SetFullName = "" },
            new Sets { SetAbbr = "PC2P", SetFullName = "" },
            new Sets { SetAbbr = "PCAP", SetFullName = "" },
            new Sets { SetAbbr = "CNSC", SetFullName = "" },
            new Sets { SetAbbr = "CN2C", SetFullName = "" },
            new Sets { SetAbbr = "E01S", SetFullName = "" },
            new Sets { SetAbbr = "15A", SetFullName = "" },
            new Sets { SetAbbr = "GPX", SetFullName = "" },
            new Sets { SetAbbr = "PRO", SetFullName = "" },
            new Sets { SetAbbr = "WRL", SetFullName = "" },
            new Sets { SetAbbr = "WMC", SetFullName = "" },
            new Sets { SetAbbr = "SUM", SetFullName = "" },
            new Sets { SetAbbr = "CHA", SetFullName = "" },
            new Sets { SetAbbr = "2HG", SetFullName = "" },
            new Sets { SetAbbr = "ARENA", SetFullName = "" },
            new Sets { SetAbbr = "FNM", SetFullName = "" },
            new Sets { SetAbbr = "MPRP", SetFullName = "" },
            new Sets { SetAbbr = "SUS", SetFullName = "" },
            new Sets { SetAbbr = "HHO", SetFullName = "" },
            new Sets { SetAbbr = "JGP", SetFullName = "" },
            new Sets { SetAbbr = "EURO", SetFullName = "" },
            new Sets { SetAbbr = "GUR", SetFullName = "" },
            new Sets { SetAbbr = "APAC", SetFullName = "" },
            new Sets { SetAbbr = "UQC", SetFullName = "" },
            new Sets { SetAbbr = "DCILM", SetFullName = "" },
            new Sets { SetAbbr = "THP", SetFullName = "" },
            new Sets { SetAbbr = "TFTH", SetFullName = "" },
            new Sets { SetAbbr = "TBTH", SetFullName = "" },
            new Sets { SetAbbr = "TDAG", SetFullName = "" },
            new Sets { SetAbbr = "RLS", SetFullName = "" },
            new Sets { SetAbbr = "HTR", SetFullName = "" },
            new Sets { SetAbbr = "PZ1", SetFullName = "Legendary Cube" },
            new Sets { SetAbbr = "PZ2", SetFullName = "Treasure Chests" },
            new Sets { SetAbbr = "WPN", SetFullName = "" },
            new Sets { SetAbbr = "GTW", SetFullName = "" },
            new Sets { SetAbbr = "MPS", SetFullName = "Kaladesh Inventions" },
            new Sets { SetAbbr = "MDI", SetFullName = "" },
            new Sets { SetAbbr = "CST", SetFullName = "" },
            new Sets { SetAbbr = "ATH", SetFullName = "Anthologies" },
            new Sets { SetAbbr = "DPW", SetFullName = "" },
            new Sets { SetAbbr = "8EDB", SetFullName = "" },
            new Sets { SetAbbr = "9EDB", SetFullName = "" },
            new Sets { SetAbbr = "DKM", SetFullName = "Deckmasters 2001" },
            new Sets { SetAbbr = "OVNC", SetFullName = "" },
            new Sets { SetAbbr = "OLGC", SetFullName = "" },
            new Sets { SetAbbr = "PXTC", SetFullName = "" }
            );
            ctxt.SaveChanges();
            return ctxt.Sets.ToArray();
        }
    }
}
