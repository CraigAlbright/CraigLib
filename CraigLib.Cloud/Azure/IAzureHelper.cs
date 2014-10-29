using Microsoft.WindowsAzure.Management;
using Microsoft.WindowsAzure.Management.Compute;
using Microsoft.WindowsAzure.Management.Sql;
using Microsoft.WindowsAzure.Management.Storage;
using Microsoft.WindowsAzure.Management.TrafficManager;
using Microsoft.WindowsAzure.Management.WebSites;

namespace CraigLib.Cloud.Azure
{
    public interface IAzureHelper
    {
        ManagementClient GetManagementClient();
        ComputeManagementClient GetComputeManagementClient();
        SqlManagementClient GetSqlManagementClient();
        StorageManagementClient GetStorageManagementClient();
        TrafficManagerManagementClient GeTrafficManagerManagementClient();
        WebSiteManagementClient GetWebSiteManagementClient();
    }
}
