using MagicDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

namespace MagicMigrator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("StartTime: " + DateTime.Now);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            DbContextOptionsBuilder<MagicContext> dbContextOptions = new DbContextOptionsBuilder<MagicContext>();

            dbContextOptions.EnableSensitiveDataLogging();
            dbContextOptions.UseSqlServer("Data Source=localhost\\SQLEXPRESS; initial catalog=MagicTG; Trusted_Connection=True", providerOptions => providerOptions.CommandTimeout(60));

            MagicContext ctxt = new MagicContext(dbContextOptions.Options);

            //DbTestInitializer db = new DbTestInitializer();
            DbInitializer db = new DbInitializer(ctxt);
            sw.Stop();
            Console.WriteLine("CompletionTime: " + DateTime.Now);
            //Console.ReadLine();

            Console.WriteLine("Completed");
            Console.WriteLine(sw.Elapsed);
            Console.ReadLine();
        }
    }
}
