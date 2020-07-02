using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SeasonTest.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeasonTest.Data
{
    internal class SeasonTestContext : DbContext
    {
        // dotnet ef migrations add AddSeasons --project ..\SeasonTest.Data
        // dotnet ef database update --project ..\SeasonTest.Data

        //private readonly IConfiguration configuration;
        public SeasonTestContext(/*IConfiguration configuration*/)
        {
            //this.configuration = configuration;
        }

        internal DbSet<Season> Seasons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.UseSqlServer(configuration.GetConnectionString("SeasonTest"), optionsBuilder => optionsBuilder.MigrationsAssembly("SeasonTest.Data"));
            var connectionString = @"Server=(localdb)\mssqllocaldb;Database=SeasonTest;Trusted_Connection=True;ConnectRetryCount=0";
            optionsBuilder.UseSqlServer(connectionString, optionsBuilder => optionsBuilder.MigrationsAssembly("SeasonTest.Data"));
        }
    }
}
