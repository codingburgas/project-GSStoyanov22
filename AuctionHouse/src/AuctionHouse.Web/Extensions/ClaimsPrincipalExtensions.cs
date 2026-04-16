using System.Security.Claims;

namespace AuctionHouse.Web.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserIdOrThrow(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier)
               ?? throw new InvalidOperationException("User ID claim is missing.");
    }
}
