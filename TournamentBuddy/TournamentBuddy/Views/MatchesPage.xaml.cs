using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TournamentBuddy.Models;
using TournamentBuddy.ViewModels;
using System.Collections.ObjectModel;

namespace TournamentBuddy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MatchesPage : ContentPage
	{
        ObservableCollection<MatchItem> matchCollection;

        public MatchesPage ()
		{
			InitializeComponent ();

            App.Database.DeleteAll();

            matchListView.RefreshCommand = new Command(() => {
                RefreshList();
                matchListView.IsRefreshing = false;
            });
        }
	
	//Retrieves all matches associated with the selected age group from the database.
	//Display the list of matches.
        async void GetMatches()
        {
            List<MatchItem> matchList = await App.Database.GetAgeGroupAsync(agePicker.SelectedItem.ToString());
            if (matchList.Count == 0)
            {
                App.Database.ScrapeMatches(agePicker.SelectedItem.ToString());
                matchList = await App.Database.GetAgeGroupAsync(agePicker.SelectedItem.ToString());
            }

            matchCollection = new ObservableCollection<MatchItem>(matchList);
            matchListView.ItemsSource = matchCollection;
        }        

	//Retrieves and displays the matches associated with an age group whenever a new age group is selected
        void Handle_AgeSelectedIndexChanged(object sender, System.EventArgs e)
        {
            GetMatches();
        }

	//Removes any matches associated with the selected age group from the database.
	//Re-downloads the age group's web page, store the matches in the database, and display the matches.
        async private void RefreshList()
        {            
            await App.Database.DeleteAgeGroup(agePicker.SelectedItem.ToString());
            App.Database.ScrapeMatches(agePicker.SelectedItem.ToString());
            GetMatches();
        }
    }
}
