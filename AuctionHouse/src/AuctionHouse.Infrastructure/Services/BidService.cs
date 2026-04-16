using AuctionHouse.Core.DTOs;
using AuctionHouse.Core.Interfaces;
using AuctionHouse.Core.Models;
using AuctionHouse.Infrastructure.Repositories;

namespace AuctionHouse.Infrastructure.Services;

public class BidService : IBidService
{
    private readonly IAuctionRepository auctionRepository;
    private readonly IBidRepository bidRepository;

    public BidService(IAuctionRepository auctionRepository, IBidRepository bidRepository)
    {
        this.auctionRepository = auctionRepository;
        this.bidRepository = bidRepository;
    }

    public async Task<(bool Success, string Message)> PlaceBidAsync(BidViewModel model, string userId)
    {
        var auction = await auctionRepository.GetByIdAsync(model.AuctionId);
        if (auction is null)
        {
            return (false, "Auction not found.");
        }

        if (!auction.IsActive || auction.EndTime <= DateTime.UtcNow)
        {
            return (false, "Auction is not active.");
        }

        if (model.Amount <= auction.CurrentPrice)
        {
            return (false, "Bid must be higher than current price.");
        }

        var difference = model.Amount - auction.CurrentPrice;
        if (difference % auction.BidStep != 0)
        {
            return (false, "Bid increment must match the auction bid step.");
        }

        var bid = new Bid
        {
            AuctionId = auction.Id,
            UserId = userId,
            Amount = model.Amount
        };

        await bidRepository.AddBidAsync(bid);

        auction.CurrentPrice = model.Amount;
        await auctionRepository.UpdateAsync(auction);

        return (true, "Bid placed successfully.");
    }
}
