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
}
