using JsonToDBConsoleApp.Clients;
using JsonToDBConsoleApp.DTOs;
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

                ServiceCollection services = new ServiceCollection();
                services.AddHttpClient<UserClient>();
                ServiceProvider serviceProvider = services.BuildServiceProvider();

                UserClient userClient = serviceProvider.GetRequiredService<UserClient>();
                List<UserDto>? users = await userClient.GetUsers();

                if (users == null || users.Count <= 0)
                {
                    Console.WriteLine("No users fetched from URL.");
                    return;
                }

                UserService userService = new UserService();
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
            using AppDbContext context = new AppDbContext();
            await context.Database.EnsureCreatedAsync();
        }
    }
}
