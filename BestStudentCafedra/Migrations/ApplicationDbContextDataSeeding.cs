using BestStudentCafedra.Data;
using BestStudentCafedra.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestStudentCafedra
{
    public class ApplicationDbContextDataSeeding
    {
        private ApplicationDbContext _context;

        public ApplicationDbContextDataSeeding(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            string[] roles = new string[] { "methodist", "teacher", "student" };

            foreach (string role in roles)
            {
                var roleStore = new RoleStore<IdentityRole>(_context);

                if (!_context.Roles.Any(r => r.Name == role))
                {
                    roleStore.CreateAsync(new IdentityRole { Name = role, NormalizedName = role.ToUpper() }).Wait();
                }
            }

            var user = new User
            {
                UserName = "root@best.com",
                NormalizedUserName = "ROOT@BEST.COM",
                Email = "root@best.com",
                NormalizedEmail = "ROOT@BEST.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                IsConfirmed = true
            };

            if (!_context.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<User>();
                var hashed = password.HashPassword(user, "root");
                user.PasswordHash = hashed;

                var userStore = new UserStore<User>(_context);
                var result = userStore.CreateAsync(user);
                userStore.AddToRoleAsync(user, roles[0]).Wait();
            }

            _context.SaveChangesAsync();
        }
    }
}
