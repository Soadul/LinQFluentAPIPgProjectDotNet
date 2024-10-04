using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SoadPizza.Model;
using SoadPizza.Services;

namespace SoadPizza.ConsoleApp
{
    public class ConsoleHandler
    {
        private readonly PizzaService _pizzaService;
        private readonly SoadPizzaDBContext _context;

        public ConsoleHandler(PizzaService pizzaService, SoadPizzaDBContext context)
        {
            _pizzaService = pizzaService;
            _context = context;
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Add Customer");
                Console.WriteLine("2. Add Product");
                Console.WriteLine("3. Show Customers");
                Console.WriteLine("4. Show Products");
                Console.WriteLine("5. Show Orders for a Customer");
                Console.WriteLine("6. Show Products with Price Greater Than");
                Console.WriteLine("7. Exit");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddCustomer();
                        break;
                    case "2":
                        AddProduct();
                        break;
                    case "3":
                        ShowCustomers();
                        break;
                    case "4":
                        ShowProducts();
                        break;
                    case "5":
                        ShowOrdersForCustomer();
                        break;
                    case "6":
                        ShowProductsWithPriceGreaterThan();
                        break;
                    case "7":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void AddCustomer()
        {
            Console.WriteLine("Enter First Name:");
            var firstName = Console.ReadLine();

            Console.WriteLine("Enter Last Name:");
            var lastName = Console.ReadLine();

            Console.WriteLine("Enter Address:");
            var address = Console.ReadLine();

            Console.WriteLine("Enter Phone:");
            var phone = Console.ReadLine();

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                Console.WriteLine("First Name and Last Name are required.");
                return;
            }

            var customer = new Customer
            {
                FirstName = firstName ?? string.Empty,
                LastName = lastName ?? string.Empty,
                Address = address,
                Phone = phone
            };

            try
            {
                _pizzaService.AddCustomer(customer);
                Console.WriteLine("Customer added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding customer: {ex.Message}");
            }
        }

        private void AddProduct()
        {
            Console.WriteLine("Enter Product Name:");
            var name = Console.ReadLine();

            Console.WriteLine("Enter Product Price:");
            if (decimal.TryParse(Console.ReadLine(), out var price))
            {
                if (string.IsNullOrEmpty(name))
                {
                    Console.WriteLine("Product Name is required.");
                    return;
                }

                var product = new Product
                {
                    Name = name ?? string.Empty,
                    Price = price
                };

                try
                {
                    _pizzaService.AddProduct(product);
                    Console.WriteLine("Product added successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error adding product: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid price. Please try again.");
            }
        }

        private void ShowCustomers()
        {
            var customers = _pizzaService.GetCustomers();
            Console.WriteLine("Customers:");
            foreach (var customer in customers)
            {
                Console.WriteLine($"ID: {customer.Id}, Name: {customer.FirstName} {customer.LastName}");
            }
        }

        private void ShowProducts()
        {
            var products = _pizzaService.GetProducts();
            Console.WriteLine("Products:");
            foreach (var product in products)
            {
                Console.WriteLine($"Product ID: {product.Id}, Name: {product.Name}, Price: {product.Price}");
            }
        }

        private void ShowOrdersForCustomer()
        {
            Console.WriteLine("Enter Customer ID:");
            if (int.TryParse(Console.ReadLine(), out var customerId))
            {
                var orders = _context.Orders
                                     .Where(o => o.CustomerId == customerId)
                                     .Include(o => o.OrderDetails)
                                     .ThenInclude(od => od.Product)
                                     .ToList();

                Console.WriteLine("\nOrders:");
                foreach (var order in orders)
                {
                    Console.WriteLine($"Order ID: {order.Id}, Order Placed: {order.OrderPlaced}");
                    foreach (var detail in order.OrderDetails)
                    {
                        Console.WriteLine($"Product: {detail.Product.Name}, Quantity: {detail.Quantity}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid Customer ID. Please try again.");
            }
        }

        private void ShowProductsWithPriceGreaterThan()
        {
            Console.WriteLine("Enter Minimum Price:");
            if (decimal.TryParse(Console.ReadLine(), out var minPrice))
            {
                var products = _context.Products
                                       .Where(p => p.Price > minPrice)
                                       .ToList();

                Console.WriteLine("\nProducts:");
                foreach (var product in products)
                {
                    Console.WriteLine($"Product ID: {product.Id}, Name: {product.Name}, Price: {product.Price}");
                }
            }
            else
            {
                Console.WriteLine("Invalid price. Please try again.");
            }
        }
    }
}