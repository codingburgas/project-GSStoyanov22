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
}
