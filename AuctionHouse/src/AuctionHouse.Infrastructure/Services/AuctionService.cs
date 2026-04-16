using AuctionHouse.Core.DTOs;
using AuctionHouse.Core.Interfaces;
using AuctionHouse.Core.Models;
using AuctionHouse.Infrastructure.Repositories;

namespace AuctionHouse.Infrastructure.Services;

public class AuctionService : IAuctionService
{
    private readonly IAuctionRepository auctionRepository;

    public AuctionService(IAuctionRepository auctionRepository)
    {
        this.auctionRepository = auctionRepository;
    }

    public async Task<AuctionViewModel> CreateAuctionAsync(AuctionViewModel model, string sellerId)
    {
        var entity = new Auction
        {
            Title = model.Title,
            Description = model.Description,
            StartingPrice = model.StartingPrice,
            CurrentPrice = model.StartingPrice,
            BidStep = model.BidStep,
            EndTime = model.EndTime,
            IsActive = true,
            SellerId = sellerId
        };

        await auctionRepository.AddAsync(entity);
        return Map(entity);
    }

    public async Task<bool> EndAuctionAsync(int auctionId, string requestUserId, bool isAdmin)
    {
        var auction = await auctionRepository.GetByIdAsync(auctionId);
        if (auction is null)
        {
            return false;
        }

        if (!isAdmin && auction.SellerId != requestUserId)
        {
            return false;
        }

        auction.IsActive = false;
        await auctionRepository.UpdateAsync(auction);
        return true;
    }

    public async Task<IEnumerable<AuctionViewModel>> GetActiveAuctionsAsync()
    {
        var auctions = await auctionRepository.GetActiveAuctionsAsync();
        return auctions.Select(Map);
    }

    public async Task<IEnumerable<AuctionViewModel>> GetAllAuctionsAsync()
    {
        var auctions = await auctionRepository.GetAllAsync();
        return auctions.Select(Map);
    }

    public async Task<AuctionViewModel?> GetAuctionByIdAsync(int id)
    {
        var auction = await auctionRepository.GetByIdAsync(id);
        return auction is null ? null : Map(auction);
    }

    public async Task<bool> UpdateAuctionAsync(AuctionViewModel model, string requestUserId, bool isAdmin)
    {
        var auction = await auctionRepository.GetByIdAsync(model.Id);
        if (auction is null)
        {
            return false;
        }

        if (!isAdmin && auction.SellerId != requestUserId)
        {
            return false;
        }

        auction.Title = model.Title;
        auction.Description = model.Description;
        auction.StartingPrice = model.StartingPrice;
        auction.BidStep = model.BidStep;
        auction.EndTime = model.EndTime;

        await auctionRepository.UpdateAsync(auction);
        return true;
    }

    public async Task<bool> DeleteAuctionAsync(int id, string requestUserId, bool isAdmin)
    {
        var auction = await auctionRepository.GetByIdAsync(id);
        if (auction is null)
        {
            return false;
        }

        if (!isAdmin && auction.SellerId != requestUserId)
        {
            return false;
        }

        await auctionRepository.DeleteAsync(auction);
        return true;
    }

    private static AuctionViewModel Map(Auction auction)
    {
        return new AuctionViewModel
        {
            Id = auction.Id,
            Title = auction.Title,
            Description = auction.Description,
            StartingPrice = auction.StartingPrice,
            CurrentPrice = auction.CurrentPrice,
            BidStep = auction.BidStep,
            EndTime = auction.EndTime,
            CreatedAt = auction.CreatedAt,
            IsActive = auction.IsActive,
            BidsCount = auction.Bids.Count,
            SellerId = auction.SellerId
        };
    }
}
