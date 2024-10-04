using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using SoadPizza.Services;

namespace SoadPizza.ConsoleApp
{
    class MainApp
    {
        static void Main(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SoadPizzaDBContext>();
            var connectionString = ConfigurationManager.ConnectionStrings["SoadPizzaDB"].ConnectionString;
            optionsBuilder.UseNpgsql(connectionString);

            using (var context = new SoadPizzaDBContext(optionsBuilder.Options))
            {
                var pizzaService = new PizzaService(context);
                var consoleHandler = new ConsoleHandler(pizzaService,context);
                consoleHandler.Run();
            }
        }
      
        
    }
}