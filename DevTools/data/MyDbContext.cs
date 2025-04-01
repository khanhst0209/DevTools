
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
        public DbSet<UserPlugins> UserPlugins { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Add foreign key 1 - n 
            modelBuilder.Entity<Plugin>()
                .HasOne(p => p.Role)
                .WithMany()
                .HasForeignKey(p => p.AccessiableRoleId);

            modelBuilder.Entity<UserPlugins>()
            .HasKey(x => new { x.UserId, x.PluginId });

            modelBuilder.Entity<UserPlugins>()
            .HasOne(up => up.user)
            .WithMany()
            .HasForeignKey(up => up.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserPlugins>()
                .HasOne(up => up.plugin)
                .WithMany()
                .HasForeignKey(up => up.PluginId)
                .OnDelete(DeleteBehavior.Cascade);

        }


        [DbFunction(name:"SOUNDEX",IsBuiltIn =true)]
        public string FuzzySearch(string querry)
        {
            throw new NotImplementedException();
        }
    }
}