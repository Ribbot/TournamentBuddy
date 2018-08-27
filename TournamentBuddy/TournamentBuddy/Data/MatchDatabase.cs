using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
        public Task<List<MatchItem>> GetItemsAsync()
        {
            return database.Table<MatchItem>().ToListAsync();
        }

        public Task<List<MatchItem>> GetItemsNotDoneAsync()
        {
            return database.QueryAsync<MatchItem>("SELECT * FROM [MatchItem] WHERE [Done] = 0");
        }

        public Task<MatchItem> GetItemAsync(int id)
        {
            return database.Table<MatchItem>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

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

        public Task<int> DeleteItemAsync(MatchItem item)
        {
            return database.DeleteAsync(item);
        }

        public Task<int> DeleteItems()
        {
            return database.ExecuteAsync("DELETE FROM MatchItem");
        }
    }
}
