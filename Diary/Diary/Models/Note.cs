using SQLite;

namespace Diary.Models
{
    [Table("Notes")]
    public class Note
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
