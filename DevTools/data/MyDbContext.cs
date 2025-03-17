
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
                    Id = "c1e2bcd1-5f2b-4ad8-b8d5-08d3b2f8e63b", // 👈 Đặt GUID cố định
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole()
                {
                    Id = "aa24b563-3c1d-41f2-91ad-08d3b2f8e63c", // 👈 Đặt GUID cố định
                    Name = "User",
                    NormalizedName = "USER"
                },
                new IdentityRole()
                {
                    Id = "f3b87c41-1f6d-4a2f-8d1a-08d3b2f8e63d", // 👈 Đặt GUID cố định
                    Name = "Premium",
                    NormalizedName = "PREMIUM"
                }
            };
            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }
    }
}