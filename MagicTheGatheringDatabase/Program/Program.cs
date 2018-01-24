﻿using Microsoft.EntityFrameworkCore;
using System;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            DbContextOptionsBuilder<MagicContext> dbContextOptions = new DbContextOptionsBuilder<MagicContext>();

            dbContextOptions.UseSqlite("Data Source=MagicDB.db", providerOptions => providerOptions.CommandTimeout(60));

            MagicContext ctx = new MagicContext(dbContextOptions.Options);
        }
    }
}