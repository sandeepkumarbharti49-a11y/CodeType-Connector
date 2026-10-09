using CodeTypeConnector.Connectors;
using Xunit;

namespace CodeTypeConnector.Connectors.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void DetectInstalled_ReturnsValidInstallationInformation()
        {
            var installations = VisualStudioDetector.DetectInstalled();

            Assert.NotNull(installations);

            foreach (var installation in installations)
            {
                Assert.False(string.IsNullOrWhiteSpace(installation.Version));
                Assert.False(string.IsNullOrWhiteSpace(installation.InstallationPath));
                Assert.False(string.IsNullOrWhiteSpace(installation.InstanceId));

                Assert.True(
                    System.IO.Path.IsPathFullyQualified(
                        installation.InstallationPath));
            }
        }
    }
}