using Microsoft.EntityFrameworkCore;
 
namespace ecommerce.Models
{
    public class ecommerceContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public ecommerceContext(DbContextOptions<ecommerceContext> options) : base(options) { }
        public DbSet <User> Users { get; set; }
        public DbSet <Product> Products { get; set; }
        public DbSet <Message> Messages { get; set; }
        public DbSet <Interest> Interests { get; set; }
        public DbSet <Category> Categories { get; set; }
        public DbSet <Image> Images { get; set; }
    }
}