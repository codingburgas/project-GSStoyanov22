# Repository Layer

Repositories encapsulate data access and keep EF Core queries out of controllers/services.

- `IAuctionRepository` + `AuctionRepository`
- `IBidRepository` + `BidRepository`

LINQ is used for filtering active auctions, ordering, eager loading, and query projections.
