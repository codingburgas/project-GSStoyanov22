using AuctionHouse.Core.Models;
using AuctionHouse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouse.Infrastructure.Repositories;

public class BidRepository : IBidRepository
{
    private readonly ApplicationDbContext dbContext;

    public BidRepository(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AddBidAsync(Bid bid)
    {
        await dbContext.Bids.AddAsync(bid);
        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Bid>> GetBidsForAuctionAsync(int auctionId)
    {
        return await dbContext.Bids
            .Where(b => b.AuctionId == auctionId)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
    }
}
