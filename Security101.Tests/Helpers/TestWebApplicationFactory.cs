using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Security101.Models;

namespace Security101.Tests;

public class TestWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    private static readonly string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    private static readonly string databasePath = $"Data Source={Path.Join(appDataPath, "todoitem_tests.db")}";

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var toDoItemContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ToDoItemContext>));

            if (toDoItemContextDescriptor != null)
            {
                services.Remove(toDoItemContextDescriptor);
            }

            services.AddDbContext<ToDoItemContext>(options =>
            {
                options.UseSqlite(databasePath);
            });
        });

        return base.CreateHost(builder);
    }

    public WebApplicationFactory<TProgram> WithAuthentication<T>() where T : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        return WithWebHostBuilder(builder =>
        {
        builder.ConfigureTestServices(services =>
        {
            services.AddAuthentication(defaultScheme: "DefaultScheme")
                .AddScheme<AuthenticationSchemeOptions, T>(
                    "DefaultScheme", options => { });        
            });
        });
    }
}