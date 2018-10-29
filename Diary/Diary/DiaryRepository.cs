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
        SQLiteConnection database;

        public DiaryRepository(string filename)
        {
            string databasePath = DependencyService.Get<ISQLite>().GetDatabasePath(filename);
            database = new SQLiteConnection(databasePath);
            database.CreateTable<Note>();
        }

        public List<Note> GetItems()
        {
            var notes = (from i in database.Table<Note>() select i).ToList();
            return notes;
        }

        public Note GetItem(int id)
        {
            return database.Get<Note>(id);
        }

        public int DeleteItem(Note item)
        {
            return database.Delete(item);
        }

        public int SaveItem(Note item)
        {
            return database.Insert(item);
        }

        public int UpdateItem(Note item)
        {
            database.Update(item);
            return item.Id;
        }
    }
}
