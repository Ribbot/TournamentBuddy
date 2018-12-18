using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TournamentBuddy.Views;
using System.IO;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TournamentBuddy
{
    public partial class App : Application
    {
        static MatchDatabase database;

        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        //Initialize SQlite database
        public static MatchDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new MatchDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MatchSQLite.db3"));
                }
                return database;
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
