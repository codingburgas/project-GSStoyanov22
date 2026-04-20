# AuctionHouse

AuctionHouse is an ASP.NET Core MVC auction platform where users can create auctions, place bids in valid bid steps, follow live countdown timers, review bid history, and track results in dashboard and reports.

## Overview

- Real-time style auction flow with countdown display (days, hours, minutes, seconds)
- Bid validation (higher-than-current, correct bid step, auction active only)
- Bid history on auction details (newest first)
- User dashboard with personal auctions, won auctions, and active bids
- Admin panel for auction moderation, user blocking, and total bid monitoring
- Reports for active, ending soon, and won auctions + statistics

## Technologies

- .NET 8
- ASP.NET Core MVC
- ASP.NET Core Identity
- Entity Framework Core 8
- SQLite (default local database)
- Bootstrap 5 + custom CSS/JS

## Setup

1. Install .NET 8 SDK.
2. From repository root:
   - `cd AuctionHouse`
   - `dotnet restore`
   - `dotnet build`
3. Apply database migrations:
   - `dotnet ef database update --project src/AuctionHouse.Infrastructure --startup-project src/AuctionHouse.Web`
4. Run the app:
   - `dotnet run --project src/AuctionHouse.Web/AuctionHouse.Web.csproj`
5. Open:
   - `https://localhost:7212`

## Roles

### User

- Register/Login
- Create, edit, delete own auctions
- Place bids on active auctions
- View dashboard and reports

### Admin

- Access Admin panel
- View all auctions
- End/remove auctions
- Block/unblock users
- View total bids and user status

Default seeded admin:

- Email: `admin@auction.com`
- Password: `Admin123!`

## Database Relations

- `ApplicationUser (1) -> (many) Auction` via `Auction.SellerId`
- `ApplicationUser (1) -> (many) Bid` via `Bid.UserId`
- `Auction (1) -> (many) Bid` via `Bid.AuctionId`
- Identity tables (`AspNetUsers`, `AspNetRoles`, etc.) are managed by ASP.NET Core Identity

## Validation

### Auction

- `[Required]` fields for essential data
- `[Range]` for `StartingPrice`, `CurrentPrice`, and `BidStep`
- End time must be in the future

### Bid

- `[Required]` + `[Range]` for bid amount
- Business validation on place bid:
  - Auction must be active and unexpired
  - Bid must be above current price
  - Bid must match configured bid step

## Testing Checklist

- Build succeeds
- Migrations apply successfully
- Register/Login/Logout works
- Bidding rules are enforced
- Countdown and expiration behavior works
- Admin moderation actions work
- Client/server validation messages are shown
