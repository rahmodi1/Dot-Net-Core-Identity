using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public interface IDbInitializer
    {
        void Initialize();
    }


    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<MyIdentityRole> _roleManager;

        public DbInitializer(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<MyIdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async void Initialize()
        {
            _context.Database.EnsureCreated();//if db is not exist ,it will create database .but ,do nothing .

            // Look for any user.
            if (_context.Users.Any())
            {
                return;   // DB has been seeded
            }

            //If there is already an Administrator role, abort
            if (_context.Roles.Any(r => r.Name == "Administrator")) return;

            //Create the Administartor Role
            await _roleManager.CreateAsync(new MyIdentityRole { Name = "Administrator", Description = "Admin Role" });

            //Create the default Admin account and apply the Administrator role
            string user = "rmo@narola.email";
            string password = "Password@123";
            await _userManager.CreateAsync(new ApplicationUser { UserName = user, Email = user, EmailConfirmed = true, FullName = "Rahul", BirthDate = DateTime.Parse("2005-09-01") }, password);
            await _userManager.AddToRoleAsync(await _userManager.FindByNameAsync(user), "Administrator");
        }
    }
}
