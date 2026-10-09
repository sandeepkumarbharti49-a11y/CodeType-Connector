using CodeTypeConnector.Permissions;

namespace CodeTypeConnector.Permissions.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void IsAllowed_NullPermissions_ReturnsFalse()
        {
            bool result = PermissionValidator.IsAllowed(
                null,
                ConnectorPermission.ReadFile);

            Assert.False(result);
        }

        [Fact]
        public void IsAllowed_EmptyPermissions_ReturnsFalse()
        {
            bool result = PermissionValidator.IsAllowed(
                Array.Empty<ConnectorPermission>(),
                ConnectorPermission.ReadFile);

            Assert.False(result);
        }

        [Fact]
        public void IsAllowed_GrantedPermission_ReturnsTrue()
        {
            bool result = PermissionValidator.IsAllowed(
                new[] { ConnectorPermission.ReadFile },
                ConnectorPermission.ReadFile);

            Assert.True(result);
        }

        [Fact]
        public void IsAllowed_NotGrantedPermission_ReturnsFalse()
        {
            bool result = PermissionValidator.IsAllowed(
                new[] { ConnectorPermission.ReadFile },
                ConnectorPermission.DeleteFile);

            Assert.False(result);
        }

        [Fact]
        public void IsAllowed_UnknownPermission_ReturnsFalse()
        {
            bool result = PermissionValidator.IsAllowed(
                new[] { ConnectorPermission.ReadFile },
                (ConnectorPermission)999);

            Assert.False(result);
        }
    }
}