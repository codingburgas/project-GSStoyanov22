using System.ComponentModel.DataAnnotations;

namespace AuctionHouse.Core.DTOs;

public class BidViewModel
{
    public int AuctionId { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }
}
