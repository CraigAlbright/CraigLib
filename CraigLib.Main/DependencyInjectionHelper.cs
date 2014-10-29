using Autofac;
using CraigLib.Cloud.AWS;
using CraigLib.Cloud.Azure;
using CraigLib.File;
using CraigLib.Sound;
using CraigLib.TimeAndDate;

namespace CraigLib
{
    public class DependencyInjectionHelper
    {
        private static IContainer Container { get; set; }
        public DependencyInjectionHelper()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<AWSHelper>().As<IAWSHelper>();
            builder.RegisterType<AzureHelper>().As<IAzureHelper>();
            builder.RegisterType<FileHelper>().As<IFileHelper>();
            builder.RegisterType<TimeAndDateHelper>().As<ITimeAndDateHelper>();
            builder.RegisterType<LibsndfileApi>().As<ILibsndfileApi>();
            Container = builder.Build();

        }
    }
}
