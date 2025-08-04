using Microsoft.EntityFrameworkCore;
using myTodoApi.Sqlite.Data;
using myTodoApi.Sqlite.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TodoDb>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

app.UseMigration(); // Ensure migrations are applied, See: // myTodoApi.Sqlite/Data/Extension.cs

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

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
