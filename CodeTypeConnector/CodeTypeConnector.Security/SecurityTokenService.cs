using System.Security.Cryptography;

namespace CodeTypeConnector.Security
{
    public sealed class SecurityTokenService
    {
        public string GenerateToken()
        {
            // Generate 32 cryptographically secure random bytes.
            byte[] tokenBytes = RandomNumberGenerator.GetBytes(32);

            // Return a URL-safe token representation.
            return Convert.ToBase64String(tokenBytes)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }
    }
}