using Microsoft.Win32;

namespace CodeTypeConnector.Connectors
{
    public sealed record VisualStudioInstallation(
        string Version,
        string InstallationPath,
        string InstanceId);

    public static class VisualStudioDetector
    {
        private const string SetupInstancesKey =
            @"SOFTWARE\Microsoft\VisualStudio\Setup\Instances";

        public static IReadOnlyList<VisualStudioInstallation> DetectInstalled()
        {
            var installations = new List<VisualStudioInstallation>();
            var seenPaths = new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

            string[] registryPaths =
            {
                SetupInstancesKey,
                @"SOFTWARE\WOW6432Node\Microsoft\VisualStudio\Setup\Instances"
            };

            foreach (string registryPath in registryPaths)
            {
                try
                {
                    using RegistryKey? instancesKey =
                        Registry.LocalMachine.OpenSubKey(registryPath);

                    if (instancesKey is null)
                        continue;

                    foreach (string instanceName in instancesKey.GetSubKeyNames())
                    {
                        try
                        {
                            using RegistryKey? instanceKey =
                                instancesKey.OpenSubKey(instanceName);

                            if (instanceKey is null)
                                continue;

                            string? version =
                                instanceKey.GetValue("installationVersion") as string;

                            string? installationPath =
                                instanceKey.GetValue("installationPath") as string;

                            if (string.IsNullOrWhiteSpace(version) ||
                                string.IsNullOrWhiteSpace(installationPath))
                            {
                                continue;
                            }

                            if (!Path.IsPathFullyQualified(installationPath) ||
                                !Directory.Exists(installationPath))
                            {
                                continue;
                            }

                            string normalizedPath =
                                Path.GetFullPath(installationPath);

                            if (!seenPaths.Add(normalizedPath))
                                continue;

                            installations.Add(
                                new VisualStudioInstallation(
                                    version,
                                    normalizedPath,
                                    instanceName));
                        }
                        catch (System.Security.SecurityException)
                        {
                            // Do not bypass registry access restrictions.
                        }
                        catch (UnauthorizedAccessException)
                        {
                            // Skip instances that cannot be read.
                        }
                    }
                }
                catch (System.Security.SecurityException)
                {
                    // Access denied: do not attempt an elevated operation.
                }
                catch (UnauthorizedAccessException)
                {
                    // Access denied: continue without this registry location.
                }
            }

            return installations
                .OrderByDescending(item => item.Version,
                    StringComparer.OrdinalIgnoreCase)
                .ToArray();
        }
    }
}