namespace CodeTypeConnector.Security
{
    public sealed class SecurityTokenRecord
    {
        public string TokenHash { get; set; } = string.Empty;

        public string ComputerId { get; set; } = string.Empty;

        public DateTime CreatedAtUtc { get; set; }

        public DateTime ExpiresAtUtc { get; set; }

        public DateTime? RevokedAtUtc { get; set; }

        public bool IsRevoked => RevokedAtUtc.HasValue;

        public bool IsExpired(DateTime utcNow)
        {
            return utcNow >= ExpiresAtUtc;
        }
    }

}