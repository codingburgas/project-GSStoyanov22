# Data Model

## Core Entities

- `BaseEntity`
- `Id`, `CreatedAt`
- `ApplicationUser`
- Identity user with additional navigation and block state support
- `Auction`
- Seller-owned listing with pricing and lifecycle fields
- `Bid`
- Bid record tied to auction and bidder

## Main Relationships

- `ApplicationUser (1) -> (many) Auction` (`Auction.SellerId`)
- `ApplicationUser (1) -> (many) Bid` (`Bid.UserId`)
- `Auction (1) -> (many) Bid` (`Bid.AuctionId`)

## Identity

ASP.NET Core Identity manages user and role tables (`AspNetUsers`, `AspNetRoles`, and related link tables).

## Seeded Security Data

- Roles:
- `Admin`
- `User`
- Default admin:
- `admin@auction.com` / `Admin123!`

