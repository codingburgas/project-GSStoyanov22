# Features and Roles

## User Features

- Register, login, logout
- Create auction listings
- Edit/delete own auctions
- Place bids on active auctions
- View auction details and bid history
- Access dashboard and reports

## Admin Features

- Access admin panel
- End auctions immediately
- Remove auctions
- Block/unblock users
- See total users and total bids

## Bid Validation Rules

`BidService` enforces:

- Auction must exist and be active
- Auction must not be expired
- Bid must be higher than current price
- Bid increment must follow configured bid step

## Authorization Rules

- Anonymous users can browse public pages and auctions.
- Authenticated users can create auctions and place bids.
- Owners or admins can edit/delete auction entries.
- Admin-only actions are guarded by `[Authorize(Roles = "Admin")]`.

