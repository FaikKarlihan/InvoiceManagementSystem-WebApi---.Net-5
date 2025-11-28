using System;
using System.Linq;
using WebApi.Common;
using WebApi.Data;
using WebApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;

public static class SeedData // not using now
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ImsDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<ImsDbContext>>()))
        {
            if (context.Users.Any())
                return;

            var user = new User
            {
                Name = "Guts",
                Surname = "Berserk",
                NationalId = "1",
                Mail = "a",
                PhoneNumber = "0123456789",
                // number plate must be '-',
                // no housing yet
                Role = Role.Admin
            };
            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, "123456");

            context.Users.Add(user);
            context.SaveChanges();
            Console.WriteLine($"✅ Default test user created: {user.Mail} / 123456");
        }
    }
}