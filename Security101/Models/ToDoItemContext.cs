using Microsoft.EntityFrameworkCore;

namespace Security101.Models
{
    public class ToDoItemContext(DbContextOptions<ToDoItemContext> options) : DbContext(options)
    {
        public DbSet<TodoItem> ToDoItems { get; set; }
    }
}