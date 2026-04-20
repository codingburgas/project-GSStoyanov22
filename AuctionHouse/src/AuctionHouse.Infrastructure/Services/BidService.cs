using AuctionHouse.Core.DTOs;
using AuctionHouse.Core.Interfaces;
using AuctionHouse.Core.Models;
using AuctionHouse.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;

namespace AuctionHouse.Infrastructure.Services;

public class BidService : IBidService
{
    private readonly IAuctionRepository auctionRepository;
    private readonly IBidRepository bidRepository;
    private readonly UserManager<ApplicationUser> userManager;

    public BidService(
        IAuctionRepository auctionRepository,
        IBidRepository bidRepository,
        UserManager<ApplicationUser> userManager)
    {
        this.auctionRepository = auctionRepository;
        this.bidRepository = bidRepository;
        this.userManager = userManager;
    }

    public async Task<(bool Success, string Message)> PlaceBidAsync(BidViewModel model, string userId)
    {
        await auctionRepository.UpdateExpiredAuctionsAsync(DateTime.UtcNow);

        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return (false, "User was not found.");
        }

        if (user.IsBlocked)
        {
            return (false, "Your account is blocked and cannot place bids.");
        }

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

        if (auction.SellerId == userId)
        {
            return (false, "You cannot place a bid on your own auction.");
        }

        var difference = model.Amount - auction.CurrentPrice;
        var quotient = difference / auction.BidStep;
        var rounded = Math.Round(quotient, 6, MidpointRounding.AwayFromZero);
        var whole = Math.Abs(rounded - Math.Round(rounded)) < 0.000001m;
        if (!whole)
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
