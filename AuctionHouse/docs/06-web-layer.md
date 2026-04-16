# Web Layer

Controllers:
- `HomeController`
- `AuctionController`
- `BidController`
- `AdminController`

Authorization strategy:
- Authenticated users can create auctions and place bids.
- Only owner or admin can edit/delete.
- Admin-only panel can end auctions.
