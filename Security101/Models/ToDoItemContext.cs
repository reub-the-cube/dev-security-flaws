using Microsoft.EntityFrameworkCore;

namespace Security101.Models
{
    public class ToDoItemContext : DbContext
    {
        public DbSet<TodoItem> ToDoItems { get; set; }

        public string DbPath { get; }

        public ToDoItemContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "todoitem.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}