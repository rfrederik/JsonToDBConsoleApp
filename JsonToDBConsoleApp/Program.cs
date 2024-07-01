using JsonToDBConsoleApp.Clients;
using JsonToDBConsoleApp.Services;
using Microsoft.Extensions.DependencyInjection;

namespace JsonToDBConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                await EnsureDatabaseCreatedAsync();

                var services = new ServiceCollection();
                services.AddHttpClient<UserClient>();
                var serviceProvider = services.BuildServiceProvider();

                var userClient = serviceProvider.GetRequiredService<UserClient>();
                var users = await userClient.GetUsers();

                if (users == null || users.Count <= 0)
                {
                    Console.WriteLine("No users fetched from URL.");
                    return;
                }

                var userService = new UserService();
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
