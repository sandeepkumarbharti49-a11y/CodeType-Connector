using System.Security.Cryptography;
using System.Text;
using CodeTypeConnector.Security;
using Microsoft.EntityFrameworkCore;

namespace CodeTypeConnector.Server.Data
{
    public sealed class SecurityTokenStore
    {
        private readonly SecurityDbContext _dbContext;
        private readonly SecurityTokenService _tokenService;

        public SecurityTokenStore(
            SecurityDbContext dbContext,
            SecurityTokenService tokenService)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
        }

        public async Task<string> CreateAsync(
            string computerId,
            TimeSpan lifetime,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(computerId))
            {
                throw new ArgumentException(
                    "Computer ID is required.",
                    nameof(computerId));
            }

            computerId = computerId.Trim();

            if (computerId.Length > 128)
            {
                throw new ArgumentException(
                    "Computer ID cannot exceed 128 characters.",
                    nameof(computerId));
            }

            if (lifetime <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(lifetime),
                    "Token lifetime must be greater than zero.");
            }

            string token = _tokenService.GenerateToken();
            DateTime now = DateTime.UtcNow;

            var record = new SecurityTokenRecord
            {
                TokenHash = ComputeHash(token),
                ComputerId = computerId,
                CreatedAtUtc = now,
                ExpiresAtUtc = now.Add(lifetime),
                RevokedAtUtc = null
            };

            _dbContext.SecurityTokens.Add(record);

            await _dbContext.SaveChangesAsync(cancellationToken);

            // Return the secret only to the caller.
            // The database stores only its hash.
            return token;
        }

        public async Task<bool> ValidateAsync(
            string? token,
            string computerId,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(token) ||
                token.Length < 32 ||
                token.Length > 4096)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(computerId))
            {
                return false;
            }

            computerId = computerId.Trim();

            if (computerId.Length > 128)
            {
                return false;
            }

            string tokenHash = ComputeHash(token);

            var record = await _dbContext.SecurityTokens
                .AsNoTracking()
                .SingleOrDefaultAsync(
                    item => item.TokenHash == tokenHash,
                    cancellationToken);

            if (record is null)
            {
                return false;
            }

            if (!string.Equals(
                    record.ComputerId,
                    computerId,
                    StringComparison.Ordinal))
            {
                return false;
            }

            if (record.IsRevoked || record.IsExpired(DateTime.UtcNow))
            {
                return false;
            }

            return true;
        }

        public async Task<bool> RevokeAsync(
            string? token,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(token) ||
                token.Length < 32 ||
                token.Length > 4096)
            {
                return false;
            }

            string tokenHash = ComputeHash(token);

            var record = await _dbContext.SecurityTokens
                .SingleOrDefaultAsync(
                    item => item.TokenHash == tokenHash,
                    cancellationToken);

            if (record is null || record.IsRevoked)
            {
                return false;
            }

            record.RevokedAtUtc = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        private static string ComputeHash(string token)
        {
            byte[] tokenBytes = Encoding.UTF8.GetBytes(token);
            byte[] hashBytes = SHA256.HashData(tokenBytes);

            return Convert.ToHexString(hashBytes);
        }
    }

}