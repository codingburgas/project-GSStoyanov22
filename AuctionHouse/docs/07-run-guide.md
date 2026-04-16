# Local Run Guide

1. Install .NET 8 SDK and SQL Server / LocalDB.
2. Open `AuctionHouse/AuctionHouse.sln`.
3. Restore packages and apply migrations.
4. Run `AuctionHouse.Web`.
5. Login as `admin@auction.com` / `Admin123!` after initial seed.

Connection string is configured in `src/AuctionHouse.Web/appsettings.json`.
