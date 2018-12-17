using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using SQLite;

namespace TournamentBuddy
{
    public class MatchDatabase
    {
        readonly SQLiteAsyncConnection database;

        public MatchDatabase(string dbpath)
        {
            database = new SQLiteAsyncConnection(dbpath);
            database.CreateTableAsync<MatchItem>().Wait();
        }
        
        //Returns all MatchItems stored in the database
        public Task<List<MatchItem>> GetItemsAsync()
        {
            return database.Table<MatchItem>().ToListAsync();
        }

        //Returns all MatchItems stored in the database
        public Task<List<MatchItem>> GetItemsNotDoneAsync()
        {
            return database.QueryAsync<MatchItem>("SELECT * FROM [MatchItem] WHERE [Done] = 0");
        }

        //Returns the first MatchItem in the database with the given ID
        public Task<MatchItem> GetItemAsync(int id)
        {
            return database.Table<MatchItem>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        //Stores a single MatchItem in the database
        public Task<int> SaveItemAsync(MatchItem item)
        {
            if (item.Id != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        //Returns a list of MatchItems that are associated with the given age group
        public Task<List<MatchItem>> GetAgeGroupAsync(string ageGroup)
        {
            return database.Table<MatchItem>().Where(i => i.AgeGroup == ageGroup).ToListAsync();
        }

        //Returns a list of MatchItems that are associated with the given team
        public Task<List<MatchItem>> GetTeamAsync(string team)
        {
            return database.Table<MatchItem>().Where(i => (i.HomeTeam == team) || (i.AwayTeam == team)).ToListAsync();
        }

        //Deletes a given MatchItem from the database
        public Task<int> DeleteItemAsync(MatchItem item)
        {
            return database.DeleteAsync(item);
        }

        //Deletes all MatchItems from the database
        public Task<int> DeleteAll()
        {
            return database.ExecuteAsync("DELETE FROM MatchItem");
        }

        //Deletes all MatchItems assocated with the given age group from the database
        public Task<int> DeleteAgeGroup(string ageGroup)
        {
            return database.ExecuteAsync("DELETE FROM MatchItem WHERE AgeGroup = '" + ageGroup + "'");
        }

        //Deletes a given list of MatchItems from the database
        async public void DeleteList(List<MatchItem> matchList)
        {
            for (int i = 0; i < matchList.Count; i++)
            {
                await DeleteItemAsync(matchList[i]);
            }
        }

        //Returns the URL for the webpage containing the match information of a given age group
        private string AgeGroupToURL(string ageGroup)
        {
            string url = null;

            switch (ageGroup)
            {
                case "Boys U10 Cascade":
                    url = "http://events.gotsport.com/events/schedule.aspx?EventID=64302&GroupID=747243&ApplicationID=&print=true";
                    break;

                case "Boys U10 Siskiyou":
                    url = "http://events.gotsport.com/events/schedule.aspx?EventID=64302&GroupID=747244&ApplicationID=&print=true";
                    break;

                case "Boys U10 Sierra":
                    url = "http://events.gotsport.com/events/schedule.aspx?EventID=64302&GroupID=747245&ApplicationID=&print=true";
                    break;

                case "Boys U11 Cascade":
                    url = "http://events.gotsport.com/events/schedule.aspx?EventID=64302&GroupID=747246&ApplicationID=&print=true";
                    break;

                case "Boys U11 Siskiyou":
                    url = "http://events.gotsport.com/events/schedule.aspx?EventID=64302&GroupID=747247&ApplicationID=&print=true";
                    break;

                case "Boys U12 Cascade/Siskiyou":
                    url = "http://events.gotsport.com/events/schedule.aspx?EventID=64302&GroupID=747248&ApplicationID=&print=true";
                    break;

                case "Boys U12 Sierra":
                    url = "http://events.gotsport.com/events/schedule.aspx?EventID=64302&GroupID=747249&ApplicationID=&print=true";
                    break;

                case "Boys U13 Cascade":
                    url = "http://events.gotsport.com/events/schedule.aspx?EventID=64302&GroupID=747252&ApplicationID=&print=true";
                    break;

                case "Boys U13 Siskiyou/Sierra":
                    url = "http://events.gotsport.com/events/schedule.aspx?EventID=64302&GroupID=747253&ApplicationID=&print=true";
                    break;

                case "Boys U14 Boys U14":
                    url = "http://events.gotsport.com/events/schedule.aspx?EventID=64302&GroupID=747254&ApplicationID=&print=true";
                    break;

                case "Boys U15 Cascade":
                    url = "http://events.gotsport.com/events/schedule.aspx?EventID=64302&GroupID=747255&ApplicationID=&print=true";
                    break;

                case "Boys U15 Siskiyou/Sierra":
                    url = "http://events.gotsport.com/events/schedule.aspx?EventID=64302&GroupID=747256&ApplicationID=&print=true";
                    break;

                case "Boys U16 Boys U16":
                    url = "http://events.gotsport.com/events/schedule.aspx?EventID=64302&GroupID=747257&ApplicationID=&print=true";
                    break;

                case "Boys U17 Boys U17":
                    url = "http://events.gotsport.com/events/schedule.aspx?EventID=64302&GroupID=747258&ApplicationID=&print=true";
                    break;
            }

            return url;
        }

        //Scrapes the match data from the webpage associated with a given age group. 
        //Creates a new MatchItem for each match detected the webpage and stores it in the database.
        public void ScrapeMatches(string AgeGroup)
        {
            string url = AgeGroupToURL(AgeGroup);

            var web = new HtmlWeb();
            var doc = web.Load(url);

            string ageGroup = doc.DocumentNode.SelectSingleNode("//*[@id=\"aspnetForm\"]/table/tr/td[2]/table/tr/td/div[1]/div/div[3]").InnerHtml;
            string bracket = doc.DocumentNode.SelectSingleNode("//*[@id=\"aspnetForm\"]/table/tr/td[2]/table/tr/td/div[1]/div/h3[1]").InnerText;

            int numDates = doc.DocumentNode.SelectNodes("//*[@id=\"aspnetForm\"]/table/tr/td[2]/table/tr/td/div[1]/div/table").Count;
            for (int i = 1; i <= numDates; i++)
            {
                string date = doc.DocumentNode.SelectSingleNode("//*[@id=\"aspnetForm\"]/table/tr/td[2]/table/tr/td/div[1]/div/table[" + i + "]/tr[1]/th").InnerText;

                int numMatches = doc.DocumentNode.SelectNodes("//*[@id=\"aspnetForm\"]/table/tr/td[2]/table/tr/td/div[1]/div/table[" + i + "]/tr").Count - 2;
                for (int j = 3; j < (numMatches + 3); j++)
                {
                    var newMatch = new MatchItem
                    {
                        AgeGroup = ageGroup,
                        Bracket = bracket,
                        Date = date,
                        Time = doc.DocumentNode.SelectSingleNode("//*[@id=\"aspnetForm\"]/table/tr/td[2]/table/tr/td/div[1]/div/table[" + i + "]/tr[" + j + "]/td[2]/div").InnerText,
                        HomeTeam = doc.DocumentNode.SelectSingleNode("//*[@id=\"aspnetForm\"]/table/tr/td[2]/table/tr/td/div[1]/div/table[" + i + "]/tr[" + j + "]/td[3]/a").InnerText,
                        HomeScore = doc.DocumentNode.SelectSingleNode("//*[@id=\"aspnetForm\"]/table/tr/td[2]/table/tr/td/div[1]/div/table[" + i + "]/tr[" + j + "]/td[4]/div[1]/span[1]").InnerText,
                        AwayScore = doc.DocumentNode.SelectSingleNode("//*[@id=\"aspnetForm\"]/table/tr/td[2]/table/tr/td/div[1]/div/table[" + i + "]/tr[" + j + "]/td[4]/div[1]/span[2]").InnerText,
                        AwayTeam = doc.DocumentNode.SelectSingleNode("//*[@id=\"aspnetForm\"]/table/tr/td[2]/table/tr/td/div[1]/div/table[" + i + "]/tr[" + j + "]/td[5]/a").InnerText,
                        Location = doc.DocumentNode.SelectSingleNode("//*[@id=\"aspnetForm\"]/table/tr/td[2]/table/tr/td/div[1]/div/table[" + i + "]/tr[" + j + "]/td[6]/div[1]/a").InnerText
                    };

                    SaveItemAsync(newMatch);
                }
            }
        }
    }
}
