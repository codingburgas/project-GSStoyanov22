# Getting Started

## Prerequisites

- .NET 8 SDK

## Run Locally

1. From repository root:
   - `cd AuctionHouse`
2. Restore and build:
   - `dotnet restore`
   - `dotnet build`
3. Apply migrations:
   - `dotnet ef database update --project src/AuctionHouse.Infrastructure --startup-project src/AuctionHouse.Web`
4. Run:
   - `dotnet run --project src/AuctionHouse.Web/AuctionHouse.Web.csproj`
5. Open:
   - `https://localhost:7212`

## Notes

- On startup the app runs migrations and seed logic.
- If no legacy migration history exists in a SQLite file, startup may recreate the database to avoid schema drift.

