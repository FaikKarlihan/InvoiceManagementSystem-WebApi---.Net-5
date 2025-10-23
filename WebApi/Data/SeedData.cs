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
                Name = "test",
                Surname = "tests",
                NationalId = "12345678912",
                Mail = "test@gmail.com",
                PhoneNumber = "0987654321",
                // number plate must be '-',
                // no housing yet
                Role = Role.User
            };
            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, "123456");

            context.Users.Add(user);
            context.SaveChanges();
        }
        Console.WriteLine("✅ Default test user created: test@gmail.com / 123456");
    }
}