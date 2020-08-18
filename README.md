# EfCoreSeasonCheck
Get the season for a specific date, with seasons coming from a DbContext

## About this demo
This project demonstrates how you can check to which season a certain date belongs.

The project uses the MintPlayer.SeasonCheck NuGet package:
- NuGet: https://www.nuget.org/packages/MintPlayer.SeasonChecker
- GitHub: https://github.com/MintPlayer/MintPlayer.SeasonChecker

This package is capable of finding out what season a certain date belongs, with the Seasons as an input parameter.
The seasons you pass to the function can also be sourced from a DbContext, while not yet enumerated.
The package will append the right LINQ query to the DbSet<Season> and return the correct season for the date.
This project is a POC to proof that the appended LINQ query can in fact be entirely converted to SQL.

## Preparation
To create the database, run the following commands:

    cd .\SeasonTest
    dotnet ef database update --project ..\SeasonTest.Data

You can now put seasons in the database.

| Id | Name   | Start      | End        |
|----|--------|------------|------------|
|  1 | Spring | 21/03/2000 | 20/06/2000 |
|  2 | Summer | 21/06/2000 | 20/09/2000 |
|  3 | Automn | 21/09/2000 | 20/12/2000 |
|  4 | Winter | 21/12/2000 | 20/03/2001 |

## What's in the box
### DbContext

    internal class SeasonTestContext : DbContext
    {
        // dotnet ef database update --project ..\SeasonTest.Data
        public SeasonTestContext()
        {
        }

        internal DbSet<Season> Seasons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var connectionString = @"Server=(localdb)\mssqllocaldb;Database=SeasonTest;Trusted_Connection=True;ConnectRetryCount=0";
            optionsBuilder.UseSqlServer(connectionString, optionsBuilder => optionsBuilder.MigrationsAssembly("SeasonTest.Data"));
        }
    }

### Repository

    public interface ISeasonRepository
    {
        Task<Season> GetSeasonForDate(DateTime date);
    }
    internal class SeasonRepository : ISeasonRepository
    {
        private readonly ISeasonChecker seasonChecker;
        private readonly SeasonTestContext seasonTestContext;
        public SeasonRepository(ISeasonChecker seasonChecker, SeasonTestContext seasonTestContext)
        {
            this.seasonChecker = seasonChecker;
            this.seasonTestContext = seasonTestContext;
        }

        public async Task<Season> GetSeasonForDate(DateTime date)
        {
            var season = await seasonChecker.FindSeasonAsync(seasonTestContext.Seasons, date);
            return ToDto(season);
        }

        #region Conversion methods
        internal static Season ToDto(Entities.Season season)
        {
            if (season == null) return null;
            return new Season
            {
                Id = season.Id,
                Name = season.Name,
                Start = season.Start,
                End = season.End
            };
        }
        #endregion
    }

### Entity

    internal class Season : ISeason
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

### Dto

    public class Season
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

### Program

    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSeasonChecker()
                .AddSeasonTest(options =>
                {
                    options.ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=SeasonTest;Trusted_Connection=True;ConnectRetryCount=0";
                })
                .BuildServiceProvider();

            var seasonRepository = serviceProvider.GetService<ISeasonRepository>();
            
            var testStart = new DateTime(2020, 1, 1);
            var testDays = 366;
            for (int i = 0; i < testDays; i++)
            {
                var day = testStart.AddDays(i);
                var season = seasonRepository.GetSeasonForDate(day).Result;
                Console.WriteLine("{0:dd/MM/yyyy} is in the {1}", day, season.Name);
            }
            Console.ReadKey();
        }
    }
