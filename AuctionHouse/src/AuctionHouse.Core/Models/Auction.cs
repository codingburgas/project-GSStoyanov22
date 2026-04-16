namespace AuctionHouse.Core.Models;

public class Auction : BaseEntity
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal StartingPrice { get; set; }

    public decimal CurrentPrice { get; set; }

    public decimal BidStep { get; set; }

    public DateTime EndTime { get; set; }

    public bool IsActive { get; set; } = true;

    public string SellerId { get; set; } = string.Empty;

    public ApplicationUser? Seller { get; set; }

    public ICollection<Bid> Bids { get; set; } = new List<Bid>();
}
