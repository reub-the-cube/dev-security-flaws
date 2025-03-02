using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Security101.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ToDoItemContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/todoitems", async (ToDoItemContext db) => {
    return await db.ToDoItems.ToListAsync();
})
.WithName("GetToDoItems")
.WithOpenApi();

app.MapGet("/api/todoitems/{id}", async (long id, ToDoItemContext db) => {
    var item = await db.ToDoItems.FindAsync(id);
    return item is not null ? Results.Ok(item) : Results.NotFound();
})
.WithName("GetToDoItem")
.WithOpenApi();

app.MapPost("/api/todoitems", async (TodoItem todoItem, ToDoItemContext db) => {
    db.ToDoItems.Add(todoItem);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{todoItem.Id}", todoItem);
})
.WithName("CreateToDoItem")
.WithOpenApi();

app.Run();