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
                Console.WriteLine("Choose an entity:");
                Console.WriteLine("1. Customer");
                Console.WriteLine("2. Product");
                Console.WriteLine("3. Order");
                Console.WriteLine("4. Exit");

                var entityChoice = Console.ReadLine();

                switch (entityChoice)
                {
                    case "1":
                        HandleCustomerOperations();
                        break;
                    case "2":
                        HandleProductOperations();
                        break;
                    case "3":
                        HandleOrderOperations();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void HandleCustomerOperations()
        {
            while (true)
            {
                Console.WriteLine("Choose an operation for Customer:");
                Console.WriteLine("1. Create");
                Console.WriteLine("2. Read");
                Console.WriteLine("3. Update");
                Console.WriteLine("4. Delete");
                Console.WriteLine("5. Back to main menu");

                var operationChoice = Console.ReadLine();

                switch (operationChoice)
                {
                    case "1":
                        AddCustomer();
                        break;
                    case "2":
                        ShowCustomers();
                        break;
                    case "3":
                        UpdateCustomer();
                        break;
                    case "4":
                        DeleteCustomer();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void HandleProductOperations()
        {
            while (true)
            {
                Console.WriteLine("Choose an operation for Product:");
                Console.WriteLine("1. Create");
                Console.WriteLine("2. Read");
                Console.WriteLine("3. Update");
                Console.WriteLine("4. Delete");
                Console.WriteLine("5. Back to main menu");

                var operationChoice = Console.ReadLine();

                switch (operationChoice)
                {
                    case "1":
                        AddProduct();
                        break;
                    case "2":
                        ShowProducts();
                        break;
                    case "3":
                        UpdateProduct();
                        break;
                    case "4":
                        DeleteProduct();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void HandleOrderOperations()
        {
            while (true)
            {
                Console.WriteLine("Choose an operation for Order:");
                Console.WriteLine("1. Show Orders for a Customer");
                Console.WriteLine("2. Show Products with Price Greater Than");
                Console.WriteLine("3. Back to main menu");

                var operationChoice = Console.ReadLine();

                switch (operationChoice)
                {
                    case "1":
                        ShowOrdersForCustomer();
                        break;
                    case "2":
                        ShowProductsWithPriceGreaterThan();
                        break;
                    case "3":
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

        private void UpdateCustomer()
        {
            Console.WriteLine("Enter Customer ID to update:");
            if (int.TryParse(Console.ReadLine(), out var customerId))
            {
                var existingCustomer = _pizzaService.GetCustomers().FirstOrDefault(c => c.Id == customerId);
                if (existingCustomer == null)
                {
                    Console.WriteLine("Customer not found.");
                    return;
                }

                Console.WriteLine("Enter First Name:");
                existingCustomer.FirstName = Console.ReadLine() ?? existingCustomer.FirstName;

                Console.WriteLine("Enter Last Name:");
                existingCustomer.LastName = Console.ReadLine() ?? existingCustomer.LastName;

                Console.WriteLine("Enter Address:");
                existingCustomer.Address = Console.ReadLine() ?? existingCustomer.Address;

                Console.WriteLine("Enter Phone:");
                existingCustomer.Phone = Console.ReadLine() ?? existingCustomer.Phone;

                try
                {
                    _pizzaService.UpdateCustomer(existingCustomer);
                    Console.WriteLine("Customer updated successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating customer: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid Customer ID. Please try again.");
            }
        }

        private void UpdateProduct()
        {
            Console.WriteLine("Enter Product ID to update:");
            if (int.TryParse(Console.ReadLine(), out var productId))
            {
                var existingProduct = _pizzaService.GetProducts().FirstOrDefault(p => p.Id == productId);
                if (existingProduct == null)
                {
                    Console.WriteLine("Product not found.");
                    return;
                }

                Console.WriteLine("Enter Product Name:");
                existingProduct.Name = Console.ReadLine() ?? existingProduct.Name;

                Console.WriteLine("Enter Product Price:");
                if (decimal.TryParse(Console.ReadLine(), out var price))
                {
                    existingProduct.Price = price;
                }
                else
                {
                    Console.WriteLine("Invalid price. Keeping the old price.");
                }

                try
                {
                    _pizzaService.UpdateProduct(existingProduct);
                    Console.WriteLine("Product updated successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating product: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid Product ID. Please try again.");
            }
        }

        private void DeleteCustomer()
        {
            Console.WriteLine("Enter Customer ID to delete:");
            if (int.TryParse(Console.ReadLine(), out var customerId))
            {
                try
                {
                    _pizzaService.DeleteCustomer(customerId);
                    Console.WriteLine("Customer deleted successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting customer: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid Customer ID. Please try again.");
            }
        }

        private void DeleteProduct()
        {
            Console.WriteLine("Enter Product ID to delete:");
            if (int.TryParse(Console.ReadLine(), out var productId))
            {
                try
                {
                    _pizzaService.DeleteProduct(productId);
                    Console.WriteLine("Product deleted successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting product: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid Product ID. Please try again.");
            }
        }
    }
}
