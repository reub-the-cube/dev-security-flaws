using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Security101.Models
{
    public class ApplicationContext : IdentityDbContext<IdentityUser>
    {
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
            var _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            if (_roleManager != null)
            {
                IdentityRole? role = await _roleManager.FindByNameAsync("Admin");
                if (role == null)
                {
                    _ = await _roleManager.CreateAsync(new IdentityRole("Admin"));
                }
            }

            var _userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            
            if (_userManager != null)
            {
                IdentityUser? user = await _userManager.FindByNameAsync("admin");
                if (user == null)
                {
                    var identityUser = new IdentityUser("admin") { Email = "admin@security-flaws.com" };
                    _ = await _userManager.CreateAsync(identityUser, "Password1!");
                    user = await _userManager.FindByNameAsync("admin");
                }

                if (user != null) await _userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}