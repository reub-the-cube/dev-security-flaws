using Microsoft.EntityFrameworkCore;
using Security101.Models;

public static class ToDoEndpoints
{
    public static void Map(WebApplication app)
    {

        app.MapGet("/api/todoitems", async (ToDoItemContext db) =>
        {
            return await db.ToDoItems.ToListAsync();
        })
        .WithName("GetToDoItems")
        .WithOpenApi()
        .RequireAuthorization("AdminOnly");

        app.MapGet("/api/todoitems/{id}", async (long id, ToDoItemContext db) =>
        {
            // SMELL: you should not be able to get an item you don't have permission for
            var item = await db.ToDoItems.FindAsync(id);
            return item is not null ? Results.Ok(item) : Results.NotFound();
        })
        .WithName("GetToDoItem")
        .WithOpenApi()
        .RequireAuthorization();

        app.MapGet("/api/todoitems/name/{name}", async (string name, ToDoItemContext db, HttpContext httpContext) =>
        {
            var query = db.Database.SqlQueryRaw<TodoItem>($"SELECT * FROM ToDoItems WHERE Name LIKE '%{name}%'");
            //var query = db.Database.SqlQueryRaw<TodoItem>($"SELECT * FROM ToDoItems WHERE AuthoredBy = '{httpContext.User.Identity?.Name}' AND Name LIKE '%{name}%'");
            return await query.ToListAsync();
        })
        .WithName("GetToDoItemsByName")
        .WithOpenApi()
        .RequireAuthorization();

        app.MapPost("/api/todoitems", async (TodoItem todoItem, ToDoItemContext db, HttpContext httpContext) =>
        {
            todoItem.AuthoredBy = httpContext.User.Identity?.Name;
            db.ToDoItems.Add(todoItem);
            await db.SaveChangesAsync();

            return Results.Created($"/todoitems/{todoItem.Id}", todoItem);
        })
        .WithName("CreateToDoItem")
        .WithOpenApi()
        .RequireAuthorization();
    }
}