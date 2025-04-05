using Microsoft.EntityFrameworkCore;
using WalletAPI.Domain;

namespace WalletAPI.Infrastructure.Data
{
    public class WalletDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Wallet> Wallet { get; set; }
        public DbSet<Transaction> Transaction { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Wallet>()
                .HasMany(w => w.Transactions) // wallet => trasactions
                .WithOne(t => t.Wallet)      // transaction to wallet
                .HasForeignKey(t => t.WalletId) //  WalletId foreignkey
                .OnDelete(DeleteBehavior.Cascade); // delete trasaction if wallet is deleted

            modelBuilder.Entity<Wallet>()
              .Property(w => w.Balance)
              .HasPrecision(18, 2); //  precision : 18 digits, 2 decimals


            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2); // precision: 18 digits, 2 decimals

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Type)
                .HasMaxLength(10) // max size of type
                .IsRequired();
        }


    }
}
