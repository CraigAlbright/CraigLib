using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using CraigLib.Cloud.AWS;

namespace CraigLib
{
    public class DependencyInjectionHelper
    {
        private static IContainer Container { get; set; }
        public DependencyInjectionHelper()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<AWSHelper>().As<IAWSHelper>();
            Container = builder.Build();

        }
    }
}
