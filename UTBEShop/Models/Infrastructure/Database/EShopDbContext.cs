using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UTBEShop.Models.Entities;
using UTBEShop.Models.Entities.Identity;
using UTBEShop.Models.Infrastructure.Database.Configuration;

namespace UTBEShop.Models.Infrastructure.Database
{
    public class EShopDbContext : IdentityDbContext<User, Role, int>
    {
        public DbSet<Product>? Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }


        #region Constructor
        public EShopDbContext(DbContextOptions options):base(options)
        {

        }
        #endregion

        //Configure Database While Creating the model 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //this line is so important because: 
            /*
             -keys of Identity tables are mapped in OnModelCreating method of IdentityDbContext and if this method is not called,
            you will end up getting the error that you got.
             */
            base.OnModelCreating(modelBuilder);
            DatabaseInit init = new DatabaseInit();
            modelBuilder.Entity<Product>().HasData(init.GenerateFakeProducts());
            //we need also to generate data seeding to fill database wiht the Role onCreating of migration
            modelBuilder.Entity<Role>().HasData(init.GenerateRoles());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
        }

    }
}
