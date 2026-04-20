using AuctionHouse.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouse.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Auction> Auctions => Set<Auction>();

    public DbSet<Bid> Bids => Set<Bid>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Auction>()
            .HasOne(a => a.Seller)
            .WithMany(u => u.Auctions)
            .HasForeignKey(a => a.SellerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Bid>()
            .HasOne(b => b.Auction)
            .WithMany(a => a.Bids)
            .HasForeignKey(b => b.AuctionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Bid>()
            .HasOne(b => b.User)
            .WithMany(u => u.Bids)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Auction>()
            .Property(a => a.Title)
            .HasMaxLength(120)
            .IsRequired();

        builder.Entity<Auction>()
            .Property(a => a.Description)
            .HasMaxLength(2000)
            .IsRequired();

        builder.Entity<Auction>()
            .Property(a => a.StartingPrice)
            .HasPrecision(18, 2);

        builder.Entity<Auction>()
            .Property(a => a.CurrentPrice)
            .HasPrecision(18, 2);

        builder.Entity<Auction>()
            .Property(a => a.BidStep)
            .HasPrecision(18, 2);

        builder.Entity<Bid>()
            .Property(b => b.Amount)
            .HasPrecision(18, 2);

        builder.Entity<ApplicationUser>()
            .Property(u => u.IsBlocked)
            .HasDefaultValue(false);
    }
}
