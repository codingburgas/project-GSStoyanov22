using AuctionHouse.Core.Models;
using AuctionHouse.Infrastructure;
using AuctionHouse.Infrastructure.Data;
using Microsoft.Data.Sqlite;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<ApplicationDbContext>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    await RecreateLegacySqliteDatabaseIfNeededAsync(dbContext);
    await dbContext.Database.MigrateAsync();
    await SeedData.SeedRolesAndAdminAsync(roleManager, userManager);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

static async Task RecreateLegacySqliteDatabaseIfNeededAsync(ApplicationDbContext dbContext)
{
    if (!dbContext.Database.IsSqlite())
    {
        return;
    }

    await using var connection = new SqliteConnection(dbContext.Database.GetConnectionString());
    await connection.OpenAsync();

    await using var hasHistoryTableCmd = connection.CreateCommand();
    hasHistoryTableCmd.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='__EFMigrationsHistory'";
    var hasHistoryTable = Convert.ToInt64(await hasHistoryTableCmd.ExecuteScalarAsync()) > 0;

    var appliedMigrations = 0L;
    if (hasHistoryTable)
    {
        await using var historyCountCmd = connection.CreateCommand();
        historyCountCmd.CommandText = "SELECT COUNT(*) FROM __EFMigrationsHistory";
        appliedMigrations = Convert.ToInt64(await historyCountCmd.ExecuteScalarAsync());
    }

    await using var hasAnyTableCmd = connection.CreateCommand();
    hasAnyTableCmd.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%'";
    var hasAnyTable = Convert.ToInt64(await hasAnyTableCmd.ExecuteScalarAsync()) > 0;

    await connection.CloseAsync();

    if (!hasAnyTable || (hasHistoryTable && appliedMigrations > 0))
    {
        return;
    }

    var builder = new SqliteConnectionStringBuilder(dbContext.Database.GetConnectionString());
    if (string.IsNullOrWhiteSpace(builder.DataSource))
    {
        return;
    }

    var databasePath = Path.GetFullPath(builder.DataSource, AppContext.BaseDirectory);
    if (File.Exists(databasePath))
    {
        File.Delete(databasePath);
    }
}
