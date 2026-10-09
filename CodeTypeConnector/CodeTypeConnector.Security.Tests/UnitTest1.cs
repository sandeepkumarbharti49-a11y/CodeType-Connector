using CodeTypeConnector.Security;

namespace CodeTypeConnector.Security.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void IsValid_NullToken_ReturnsFalse()
        {
            var validator = new SecurityTokenValidator();

            Assert.False(validator.IsValid(null));
        }

        [Fact]
        public void IsValid_EmptyToken_ReturnsFalse()
        {
            var validator = new SecurityTokenValidator();

            Assert.False(validator.IsValid(""));
        }

        [Fact]
        public void IsValid_WhitespaceToken_ReturnsFalse()
        {
            var validator = new SecurityTokenValidator();

            Assert.False(validator.IsValid("   "));
        }

        [Fact]
        public void IsValid_ShortToken_ReturnsFalse()
        {
            var validator = new SecurityTokenValidator();

            Assert.False(validator.IsValid("short-token"));
        }

        [Fact]
        public void IsValid_LongToken_ReturnsFalse()
        {
            var validator = new SecurityTokenValidator();

            Assert.False(validator.IsValid(new string('A', 4097)));
        }

        [Fact]
        public void IsValid_32CharacterToken_ReturnsFalseUntilCryptographicValidationExists()
        {
            var validator = new SecurityTokenValidator();

            Assert.False(validator.IsValid(new string('A', 32)));
        }

        [Fact]
        public void GenerateToken_ReturnsNonEmptyToken()
        {
            var service = new SecurityTokenService();

            string token = service.GenerateToken();

            Assert.False(string.IsNullOrWhiteSpace(token));
        }

        [Fact]
        public void GenerateToken_CreatesUniqueTokens()
        {
            var service = new SecurityTokenService();

            string firstToken = service.GenerateToken();
            string secondToken = service.GenerateToken();

            Assert.NotEqual(firstToken, secondToken);
        }

        [Fact]
        public void SecurityTokenRecord_NewRecord_IsNotRevoked()
        {
            var record = new SecurityTokenRecord();

            Assert.False(record.IsRevoked);
            Assert.Null(record.RevokedAtUtc);
        }

        [Fact]
        public void SecurityTokenRecord_ExpiredAtCurrentTime_ReturnsTrue()
        {
            var now = DateTime.UtcNow;

            var record = new SecurityTokenRecord
            {
                CreatedAtUtc = now.AddMinutes(-10),
                ExpiresAtUtc = now
            };

            Assert.True(record.IsExpired(now));
        }

        [Fact]
        public void SecurityTokenRecord_RevokedAtUtc_IsRevoked()
        {
            var record = new SecurityTokenRecord
            {
                RevokedAtUtc = DateTime.UtcNow
            };

            Assert.True(record.IsRevoked);
        }
    }
}