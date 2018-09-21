using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TournamentBuddy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TeamsPage : ContentPage
	{
		public TeamsPage ()
		{
			InitializeComponent ();

            matchListView.RefreshCommand = new Command(() => {
                matchListView.IsRefreshing = true;
                RefreshList();
                matchListView.IsRefreshing = false;
            });
        }

        async void GetTeams()
        {
            List<MatchItem> matchList = await App.Database.GetAgeGroupAsync(agePicker.SelectedItem.ToString());
            if (matchList.Count == 0)
            {
                App.Database.ScrapeMatches(agePicker.SelectedItem.ToString());
                matchList = await App.Database.GetAgeGroupAsync(agePicker.SelectedItem.ToString());
            }
            List<string> teamList = new List<string>();

            for(int i = 0; i < matchList.Count; i++)
            {
                if(teamList.Contains(matchList[i].HomeTeam) == false)
                {
                    teamList.Add(matchList[i].HomeTeam);
                }

                if (teamList.Contains(matchList[i].AwayTeam) == false)
                {
                    teamList.Add(matchList[i].AwayTeam);
                }
            }

            teamPicker.ItemsSource = teamList;
        }

        async void DisplayMatches(string team)
        {
            List<MatchItem> matchList = await App.Database.GetTeamAsync(team);
            matchListView.ItemsSource = matchList;
        }

        void Handle_AgeSelectedIndexChanged(object sender, System.EventArgs e)
        {
            teamPicker.SelectedIndex = -1;
            GetTeams();
        }

        void Handle_TeamSelectedIndexChanged(object sender, System.EventArgs e)
        {
            if(teamPicker.SelectedIndex != -1)
            {
                DisplayMatches(teamPicker.SelectedItem.ToString());
            }
            else
            {
                matchListView.ItemsSource = null;
            }
        }

        /*private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }*/

        async void RefreshList()
        {
            int selectedIndex = teamPicker.SelectedIndex;

            await App.Database.DeleteAgeGroup(agePicker.SelectedItem.ToString());
            App.Database.ScrapeMatches(agePicker.SelectedItem.ToString());
            GetTeams();

            teamPicker.SelectedIndex = selectedIndex;
        }

        /*public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;

                    RefreshData();

                    IsRefreshing = false;
                });
            }
        }*/
    }
}