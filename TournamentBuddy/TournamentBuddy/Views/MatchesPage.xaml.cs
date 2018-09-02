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
        List<MatchItem> matchList;

        public MatchesPage ()
		{
			InitializeComponent ();

            GetMatches();
        }

        async void GetMatches()
        {
            App.Database.ScrapeMatches(agePicker.SelectedItem.ToString());
            matchList = await App.Database.GetAgeGroupAsync(agePicker.SelectedItem.ToString());
            ObservableCollection<MatchItem> matchCollection = new ObservableCollection<MatchItem>(matchList);
            listView.ItemsSource = matchCollection;
        }        

        void Retrieve_Clicked(object sender, EventArgs e)
        {
            GetMatches();
        }

        async void Delete_Clicked(object sender, EventArgs e)
        {
            await App.Database.DeleteAll();
        }

        void Handle_AgeSelectedIndexChanged(object sender, System.EventArgs e)
        {
            //App.Database.DeleteList(matchList);
            App.Database.ScrapeMatches(agePicker.SelectedItem.ToString());
            GetMatches();
        }
    }
}