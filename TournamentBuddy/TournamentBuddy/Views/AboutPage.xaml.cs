using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TournamentBuddy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        async void Push_Clicked(object sender, EventArgs e)
        {
            MatchItem Match1 = new MatchItem
            {
                AgeGroup = "Agegroup",
                Bracket = "Bracket",
                Date = "Date",
                Time = "Time",
                HomeTeam = "HomeTeam",
                HomeScore = "HomeScore",
                AwayScore = "AwayScore",
                AwayTeam = "AwayTeam",
                Location = "Location"
            };

            await App.Database.SaveItemAsync(Match1);
        }

        async void Retrieve_Clicked(object sender, EventArgs e)
        {
            List<MatchItem> tournament1 = await App.Database.GetItemsAsync();

            ObservableCollection<MatchItem> matches = new ObservableCollection<MatchItem>(tournament1);

            listView.ItemsSource = matches;
        }

        async void Delete_Clicked(object sender, EventArgs e)
        {
            await App.Database.DeleteAll();
        }
    }
}