using EcommerceBookApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceBookApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) //Options will be passed to the base class DbContext
        {
            
        }

        public DbSet<Category> Categories{ get; set; } //Categories will be the table name

        //We are overriding the below method from DbContext to seed the data in to the table Category.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 }, //Object Initializer syntax
                new Category { Id = 2, Name = "SciFi", DisplayOrder = 2 },
                new Category { Id = 3, Name = "History", DisplayOrder = 3 }
                );
        }

    }
}
