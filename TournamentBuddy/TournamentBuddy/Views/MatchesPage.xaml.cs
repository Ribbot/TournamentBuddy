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

            App.Database.DeleteAll();
        }

        async void GetMatches()
        {
            App.Database.ScrapeMatches(agePicker.SelectedItem.ToString());
            matchList = await App.Database.GetAgeGroupAsync(agePicker.SelectedItem.ToString());
            ObservableCollection<MatchItem> matchCollection = new ObservableCollection<MatchItem>(matchList);
            listView.ItemsSource = matchCollection;
        }        

        void Handle_AgeSelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (matchList != null)
            {
                App.Database.DeleteList(matchList);
            }

            GetMatches();
        }
    }
}