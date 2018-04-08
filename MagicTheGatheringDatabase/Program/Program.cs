using Microsoft.EntityFrameworkCore;
using System;

namespace MagicDbContext
{
    class Program
    {
        static void Main(string[] args)
        {
            DbContextOptionsBuilder<MagicContext> dbContextOptions = new DbContextOptionsBuilder<MagicContext>();

            dbContextOptions.UseSqlServer("Server=localhost\\SQLEXPRESS; Database=MagicTG; Trusted_Connection=True;", providerOptions => providerOptions.CommandTimeout(60));

            MagicContext ctx = new MagicContext(dbContextOptions.Options);
            ctx.Database.EnsureCreated();
        }
    }
}
