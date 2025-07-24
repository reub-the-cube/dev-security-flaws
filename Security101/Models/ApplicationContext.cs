using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;

namespace Security101.Models
{
    public class ApplicationContext : IdentityDbContext<IdentityUser>
    {
        private const string AdminPassword = "Password1!";

        public string DbPath { get; }

        public ApplicationContext()
        {

            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "auth.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options
                .UseSqlite($"Data Source={DbPath}");

        public static async Task SeedDataAsync(IServiceProvider serviceProvider)
        {
            await SeedRoleAsync(serviceProvider.GetRequiredService<RoleManager<IdentityRole>>(), "Admin");
            await SeedUserAsync(serviceProvider.GetRequiredService<UserManager<IdentityUser>>(), "admin", "admin@security-flaws.com");
            await SeedUserRoleAsync(serviceProvider.GetRequiredService<UserManager<IdentityUser>>(), "admin", "Admin");
        }

        private static async Task SeedRoleAsync(RoleManager<IdentityRole> roleManager, string name)
        {
            if (roleManager != null)
            {
                IdentityRole? role = await roleManager.FindByNameAsync(name);
                if (role == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(name));
                }
            }
        }

        private static async Task SeedUserAsync(UserManager<IdentityUser> userManager, string name, string email)
        {
            if (userManager != null)
            {
                IdentityUser? user = await userManager.FindByNameAsync(name);
                if (user == null)
                {
                    var identityUser = new IdentityUser(name) { Email = email };
                    await userManager.CreateAsync(identityUser, AdminPassword);
                }
            }
        }

        private static async Task SeedUserRoleAsync(UserManager<IdentityUser> userManager, string name, string role)
        {
            if (userManager != null)
            {
                IdentityUser? user = await userManager.FindByNameAsync(name);
                if (user != null) await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}