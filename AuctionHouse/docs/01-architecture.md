# Architecture

- `AuctionHouse.Core`: entities, DTOs, service contracts.
- `AuctionHouse.Infrastructure`: EF Core, Identity context, repositories, services.
- `AuctionHouse.Web`: MVC controllers, views, static assets, DI composition.

The solution follows clean architecture by keeping web concerns outside domain contracts and isolating data access in infrastructure.
