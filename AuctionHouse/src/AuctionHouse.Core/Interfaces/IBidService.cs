using AuctionHouse.Core.DTOs;

namespace AuctionHouse.Core.Interfaces;

public interface IBidService
{
    Task<(bool Success, string Message)> PlaceBidAsync(BidViewModel model, string userId);
}
