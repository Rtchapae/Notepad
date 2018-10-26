using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Diary.Attributes;
using Diary.IService;
using Diary.Models;
using SQLite;
using Xamarin.Forms;

namespace Diary
{
    public class DiaryRepository 
    {
        SQLiteAsyncConnection database;
        public DiaryRepository(string filename)
        {
            string databasePath = DependencyService.Get<ISQLite>().GetDatabasePath(filename);
            database = new SQLiteAsyncConnection(databasePath);
        }

        public async Task CreateTable()
        {
            await database.CreateTableAsync<Note>().ConfigureAwait(false);
        }
        public async Task<List<Note>> GetItemsAsync()
        {
            var notes = await database.Table<Note>().ToListAsync().ConfigureAwait(false);
            return notes;
        }
        public async Task<Note> GetItemAsync(int id)
        {
            return await database.GetAsync<Note>(id).ConfigureAwait(false);
        }
        public async Task<int> DeleteItemAsync(Note item)
        {
            return await database.DeleteAsync(item).ConfigureAwait(false);
        }
        public async Task<int> SaveItemAsync(Note item)
        {
            if (item.Id != 0)
            {
               await database.UpdateAsync(item).ConfigureAwait(false);
                return item.Id;
            }
            else
            {
                return await database.InsertAsync(item).ConfigureAwait(false);
            }
        }
    }
}
