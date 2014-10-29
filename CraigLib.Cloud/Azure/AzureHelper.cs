using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management;
using Microsoft.WindowsAzure.Management.Compute;
using Microsoft.WindowsAzure.Management.Sql;
using Microsoft.WindowsAzure.Management.Storage;
using Microsoft.WindowsAzure.Management.TrafficManager;
using Microsoft.WindowsAzure.Management.WebSites;

namespace CraigLib.Cloud.Azure
{
    public class AzureHelper : IAzureHelper
    {
        private readonly SubscriptionCloudCredentials _credentials;

        public AzureHelper()
        {
            var config = new AzureConfiguration();
            var token = config.AzureToken;
            _credentials = new TokenCloudCredentials(token);

        }
        public ManagementClient GetManagementClient()
        {
            return new ManagementClient(_credentials);
        }

        public ComputeManagementClient GetComputeManagementClient()
        {
            return new ComputeManagementClient(_credentials);
        }

        public SqlManagementClient GetSqlManagementClient()
        {
            return new SqlManagementClient(_credentials);
        }

        public StorageManagementClient GetStorageManagementClient()
        {
            return new StorageManagementClient(_credentials);
        }

        public TrafficManagerManagementClient GeTrafficManagerManagementClient()
        {
            return new TrafficManagerManagementClient(_credentials);
        }

        public WebSiteManagementClient GetWebSiteManagementClient()
        {
            return new WebSiteManagementClient(_credentials);
        }
    }
}
