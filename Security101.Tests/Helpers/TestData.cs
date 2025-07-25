using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Security101.Models;

namespace Security101.Tests;

public class TestData
{
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

    public static void RemoveSeededData(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<ToDoItemContext>();

        db.ToDoItems.RemoveRange(GetSeedingToDoItems());
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
            new (){ Id = 1, Name = "Remember something", AuthoredBy = "test-client", IsComplete = false }
        ];
    }
}