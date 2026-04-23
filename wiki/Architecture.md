# Architecture

The solution follows layered architecture with clear separation of concerns.

## Projects

- `AuctionHouse.Core`
- Domain models (`Auction`, `Bid`, `ApplicationUser`)
- DTOs and service interfaces
- `AuctionHouse.Infrastructure`
- `ApplicationDbContext`, EF Core migrations
- Repository implementations
- Service implementations
- Identity role/admin seeding
- `AuctionHouse.Web`
- MVC controllers and Razor views
- Authentication/authorization flow
- Static assets (CSS/JS)

## Dependency Flow

- Web depends on Core + Infrastructure.
- Infrastructure depends on Core.
- Core is independent of web and EF details.

## Startup Pipeline Highlights

- Registers infrastructure services via DI.
- Applies pending migrations at startup.
- Seeds roles (`Admin`, `User`) and default admin account.
- Enables authentication and authorization middleware.

