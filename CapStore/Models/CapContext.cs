using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CapStore.Models
{
    public class CapContext : IdentityDbContext<Customer>
    {
        public DbSet<Cap> Caps { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderCap> OrderCaps { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<CartCap> CartCaps { get; set; }

        public CapContext(DbContextOptions<CapContext> options)
            : base(options)
        {

        }
    }
}
