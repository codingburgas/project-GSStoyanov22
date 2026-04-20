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
            EndTime = model.EndTime.ToUniversalTime(),
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
        await auctionRepository.UpdateExpiredAuctionsAsync(DateTime.UtcNow);

        var auctions = await auctionRepository.GetActiveAuctionsAsync();
        return auctions.Select(Map);
    }

    public async Task<IEnumerable<AuctionViewModel>> GetAllAuctionsAsync()
    {
        await auctionRepository.UpdateExpiredAuctionsAsync(DateTime.UtcNow);

        var auctions = await auctionRepository.GetAllAsync();
        return auctions.Select(Map);
    }

    public async Task<AuctionViewModel?> GetAuctionByIdAsync(int id)
    {
        await auctionRepository.UpdateExpiredAuctionsAsync(DateTime.UtcNow);

        var auction = await auctionRepository.GetByIdAsync(id);
        return auction is null ? null : Map(auction);
    }

    public async Task<AuctionDetailsViewModel?> GetAuctionDetailsByIdAsync(int id)
    {
        await auctionRepository.UpdateExpiredAuctionsAsync(DateTime.UtcNow);

        var auction = await auctionRepository.GetByIdAsync(id);
        if (auction is null)
        {
            return null;
        }

        return new AuctionDetailsViewModel
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
            SellerId = auction.SellerId,
            BidHistory = auction.Bids
                .OrderByDescending(b => b.CreatedAt)
                .Select(b => new BidHistoryItemViewModel
                {
                    Bidder = b.User?.UserName ?? "Unknown",
                    Amount = b.Amount,
                    Time = b.CreatedAt
                })
                .ToArray()
        };
    }

    public async Task<UserDashboardViewModel> GetUserDashboardAsync(string userId)
    {
        await auctionRepository.UpdateExpiredAuctionsAsync(DateTime.UtcNow);

        var userAuctions = (await auctionRepository.GetBySellerIdAsync(userId)).ToList();
        var activeBidAuctions = (await auctionRepository.GetAuctionsWhereUserHasBidsAsync(userId)).ToList();
        var allAuctions = (await auctionRepository.GetAllAsync()).ToList();

        var wonAuctions = allAuctions
            .Where(a => !a.IsActive && a.EndTime <= DateTime.UtcNow)
            .Where(a => a.Bids
                .OrderByDescending(b => b.Amount)
                .ThenByDescending(b => b.CreatedAt)
                .FirstOrDefault()?.UserId == userId)
            .ToList();

        var currentlyActiveBids = activeBidAuctions
            .Where(a => a.IsActive && a.EndTime > DateTime.UtcNow)
            .ToList();

        return new UserDashboardViewModel
        {
            UserAuctions = userAuctions.Select(Map).ToArray(),
            WonAuctions = wonAuctions.Select(Map).ToArray(),
            ActiveBids = currentlyActiveBids.Select(Map).ToArray()
        };
    }

    public async Task<ReportsViewModel> GetReportsAsync()
    {
        await auctionRepository.UpdateExpiredAuctionsAsync(DateTime.UtcNow);

        var allAuctions = (await auctionRepository.GetAllAsync()).ToList();

        var activeAuctions = allAuctions
            .Where(a => a.IsActive && a.EndTime > DateTime.UtcNow)
            .OrderBy(a => a.EndTime)
            .ToList();

        var endingSoon = allAuctions
            .Where(a => a.IsActive && a.EndTime > DateTime.UtcNow && a.EndTime <= DateTime.UtcNow.AddHours(24))
            .OrderBy(a => a.EndTime)
            .ToList();

        var wonAuctions = allAuctions
            .Where(a => !a.IsActive && a.EndTime <= DateTime.UtcNow)
            .OrderByDescending(a => a.EndTime)
            .ToList();

        return new ReportsViewModel
        {
            ActiveAuctions = activeAuctions.Select(Map).ToArray(),
            EndingSoonAuctions = endingSoon.Select(Map).ToArray(),
            WonAuctions = wonAuctions.Select(Map).ToArray(),
            TotalAuctions = allAuctions.Count,
            TotalActiveAuctions = activeAuctions.Count,
            TotalClosedAuctions = wonAuctions.Count,
            TotalBids = await auctionRepository.GetTotalBidsCountAsync()
        };
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
        auction.EndTime = model.EndTime.ToUniversalTime();

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
