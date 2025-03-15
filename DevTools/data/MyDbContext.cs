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
        public DbSet<concac> concacs {get; set;}
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // modelBuilder.Entity<Receipt>(e =>
            // {
            //     e.ToTable("Receipt");
            //     e.Property(p => p.Address).HasDefaultValue("227 Nguyen Van Cu");
            //     e.Property(p => p.OrderDate).HasDefaultValue(new DateTime(2024, 1, 1));
            // });

            // modelBuilder.Entity<ReceiptDetail>(e =>
            // {
            //     e.ToTable("ReceiptDetail");
            //     e.HasKey(e => new { e.ItemId, e.ReceiptId });
            //     e.Property(p => p.Quantity).HasDefaultValue(0);

            //     e.HasOne(e => e.item)
            //     .WithMany(e => e.receiptDetails)
            //     .HasForeignKey(e => e.ItemId).
            //     HasConstraintName("FK_ReceiptDetails_Item");

            //     e.HasOne(e => e.receipt)
            //     .WithMany(e => e.receiptDetails)
            //     .HasForeignKey(e => e.ReceiptId).
            //     HasConstraintName("FK_ReceiptDetails_Receipt");

                
            // });

            List<IdentityRole> roles = new List<IdentityRole>
                {
                    new IdentityRole()
                    {
                        Id = "1a2b3c4d-1234-5678-9abc-def123456789",  
                        Name = "Admin",
                        NormalizedName = "ADMIN"
                    },
                    new IdentityRole()
                    {
                        Id = "2b3c4d5e-2345-6789-abcd-ef1234567890",  
                        Name = "User",
                        NormalizedName = "USER"
                    }
                };
            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }
    }
}