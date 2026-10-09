namespace CodeTypeConnector.Permissions
{
    public enum ConnectorPermission
    {
        ReadProject = 1,
        ReadFile = 2,
        CreateFile = 3,
        WriteFile = 4,
        DeleteFile = 5,
        BuildProject = 6,
        RunProject = 7,
        ExecuteCommand = 8,
        GitPush = 9
    }

    public static class PermissionValidator
    {
        public static bool IsAllowed(
            IEnumerable<ConnectorPermission>? grantedPermissions,
            ConnectorPermission requestedPermission)
        {
            // Unknown permissions must always be denied.
            if (!Enum.IsDefined(requestedPermission))
            {
                return false;
            }

            // No grants means no permission.
            if (grantedPermissions is null)
            {
                return false;
            }

            // Allow only a permission that was explicitly granted.
            return grantedPermissions.Contains(requestedPermission);
        }
    }
}