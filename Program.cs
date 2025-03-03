using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Security101.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ToDoItemContext>();
builder.Services.AddDbContext<ApplicationContext>();

builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationContext>();

var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
    await ApplicationContext.SeedDataAsync(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapIdentityApi<IdentityUser>();

app.MapPost("/logout", async (SignInManager<IdentityUser> signInManager,
    [Microsoft.AspNetCore.Mvc.FromBody] object empty) =>
{
    if (empty != null)
    {
        await signInManager.SignOutAsync();
        return Results.Ok();
    }
    return Results.Unauthorized();
})
.WithOpenApi()
.RequireAuthorization();

app.MapGet("/api/todoitems", async (ToDoItemContext db) => {
    return await db.ToDoItems.ToListAsync();
})
.WithName("GetToDoItems")
.WithOpenApi()
.RequireAuthorization();

app.MapGet("/api/todoitems/{id}", async (long id, ToDoItemContext db) => {
    var item = await db.ToDoItems.FindAsync(id);
    return item is not null ? Results.Ok(item) : Results.NotFound();
})
.WithName("GetToDoItem")
.WithOpenApi()
.RequireAuthorization();

app.MapPost("/api/todoitems", async (TodoItem todoItem, ToDoItemContext db) => {
    db.ToDoItems.Add(todoItem);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{todoItem.Id}", todoItem);
})
.WithName("CreateToDoItem")
.WithOpenApi()
.RequireAuthorization();

app.Run();