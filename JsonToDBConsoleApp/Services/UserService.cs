using JsonToDBConsoleApp.DTOs;
using System.Net.Http.Json;
using JsonToDBConsoleApp.Models;
using Microsoft.EntityFrameworkCore;

namespace JsonToDBConsoleApp.Services
{
    public class UserService
    {
        public async Task InsertUsersToDb(List<UserDto> userDtos)
        {
            try
            {
                using var context = new AppDbContext();

                var incomingUserIds = userDtos.Select(u => u.Id).ToList();
                var existingUsersDictionary = await context.Users
                    .Where(u => incomingUserIds.Contains(u.Id))
                    .ToDictionaryAsync(u => u.Id!);

                foreach (var dto in userDtos)
                {
                    if (dto.Id != null && existingUsersDictionary.ContainsKey(dto.Id))
                    {
                        Console.WriteLine($"User ID {dto.Id} exists or null. Skipping insertion...");
                    }
                    else
                    {
                        var nameParts = dto?.Name?.Split(' ', 2);
                        var user = new User
                        {
                            Id = dto?.Id,
                            FirstName = nameParts?[0],
                            LastName = nameParts?.Length > 1 ? nameParts[1] : "",
                            Bio = dto?.Bio,
                            Language = dto?.Language,
                            Version = dto?.Version
                        };

                        context.Users.Add(user);
                    }
                }

                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error inserting users into database: {e.Message}");
            }
        }

        public async Task OutputUsersFromDb()
        {
            try
            {
                using var context = new AppDbContext();
                var users = await context.Users
                    .OrderBy(u => u.LastName)
                    .ThenBy(u => u.FirstName)
                    .ToListAsync();

                foreach (var user in users)
                {
                    if (user is null)
                    {
                        continue;
                    }

                    Console.WriteLine($"{user.FirstName} {user.LastName} {user.Id} {user.Bio} {user.Language} {user.Version}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error reading users from database: {e.Message}");
            }
        }
    }
}