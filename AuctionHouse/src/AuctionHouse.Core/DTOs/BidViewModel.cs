using System.ComponentModel.DataAnnotations;

namespace AuctionHouse.Core.DTOs;

public class BidViewModel
{
    [Required]
    public int AuctionId { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }
}
