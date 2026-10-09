using CodeTypeConnector.Security;
using CodeTypeConnector.Server.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CodeTypeConnector.Server.Tests
{
    public class UnitTest1
    {
        private static async Task<(SqliteConnection Connection, SecurityDbContext DbContext)>
            CreateTestDatabaseAsync()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<SecurityDbContext>()
                .UseSqlite(connection)
                .Options;

            var dbContext = new SecurityDbContext(options);
            await dbContext.Database.EnsureCreatedAsync();

            return (connection, dbContext);
        }

        [Fact]
        public async Task CreateAsync_ShouldStoreHashInsteadOfRawToken()
        {
            var (connection, dbContext) = await CreateTestDatabaseAsync();

            await using (connection)
            await using (dbContext)
            {
                var store = new SecurityTokenStore(
                    dbContext,
                    new SecurityTokenService());

                string token = await store.CreateAsync(
                    "Studio-PC-01",
                    TimeSpan.FromHours(1));

                var record = await dbContext.SecurityTokens
                    .SingleAsync();

                Assert.False(string.IsNullOrWhiteSpace(token));
                Assert.NotEqual(token, record.TokenHash);
                Assert.Equal(64, record.TokenHash.Length);
                Assert.Equal("Studio-PC-01", record.ComputerId);
                Assert.True(record.ExpiresAtUtc > record.CreatedAtUtc);
            }
        }

        [Fact]
        public async Task ValidateAsync_ShouldAcceptValidTokenAndRejectInvalidToken()
        {
            var (connection, dbContext) = await CreateTestDatabaseAsync();

            await using (connection)
            await using (dbContext)
            {
                var store = new SecurityTokenStore(
                    dbContext,
                    new SecurityTokenService());

                string token = await store.CreateAsync(
                    "Studio-PC-01",
                    TimeSpan.FromHours(1));

                bool validTokenResult = await store.ValidateAsync(
                    token,
                    "Studio-PC-01");

                bool invalidTokenResult = await store.ValidateAsync(
                    "invalid-token-value-which-is-long-enough",
                    "Studio-PC-01");

                Assert.True(validTokenResult);
                Assert.False(invalidTokenResult);
            }
        }

        [Fact]
        public async Task ValidateAsync_ShouldRejectWrongComputerId()
        {
            var (connection, dbContext) = await CreateTestDatabaseAsync();

            await using (connection)
            await using (dbContext)
            {
                var store = new SecurityTokenStore(
                    dbContext,
                    new SecurityTokenService());

                string token = await store.CreateAsync(
                    "Studio-PC-01",
                    TimeSpan.FromHours(1));

                bool result = await store.ValidateAsync(
                    token,
                    "Studio-PC-02");

                Assert.False(result);
            }
        }

        [Fact]
        public async Task RevokeAsync_ShouldRejectTokenAfterRevocation()
        {
            var (connection, dbContext) = await CreateTestDatabaseAsync();

            await using (connection)
            await using (dbContext)
            {
                var store = new SecurityTokenStore(
                    dbContext,
                    new SecurityTokenService());

                string token = await store.CreateAsync(
                    "Studio-PC-01",
                    TimeSpan.FromHours(1));

                bool revokeResult = await store.RevokeAsync(token);

                bool validateResult = await store.ValidateAsync(
                    token,
                    "Studio-PC-01");

                Assert.True(revokeResult);
                Assert.False(validateResult);
            }
        }

        [Fact]
        public async Task ValidateAsync_ShouldRejectExpiredToken()
        {
            var (connection, dbContext) = await CreateTestDatabaseAsync();

            await using (connection)
            await using (dbContext)
            {
                var store = new SecurityTokenStore(
                    dbContext,
                    new SecurityTokenService());

                string token = await store.CreateAsync(
                    "Studio-PC-01",
                    TimeSpan.FromHours(1));

                var record = await dbContext.SecurityTokens
                    .SingleAsync();

                record.ExpiresAtUtc = DateTime.UtcNow.AddMinutes(-1);

                await dbContext.SaveChangesAsync();

                bool result = await store.ValidateAsync(
                    token,
                    "Studio-PC-01");

                Assert.False(result);
            }
        }
    }
}