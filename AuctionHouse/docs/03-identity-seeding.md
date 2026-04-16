# Identity and Seeding

Identity is configured with `ApplicationUser` and roles.

Startup seed routine creates:
- Role `Admin`
- Role `User`
- Admin account `admin@auction.com` with password `Admin123!`

Seed logic runs in `Program.cs` through `SeedData.SeedRolesAndAdminAsync`.
