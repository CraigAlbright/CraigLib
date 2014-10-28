using Amazon.AutoScaling;
using Amazon.AWSSupport;
using Amazon.CloudFormation;
using Amazon.CloudFront;
using Amazon.CloudSearch;
using Amazon.CloudSearchDomain;
using Amazon.CloudTrail;
using Amazon.CloudWatch;
using Amazon.CloudWatchLogs;
using Amazon.CognitoIdentity;
using Amazon.CognitoSync;
using Amazon.DataPipeline;
using Amazon.DirectConnect;
using Amazon.DynamoDBv2;
using Amazon.EC2;
using Amazon.ElastiCache;
using Amazon.ElasticBeanstalk;
using Amazon.ElasticLoadBalancing;
using Amazon.ElasticMapReduce;
using Amazon.ElasticTranscoder;
using Amazon.Glacier;
using Amazon.IdentityManagement;
using Amazon.ImportExport;
using Amazon.Kinesis;
using Amazon.OpsWorks;
using Amazon.RDS;
using Amazon.Redshift;
using Amazon.Route53;
using Amazon.S3;
using Amazon.SecurityToken;
using Amazon.SimpleDB;
using Amazon.SimpleEmail;
using Amazon.SimpleNotificationService;
using Amazon.SimpleWorkflow;
using Amazon.SQS;
using Amazon.StorageGateway;

namespace CraigLib.Cloud.AWS
{
    public class AWSHelper : IAWSHelper
    {
        public AmazonConfiguration GetAmazonConfiguration()
        {
            return (AmazonConfiguration)System.Configuration.ConfigurationManager.GetSection("AmazonConfigurationGroup");
        }
        public AmazonAutoScalingClient GetAmazonAutoScalingClient()
        {
            return new AmazonAutoScalingClient();
        }

        public AmazonAWSSupportClient GetAWSSupportClient()
        {
            return new AmazonAWSSupportClient();
        }

        public AmazonCloudFormationClient GetAmazonCloudFormationClient()
        {
            return new AmazonCloudFormationClient();
        }

        public AmazonCloudFrontClient GetCloudFrontClient()
        {
            return new AmazonCloudFrontClient();
        }

        public AmazonCloudSearchClient GetAmazonCloudSearchClient()
        {
            return new AmazonCloudSearchClient();
        }

        public AmazonCloudSearchDomainClient GetAmazonCloudSearchDomainClient(string serviceUrl)
        {
            return new AmazonCloudSearchDomainClient(serviceUrl);
        }

        public AmazonCloudTrailClient GetCloudTrailClient()
        {
            return new AmazonCloudTrailClient();
        }

        public AmazonCloudWatchClient GetCloudWatchClient()
        {
            return new AmazonCloudWatchClient();
        }

        public AmazonCloudWatchLogsClient GetAmazonCloudWatchLogsClient()
        {
            return new AmazonCloudWatchLogsClient();
        }

        public AmazonCognitoIdentityClient GetAmazonCognitoIdentityClient()
        {
            return new AmazonCognitoIdentityClient();
        }

        public AmazonCognitoSyncClient GetCognitoSyncClient()
        {
            return new AmazonCognitoSyncClient();
        }

        public AmazonDataPipelineClient GetDataPipelineClient()
        {
            return new AmazonDataPipelineClient();
        }

        public AmazonDirectConnectClient GetAmazonDirectConnectClient()
        {
            return new AmazonDirectConnectClient();
        }

        public AmazonDynamoDBClient GetAmazonDynamoDbClient()
        {
            return new AmazonDynamoDBClient();
        }

        public AmazonEC2Client GetAmazonEc2Client()
        {
            return new AmazonEC2Client();
        }

        public AmazonElastiCacheClient GetAmazonElastiCacheClient()
        {
            return new AmazonElastiCacheClient();
        }

        public AmazonElasticBeanstalkClient GetAmazonElasticBeanstalkClient()
        {
            return new AmazonElasticBeanstalkClient();
        }

        public AmazonElasticLoadBalancingClient GetAmazonElasticLoadBalancingClient()
        {
            return new AmazonElasticLoadBalancingClient();
        }

        public AmazonElasticMapReduceClient GetAmazonElasticMapReduceClient()
        {
            return new AmazonElasticMapReduceClient();
        }

        public AmazonElasticTranscoderClient GeElasticTranscoderClient()
        {
            return new AmazonElasticTranscoderClient();
        }

        public AmazonGlacierClient GetAmazonGlacierClient()
        {
            return new AmazonGlacierClient();
        }

        public AmazonIdentityManagementServiceClient GetAmazonIdentityManagementServiceClient()
        {
            return new AmazonIdentityManagementServiceClient();
        }

        public AmazonImportExportClient GetAmazonImportExportClient()
        {
            return new AmazonImportExportClient();
        }

        public AmazonKinesisClient GetAmazonKinesisClient()
        {
            return new AmazonKinesisClient();
        }

        public AmazonOpsWorksClient GetAmazonOpsWorksClient()
        {
            return new AmazonOpsWorksClient();
        }

        public AmazonRDSClient GetAmazonRdsClient()
        {
            return new AmazonRDSClient();
        }

        public AmazonRedshiftClient GetAmazonRedshiftClient()
        {
            return new AmazonRedshiftClient();
        }

        public AmazonRoute53Client GetAmazonRoute53Client()
        {
            return new AmazonRoute53Client();
        }

        public AmazonS3Client GetS3Client()
        {
            return new AmazonS3Client();
        }

        public AmazonSecurityTokenServiceClient GetAmazonSecurityTokenServiceClient()
        {
            return new AmazonSecurityTokenServiceClient();
        }

        public AmazonSimpleDBClient GetAmazonSimpleDbClient()
        {
            return new AmazonSimpleDBClient();
        }

        public AmazonSimpleEmailServiceClient GetAmazonSimpleEmailServiceClient()
        {
            return new AmazonSimpleEmailServiceClient();
        }

        public AmazonSimpleNotificationServiceClient GetAmazonSimpleNotificationServiceClient()
        {
            return new AmazonSimpleNotificationServiceClient();
        }

        public AmazonSimpleWorkflowClient GetAmazonSimpleWorkflowClient()
        {
            return new AmazonSimpleWorkflowClient();
        }

        public AmazonSQSClient GetAmazonSqsClient()
        {
            return new AmazonSQSClient();
        }

        public AmazonStorageGatewayClient GetAmazonStorageGatewayClient()
        {
            return new AmazonStorageGatewayClient();
        }
    }
}
