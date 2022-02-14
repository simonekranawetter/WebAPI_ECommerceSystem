using Microsoft.EntityFrameworkCore;
using WebAPI_ECommerceSystem.Entities;

namespace WebAPI_ECommerceSystem
{
    public class SqlContext : DbContext
    {
        public SqlContext(DbContextOptions<SqlContext> options) : base(options)
        {
        }

        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<AddressEntity> Addresses => Set<AddressEntity>();
        public DbSet<ProductEntity> Products => Set<ProductEntity>();
        public DbSet<ProductCategoryEntity> ProductCategories => Set<ProductCategoryEntity>();
        public DbSet<OrderEntity> Orders => Set<OrderEntity>();
        public DbSet<OrderRowEntity> OrderRows => Set<OrderRowEntity>();
    }
}
