# Service Layer

`AuctionService` handles auction business workflows and owner/admin permissions.

`BidService` enforces bid rules:
- auction must be active
- bid must exceed current price
- increment must match bid step

Services map entities to DTOs and coordinate repositories.
