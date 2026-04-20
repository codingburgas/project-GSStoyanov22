namespace AuctionHouse.Core.DTOs;

public class AuctionDetailsViewModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal StartingPrice { get; set; }

    public decimal CurrentPrice { get; set; }

    public decimal BidStep { get; set; }

    public DateTime EndTime { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsActive { get; set; }

    public int BidsCount { get; set; }

    public string? SellerId { get; set; }

    public bool IsExpired => EndTime <= DateTime.UtcNow;

    public IReadOnlyCollection<BidHistoryItemViewModel> BidHistory { get; set; } = [];
}
