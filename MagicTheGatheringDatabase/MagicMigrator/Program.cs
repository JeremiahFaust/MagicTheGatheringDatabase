using MagicDbContext;
using Microsoft.EntityFrameworkCore;
using System;

namespace MagicMigrator
{
    class Program
    {
        static void Main(string[] args)
        {

            DbContextOptionsBuilder<MagicContext> dbContextOptions = new DbContextOptionsBuilder<MagicContext>();

            dbContextOptions.EnableSensitiveDataLogging();
            dbContextOptions.UseSqlite("Data Source=MagicDB.db", providerOptions => providerOptions.CommandTimeout(60));

            MagicContext ctxt = new MagicContext(dbContextOptions.Options);

            //DbTestInitializer db = new DbTestInitializer();
            DbInitializer db = new DbInitializer(ctxt);
        }
    }
}
