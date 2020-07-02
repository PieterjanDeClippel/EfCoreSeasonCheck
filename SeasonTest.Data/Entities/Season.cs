using MintPlayer.SeasonCheck;
using System;

namespace SeasonTest.Data.Entities
{
    internal class Season : ISeason
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
