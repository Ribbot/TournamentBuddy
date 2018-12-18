using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace TournamentBuddy.ViewModels
{
    public class MatchesViewModel : BaseViewModel
    {
        public ObservableCollection<MatchItem> Matches { get; set; }


        public MatchesViewModel()
        {
            Title = "Matches";
            Matches = new ObservableCollection<MatchItem>();
        }
    }
}
