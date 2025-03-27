
using DevTools.data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyWebAPI.data;

namespace MyWebAPI.data
{
    public class MyDbContext : IdentityDbContext<User>
    {
        public MyDbContext(DbContextOptions options) : base(options)
        {

        }

        #region DbSet
        public DbSet<Item> Items { get; set; }
        public DbSet<Plugin> Plugins { get; set; }
        public DbSet<PluginCategory> PluginCategories { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Add foreign key 1 - n 
            modelBuilder.Entity<Plugin>()
                .HasOne(p => p.Role)
                .WithMany()
                .HasForeignKey(p => p.AccessiableRoleId);

        }
    }
}