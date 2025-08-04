using Microsoft.EntityFrameworkCore;
using myTodoApi.InMem.Data;
using myTodoApi.InMem.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    //Seed the database with initial data
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<TodoDb>();
        var seeder = new DataSeeder(context);
        seeder.SeedData();
    }
}

app.UseHttpsRedirection();

app.MapGet("/todoitems", async (TodoDb db) => await db.Todos.ToListAsync());
app.MapGet("/todoitems/{id}", async (int id, TodoDb db) => await db.Todos.FindAsync(id));
app.MapPost("/todoitems", async (TodoItem todo, TodoDb db) =>
{
    /* Add & Save Logic */
    db.Add(todo);
    await db.SaveChangesAsync();
});
app.MapDelete("/todoitems/{id}", async (int id, TodoDb db) =>
{
    /* Delete Logic */
    var todo = await db.Todos.FindAsync(id);
    if (todo is not null)
    {
        db.Remove(todo);
        await db.SaveChangesAsync();
    }
});

app.Run();
