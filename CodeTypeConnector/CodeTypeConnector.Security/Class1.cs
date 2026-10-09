namespace CodeTypeConnector.Security
{
    public sealed class SecurityTokenValidator
    {
        public bool IsValid(string? token)
        {
            // Reject null, empty, or whitespace-only tokens.
            if (string.IsNullOrWhiteSpace(token))
            {
                return false;
            }

            // Reject tokens that are too short or too long.
            if (token.Length < 32 || token.Length > 4096)
            {
                return false;
            }

            // A non-empty token is not proof of authenticity.
            // Cryptographic validation will be added in the next step.
            return false;
        }
    }
}