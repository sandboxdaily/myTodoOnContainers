using Microsoft.EntityFrameworkCore;
using myTodoApi.Sqlite.Models;

namespace myTodoApi.Sqlite.Data
{
    public class TodoDb : DbContext
    {
        public TodoDb(DbContextOptions<TodoDb> options) : base(options) { }
        public DbSet<TodoItem> Todos { get; set; }
    }
}
