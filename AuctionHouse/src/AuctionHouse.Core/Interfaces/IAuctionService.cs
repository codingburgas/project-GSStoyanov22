using AuctionHouse.Core.DTOs;

namespace AuctionHouse.Core.Interfaces;

public interface IAuctionService
{
    Task<AuctionViewModel> CreateAuctionAsync(AuctionViewModel model, string sellerId);

    Task<bool> EndAuctionAsync(int auctionId, string requestUserId, bool isAdmin);

    Task<IEnumerable<AuctionViewModel>> GetActiveAuctionsAsync();

    Task<IEnumerable<AuctionViewModel>> GetAllAuctionsAsync();

    Task<AuctionViewModel?> GetAuctionByIdAsync(int id);

    Task<bool> UpdateAuctionAsync(AuctionViewModel model, string requestUserId, bool isAdmin);

    Task<bool> DeleteAuctionAsync(int id, string requestUserId, bool isAdmin);
}
