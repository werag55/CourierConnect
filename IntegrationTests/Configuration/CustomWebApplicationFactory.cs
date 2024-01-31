using CourierConnect.DataAccess.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Data.Common;

namespace IntegrationTests.Configuration
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
                var connString = GetConnectionString();
                services.AddSqlServer<ApplicationDbContext>(connString);

                var dbContext = CreateDbContext(services);
                dbContext.Database.EnsureDeleted();
            });
        }
        //protected override void ConfigureWebHost(IWebHostBuilder builder)
        //{
        //    builder.ConfigureServices(services =>
        //    {
        //        var dbContextDescriptor = services.SingleOrDefault(
        //            d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

        //        services.Remove(dbContextDescriptor);

        //        var dbConnectionDescriptor = services.SingleOrDefault(
        //            d => d.ServiceType == typeof(DbConnection));

        //        services.Remove(dbConnectionDescriptor);

        //        // open sqliteConnection
        //        services.AddSingleton<DbConnection>(container =>
        //        {
        //            var connection = new SqliteConnection("DataSource=:memory:");
        //            connection.Open();

        //            return connection;
        //        });

        //        services.AddDbContext<ApplicationDbContext>((container, options) =>
        //        {
        //            var connection = container.GetRequiredService<DbConnection>();
        //            options.UseSqlite(connection);
        //        });
        //    });

        //    builder.UseEnvironment("Development"); 
        //}
        private static string? GetConnectionString()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<CustomWebApplicationFactory>()
                .Build();

            var connString = configuration.GetConnectionString("IntegrationTests");
            return connString;
        }

        private static ApplicationDbContext CreateDbContext(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return dbContext;
        }
    }

    
}
