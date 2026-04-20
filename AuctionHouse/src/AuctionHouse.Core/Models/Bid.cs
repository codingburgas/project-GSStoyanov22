using System.ComponentModel.DataAnnotations;

namespace AuctionHouse.Core.Models;

public class Bid : BaseEntity
{
    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }

    public int AuctionId { get; set; }

    public Auction? Auction { get; set; }

    public string UserId { get; set; } = string.Empty;

    public ApplicationUser? User { get; set; }
}
