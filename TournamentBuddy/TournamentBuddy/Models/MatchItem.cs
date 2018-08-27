using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TournamentBuddy
{
    public class MatchItem
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public string AgeGroup { get; set; }
        public string Bracket { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string HomeTeam { get; set; }
        public string HomeScore { get; set; }
        public string AwayScore { get; set; }
        public string AwayTeam { get; set; }
        public string Location { get; set; }
    }
}
