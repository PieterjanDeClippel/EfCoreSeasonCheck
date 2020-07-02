using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SeasonTest.Data.Options;
using SeasonTest.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeasonTest.Data.Extensions
{
    public static class SeasonTestExtensions
    {
        public static IServiceCollection AddSeasonTest(this IServiceCollection services, Action<SeasonTestOptions> options)
        {
            var opt = new SeasonTestOptions();
            options(opt);

            return services
                .AddDbContext<SeasonTestContext>(/*options =>
                {
                    options.UseSqlServer(opt.ConnectionString);
                }*/)
                .AddScoped<ISeasonRepository, SeasonRepository>();
        }
    }
}
