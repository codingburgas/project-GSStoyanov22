using AuctionHouse.Core.Models;

namespace AuctionHouse.Infrastructure.Repositories;

public interface IBidRepository
{
    Task AddBidAsync(Bid bid);

    Task<IEnumerable<Bid>> GetBidsForAuctionAsync(int auctionId);
}
