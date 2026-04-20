using Microsoft.AspNetCore.Identity;

namespace AuctionHouse.Core.Models;

public class ApplicationUser : IdentityUser
{
    public bool IsBlocked { get; set; }

    public ICollection<Auction> Auctions { get; set; } = new List<Auction>();

    public ICollection<Bid> Bids { get; set; } = new List<Bid>();
}
