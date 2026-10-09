using CodeTypeConnector.Security;
using Microsoft.EntityFrameworkCore;

namespace CodeTypeConnector.Server.Data
{
    public sealed class SecurityDbContext : DbContext
    {
        public SecurityDbContext(
        DbContextOptions<SecurityDbContext> options)
        : base(options)
        {
        }

        public DbSet<SecurityTokenRecord> SecurityTokens =>
            Set<SecurityTokenRecord>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SecurityTokenRecord>(entity =>
            {
                entity.HasKey(token => token.TokenHash);

                entity.Property(token => token.TokenHash)
                    .HasMaxLength(64)
                    .IsRequired();

                entity.Property(token => token.ComputerId)
                    .HasMaxLength(128)
                    .IsRequired();

                entity.HasIndex(token => token.ComputerId);

                entity.HasIndex(token => token.ExpiresAtUtc);

                entity.HasIndex(token => token.RevokedAtUtc);
            });
        }
    }

}