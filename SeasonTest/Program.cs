using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MintPlayer.SeasonCheck;
using SeasonTest.Data.Extensions;
using SeasonTest.Data.Repositories;
using System;

namespace SeasonTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //// Create configuration
            //var configuration = new ConfigurationBuilder()
            //    .AddJsonFile("appsettings.json")
            //    .Build();

            // Create service provider
            var serviceProvider = new ServiceCollection()
                //.AddSingleton<IConfiguration>(configuration)
                .AddSeasonChecker()
                .AddSeasonTest(options =>
                {
                    //options.ConnectionString = configuration.GetConnectionString("SeasonTest");
                    options.ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=SeasonTest;Trusted_Connection=True;ConnectRetryCount=0";
                })
                .BuildServiceProvider();

            #region Snippet 1

            //// Get the season checker from the service-container
            //var seasonChecker = serviceProvider.GetService<ISeasonChecker>();

            //// Run the test for all days of the year
            //var testStart = new DateTime(2020, 1, 1);
            //var testDays = 366;
            //for (int i = 0; i < testDays; i++)
            //{
            //    var day = testStart.AddDays(i);
            //    var season = seasonChecker.FindSeasonAsync(seasons, day).Result;
            //    Console.WriteLine("{0:dd/MM/yyyy} is in the {1}", day, season.Name);
            //}
            //Console.ReadKey();

            #endregion

            #region Snippet 2
            
            // Get the repository from the service-container
            var seasonRepository = serviceProvider.GetService<ISeasonRepository>();

            // Run the test for all days of the year
            var testStart = new DateTime(2020, 1, 1);
            var testDays = 366;
            for (int i = 0; i < testDays; i++)
            {
                var day = testStart.AddDays(i);
                var season = seasonRepository.GetSeasonForDate(day).Result;
                Console.WriteLine("{0:dd/MM/yyyy} is in the {1}", day, season.Name);
            }
            Console.ReadKey();

            #endregion
        }
    }
}
