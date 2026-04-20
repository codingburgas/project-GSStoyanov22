namespace AuctionHouse.Core.DTOs;

public class UserDashboardViewModel
{
    public IReadOnlyCollection<AuctionViewModel> UserAuctions { get; set; } = [];

    public IReadOnlyCollection<AuctionViewModel> WonAuctions { get; set; } = [];

    public IReadOnlyCollection<AuctionViewModel> ActiveBids { get; set; } = [];
}
