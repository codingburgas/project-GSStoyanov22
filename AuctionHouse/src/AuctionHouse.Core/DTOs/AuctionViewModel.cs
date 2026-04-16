using System.ComponentModel.DataAnnotations;

namespace AuctionHouse.Core.DTOs;

public class AuctionViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(120)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal StartingPrice { get; set; }

    public decimal CurrentPrice { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal BidStep { get; set; }

    public DateTime EndTime { get; set; }

    public bool IsActive { get; set; }

    public string? SellerId { get; set; }
}
