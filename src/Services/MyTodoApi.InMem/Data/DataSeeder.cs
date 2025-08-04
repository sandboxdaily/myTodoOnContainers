namespace myTodoApi.InMem.Data
{
    public class DataSeeder
    {
        private TodoDb _db;

        public DataSeeder(TodoDb db)
        {
            _db = db;
        }

        public void SeedData()
        {
            _db.Todos.AddRange(GetTestData());
            _db.SaveChanges();
        }

        private List<TodoItem> GetTestData()
        {
            return new List<TodoItem>()
            {
               new TodoItem { Id = 1, Name = "Call Alvin", IsComplete = false },
               new TodoItem { Id = 2, Name = "Call Simon", IsComplete = false },
               new TodoItem { Id = 3, Name = "Call Theodore", IsComplete = false }
            };
        }
    }
}
