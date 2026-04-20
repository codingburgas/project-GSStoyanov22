namespace AuctionHouse.Core.DTOs;

public class ReportsViewModel
{
    public IReadOnlyCollection<AuctionViewModel> ActiveAuctions { get; set; } = [];

    public IReadOnlyCollection<AuctionViewModel> EndingSoonAuctions { get; set; } = [];

    public IReadOnlyCollection<AuctionViewModel> WonAuctions { get; set; } = [];

    public int TotalAuctions { get; set; }

    public int TotalActiveAuctions { get; set; }

    public int TotalClosedAuctions { get; set; }

    public int TotalBids { get; set; }
}
