using AuctionHouse.Core.Models;
using AuctionHouse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouse.Infrastructure.Repositories;

public class AuctionRepository : IAuctionRepository
{
    private readonly ApplicationDbContext dbContext;

    public AuctionRepository(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<Auction>> GetAllAsync()
    {
        return await dbContext.Auctions
            .Include(a => a.Seller)
            .Include(a => a.Bids)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<Auction?> GetByIdAsync(int id)
    {
        return await dbContext.Auctions
            .Include(a => a.Seller)
            .Include(a => a.Bids)
            .ThenInclude(b => b.User)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task AddAsync(Auction auction)
    {
        await dbContext.Auctions.AddAsync(auction);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Auction auction)
    {
        dbContext.Auctions.Update(auction);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Auction auction)
    {
        dbContext.Auctions.Remove(auction);
        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Auction>> GetActiveAuctionsAsync()
    {
        return await dbContext.Auctions
            .Include(a => a.Bids)
            .Where(a => a.IsActive && a.EndTime > DateTime.UtcNow)
            .OrderBy(a => a.EndTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Auction>> GetBySellerIdAsync(string sellerId)
    {
        return await dbContext.Auctions
            .Include(a => a.Bids)
            .Where(a => a.SellerId == sellerId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Auction>> GetAuctionsWhereUserHasBidsAsync(string userId)
    {
        return await dbContext.Auctions
            .Include(a => a.Bids)
            .Where(a => a.Bids.Any(b => b.UserId == userId))
            .OrderByDescending(a => a.EndTime)
            .ToListAsync();
    }

    public async Task<int> GetTotalBidsCountAsync()
    {
        return await dbContext.Bids.CountAsync();
    }

    public async Task UpdateExpiredAuctionsAsync(DateTime utcNow)
    {
        var expiredActiveAuctions = await dbContext.Auctions
            .Where(a => a.IsActive && a.EndTime <= utcNow)
            .ToListAsync();

        if (expiredActiveAuctions.Count == 0)
        {
            return;
        }

        foreach (var auction in expiredActiveAuctions)
        {
            auction.IsActive = false;
        }

        await dbContext.SaveChangesAsync();
    }
}
