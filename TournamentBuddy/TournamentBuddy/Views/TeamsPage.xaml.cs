﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            //GetTeams();
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

            teamPicker.SelectedIndex = 0;
            teamPicker.ItemsSource = teamList;
            //teamPicker.SelectedIndex = 0;
        }

        async void DisplayMatches(string team)
        {
            List<MatchItem> matchList = await App.Database.GetTeamAsync(team);
            matchListView.ItemsSource = matchList;
        }

        void Handle_AgeSelectedIndexChanged(object sender, System.EventArgs e)
        {
            GetTeams();
            //DisplayMatches();
        }

        void Handle_TeamSelectedIndexChanged(object sender, System.EventArgs e)
        {
            DisplayMatches((sender as Picker).SelectedItem.ToString());
        }
    }
}