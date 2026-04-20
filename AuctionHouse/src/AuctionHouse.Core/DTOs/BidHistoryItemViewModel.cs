namespace AuctionHouse.Core.DTOs;

public class BidHistoryItemViewModel
{
    public string Bidder { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public DateTime Time { get; set; }
}
