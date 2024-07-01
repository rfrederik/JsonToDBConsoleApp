using System;
using JsonToDBConsoleApp.Services;

namespace JsonToDBConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const string url = "https://microsoftedge.github.io/Demos/json-dummy-data/64KB.json";

            try
            {
                await EnsureDatabaseCreatedAsync();

                var userService = new UserService();
                var users = await userService.GetUsersFromUrl(url);

                if (users == null || users.Count <= 0)
                {
                    Console.WriteLine("No users fetched from URL.");
                    return;
                }

                await userService.InsertUsersToDb(users);
                await userService.OutputUsersFromDb();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private static async Task EnsureDatabaseCreatedAsync()
        {
            using var context = new AppDbContext();
            await context.Database.EnsureCreatedAsync();
        }
    }
}
