namespace myTodoApi.Sqlite.Data
{
    public static class Extension
    {
        public static IApplicationBuilder UseMigration(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TodoDb>();
                context.Database.MigrateAsync();
            }
            return app;
        }
    }
}
