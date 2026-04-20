using AuctionHouse.Core.Models;

namespace AuctionHouse.Infrastructure.Repositories;

public interface IAuctionRepository
{
    Task<IEnumerable<Auction>> GetAllAsync();

    Task<Auction?> GetByIdAsync(int id);

    Task AddAsync(Auction auction);

    Task UpdateAsync(Auction auction);

    Task DeleteAsync(Auction auction);

    Task<IEnumerable<Auction>> GetActiveAuctionsAsync();

    Task<IEnumerable<Auction>> GetBySellerIdAsync(string sellerId);

    Task<IEnumerable<Auction>> GetAuctionsWhereUserHasBidsAsync(string userId);

    Task<int> GetTotalBidsCountAsync();

    Task UpdateExpiredAuctionsAsync(DateTime utcNow);
}
