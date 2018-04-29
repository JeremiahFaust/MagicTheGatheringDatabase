using Microsoft.EntityFrameworkCore;
using System;

namespace MagicDbContext
{
    class Program
    {
        static void Main(string[] args)
        {
            DbContextOptionsBuilder<MagicContext> dbContextOptions = new DbContextOptionsBuilder<MagicContext>();

            dbContextOptions.UseSqlServer("Data Source=localhost\\SQLEXPRESS; initial catalog=MagicTG; user=JD; password=7a521039", providerOptions => providerOptions.CommandTimeout(60));

            MagicContext ctx = new MagicContext(dbContextOptions.Options);
            ctx.Database.EnsureCreated();
        }
    }
}
