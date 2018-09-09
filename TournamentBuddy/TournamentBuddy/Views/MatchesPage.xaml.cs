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
        //List<MatchItem> matchList;
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

        async void GetMatches()
        {
            List<MatchItem> matchList = await App.Database.GetAgeGroupAsync(agePicker.SelectedItem.ToString());
            matchCollection = new ObservableCollection<MatchItem>(matchList);
            matchListView.ItemsSource = matchCollection;
        }        

        void Handle_AgeSelectedIndexChanged(object sender, System.EventArgs e)
        {
            GetMatches();
        }

        async private void RefreshList()
        {            
            await App.Database.DeleteAgeGroup(agePicker.SelectedItem.ToString());
            App.Database.ScrapeMatches(agePicker.SelectedItem.ToString());
            GetMatches();
        }
    }
}