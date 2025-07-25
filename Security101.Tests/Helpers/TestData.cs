using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Security101.Models;

namespace Security101.Tests;

public class TestData
{
    internal const string TEST_USER_NAME = "test-client";
    
    private static bool _hasBeenMigrated = false;

    private static void Migrate(ToDoItemContext db)
    {
        db.Database.Migrate();
        _hasBeenMigrated = true;
    }

    public static void AddSeededData(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<ToDoItemContext>();

        EnsureMigrated(db);

        db.ToDoItems.AddRange(GetSeedingToDoItems());
        db.SaveChanges();
    }

    public static void RemoveToDoItems(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<ToDoItemContext>();

        db.ToDoItems.RemoveRange(db.ToDoItems);
        db.SaveChanges();
    }

    private static void EnsureMigrated(ToDoItemContext db)
    {
        if (!_hasBeenMigrated) Migrate(db);
    }

    private static List<TodoItem> GetSeedingToDoItems()
    {
        return
        [
            new (){ Id = 1, Name = "Remember something", AuthoredBy = TEST_USER_NAME, IsComplete = false },
            new (){ Id = 2, Name = "Remember something else", AuthoredBy = TEST_USER_NAME, IsComplete = false },
            new (){ Id = 3, Name = "Do a nice deed for somebody", AuthoredBy = TEST_USER_NAME, IsComplete = false },
        ];
    }
}