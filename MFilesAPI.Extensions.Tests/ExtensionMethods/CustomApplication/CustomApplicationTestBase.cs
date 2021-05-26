using Moq;

namespace MFilesAPI.Extensions.Tests.ExtensionMethods.CustomApplication
{
    public abstract class CustomApplicationTestBase
    {
        protected virtual Mock<Vault> GetVaultMock()
        {
            return new Mock<Vault>();
        }
        protected virtual Mock<MFilesAPI.CustomApplication> GetCustomApplicationMock()
        {
            return new Mock<MFilesAPI.CustomApplication>();
        }
        protected virtual Mock<VaultCustomApplicationManagementOperations> GetVaultCustomApplicationManagementOperationsMock()
        {
            return new Mock<VaultCustomApplicationManagementOperations>();
        }
    }
}
