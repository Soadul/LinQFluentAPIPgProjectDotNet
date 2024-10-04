using System;
using System.Collections.Generic;
using System.Linq;
using SoadPizza.Model;

namespace SoadPizza.Services
{
    public class PizzaService
    {
        private readonly SoadPizzaDBContext _context;

        public PizzaService(SoadPizzaDBContext context)
        {
            _context = context;
        }

        // Create
        public void AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        // Read
        public List<Customer> GetCustomers()
        {
            return _context.Customers.ToList();
        }

        public List<Product> GetProducts()
        {
            return _context.Products.ToList();
        }

        // Update
        public void UpdateCustomer(Customer customer)
        {
            var existingCustomer = _context.Customers.FirstOrDefault(c => c.Id == customer.Id);
            if (existingCustomer != null)
            {
                existingCustomer.FirstName = customer.FirstName;
                existingCustomer.LastName = customer.LastName;
                existingCustomer.Address = customer.Address;
                existingCustomer.Phone = customer.Phone;
                _context.SaveChanges();
            }
        }

        public void UpdateProduct(Product product)
        {
            var existingProduct = _context.Products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                _context.SaveChanges();
            }
        }

        // Delete
        public void DeleteCustomer(int customerId)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Id == customerId);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }
        }

        public void DeleteProduct(int productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
    }
}