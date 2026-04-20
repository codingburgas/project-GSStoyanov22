using System.ComponentModel.DataAnnotations;

namespace AuctionHouse.Core.Models;

public class Auction : BaseEntity
{
    [Required]
    [StringLength(120)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal StartingPrice { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal CurrentPrice { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal BidStep { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    public bool IsActive { get; set; } = true;

    public string SellerId { get; set; } = string.Empty;

    public ApplicationUser? Seller { get; set; }

    public ICollection<Bid> Bids { get; set; } = new List<Bid>();
}
