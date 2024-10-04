using Microsoft.EntityFrameworkCore;

using System.Configuration;

using SoadPizza.Model;


namespace SoadPizza
{
    public class SoadPizzaDBContext : DbContext
    {
        public SoadPizzaDBContext(DbContextOptions<SoadPizzaDBContext> options) : base(options) { }

        public SoadPizzaDBContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = ConfigurationManager.ConnectionStrings["SoadPizzaDB"].ConnectionString;
                optionsBuilder.UseNpgsql(connectionString);
            }
        }
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;


         
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Configuring the Customer entity
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customers");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Address).HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(15);
            });

            // Configuring the Order entity
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Customer)
                      .WithMany(c => c.Orders)
                      .HasForeignKey(e => e.CustomerId);
                entity.Property(e => e.OrderPlaced).IsRequired();
                entity.Property(e => e.OrderFulfilled).IsRequired();
            });

            // Configuring the OrderDetail entity
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetails");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Order)
                      .WithMany(o => o.OrderDetails)
                      .HasForeignKey(e => e.OrderId);
                entity.HasOne(e => e.Product)
                      .WithMany()
                      .HasForeignKey(e => e.ProductId);
                entity.Property(e => e.Quantity).IsRequired();
            });

            // Configuring the Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Price).HasColumnType("decimal(6,2)");
            });
        }
    }
}