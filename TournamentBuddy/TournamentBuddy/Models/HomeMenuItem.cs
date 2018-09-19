using System;
using System.Collections.Generic;
using System.Text;

namespace TournamentBuddy.Models
{
    public enum MenuItemType
    {
        Matches,
        Teams
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
