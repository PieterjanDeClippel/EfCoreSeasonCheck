using MintPlayer.SeasonCheck;
using SeasonTest.Data.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeasonTest.Data.Repositories
{
    public interface ISeasonRepository
    {
        IEnumerable<Season> GetSeasons();
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

        public IEnumerable<Season> GetSeasons()
        {
            return seasonTestContext.Seasons
                .Select(s => ToDto(s));
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
        internal static Entities.Season ToEntity(Season season)
        {
            if (season == null) return null;
            return new Entities.Season
            {
                Id = season.Id,
                Name = season.Name,
                Start = season.Start,
                End = season.End
            };
        }
        #endregion
    }
}
