using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public interface IAWSHelper
    {
        AmazonConfiguration GetAmazonConfiguration();
        AmazonAutoScalingClient GetAmazonAutoScalingClient();
        AmazonAWSSupportClient GetAWSSupportClient();
        AmazonCloudFormationClient GetAmazonCloudFormationClient();
        AmazonCloudFrontClient GetCloudFrontClient();
        AmazonCloudSearchClient GetAmazonCloudSearchClient();
        AmazonCloudSearchDomainClient GetAmazonCloudSearchDomainClient(string serviceUrl);
        AmazonCloudTrailClient GetCloudTrailClient();
        AmazonCloudWatchClient GetCloudWatchClient();
        AmazonCloudWatchLogsClient GetAmazonCloudWatchLogsClient();
        AmazonCognitoIdentityClient GetAmazonCognitoIdentityClient();
        AmazonCognitoSyncClient GetCognitoSyncClient();
        AmazonDataPipelineClient GetDataPipelineClient();
        AmazonDirectConnectClient GetAmazonDirectConnectClient();
        AmazonDynamoDBClient GetAmazonDynamoDbClient();
        AmazonEC2Client GetAmazonEc2Client();
        AmazonElastiCacheClient GetAmazonElastiCacheClient();
        AmazonElasticBeanstalkClient GetAmazonElasticBeanstalkClient();
        AmazonElasticLoadBalancingClient GetAmazonElasticLoadBalancingClient();
        AmazonElasticMapReduceClient GetAmazonElasticMapReduceClient();
        AmazonElasticTranscoderClient GeElasticTranscoderClient();
        AmazonGlacierClient GetAmazonGlacierClient();
        AmazonIdentityManagementServiceClient GetAmazonIdentityManagementServiceClient();
        AmazonImportExportClient GetAmazonImportExportClient();
        AmazonKinesisClient GetAmazonKinesisClient();
        AmazonOpsWorksClient GetAmazonOpsWorksClient();
        AmazonRDSClient GetAmazonRdsClient();
        AmazonRedshiftClient GetAmazonRedshiftClient();
        AmazonRoute53Client GetAmazonRoute53Client();
        AmazonS3Client GetS3Client();
        AmazonSecurityTokenServiceClient GetAmazonSecurityTokenServiceClient();
        AmazonSimpleDBClient GetAmazonSimpleDbClient();
        AmazonSimpleEmailServiceClient GetAmazonSimpleEmailServiceClient();
        AmazonSimpleNotificationServiceClient GetAmazonSimpleNotificationServiceClient();
        AmazonSimpleWorkflowClient GetAmazonSimpleWorkflowClient();
        AmazonSQSClient GetAmazonSqsClient();
        AmazonStorageGatewayClient GetAmazonStorageGatewayClient();

    }
}
