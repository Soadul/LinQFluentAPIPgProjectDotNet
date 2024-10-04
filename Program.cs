using System;
using System.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SoadPizza.Services;
using SoadPizza.ConsoleApp;
using Microsoft.EntityFrameworkCore;

namespace SoadPizza
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var consoleHandler = host.Services.GetRequiredService<ConsoleHandler>();
            consoleHandler.Run();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    // Read the connection string from App.config
                    string connectionString = ConfigurationManager.ConnectionStrings["SoadPizzaDB"].ConnectionString;

                    services.AddDbContext<SoadPizzaDBContext>(options =>
                        options.UseNpgsql(connectionString)); // UseNpgsql for PostgreSQL
                    services.AddScoped<PizzaService>();
                    services.AddScoped<ConsoleHandler>();
                });
    }
}