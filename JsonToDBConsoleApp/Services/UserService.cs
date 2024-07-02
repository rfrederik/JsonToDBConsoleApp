using System.Text;
using JsonToDBConsoleApp.DTOs;
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
                AppDbContext context = new AppDbContext();
                HashSet<string> existingUserIds = new HashSet<string>(context.Users.Select(u => u.Id));

                foreach (UserDto dto in userDtos)
                {
                    if (dto?.Id == null || existingUserIds.Contains(dto.Id))
                    {
                        Console.WriteLine($"User ID {dto?.Id} exists or null. Skipping insertion...");
                        continue;
                    }

                    string[]? nameParts = dto?.Name?.Split(' ', 2);
                    User user = new User
                    {
                        Id = dto.Id,
                        FirstName = nameParts?[0],
                        LastName = nameParts?.Length > 1 ? nameParts[1] : "",
                        Bio = dto?.Bio,
                        Language = dto?.Language,
                        Version = dto?.Version
                    };

                    context.Users.Add(user);
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
                using AppDbContext context = new AppDbContext();
                List<User> users = await context.Users
                    .OrderBy(u => u.LastName)
                    .ThenBy(u => u.FirstName)
                    .ToListAsync();

                foreach (User user in users)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"{user.FirstName} {user.LastName}");
                    sb.AppendLine(user.Id);
                    sb.AppendLine(user.Bio);
                    sb.AppendLine(user.Language);
                    sb.AppendLine(user.Version.ToString());

                    Console.WriteLine(sb);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error reading users from database: {e.Message}");
            }
        }
    }
}