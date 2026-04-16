# Data Model

Core entities:
- `BaseEntity`: `Id`, `CreatedAt`
- `ApplicationUser`: Identity user + navigation collections.
- `Auction`: seller-owned listing with current price and bid step.
- `Bid`: linked to auction and user with amount.

Relationships are configured in `ApplicationDbContext` with explicit delete behaviors.
