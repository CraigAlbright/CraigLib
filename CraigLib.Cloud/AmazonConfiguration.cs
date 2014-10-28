using System.Configuration;

namespace CraigLib.Cloud
{
    public class AmazonConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("AWSProfileName", DefaultValue = "development", IsRequired = true)]
        [StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\", MinLength = 1, MaxLength = 60)]
        public string AwsProfileName
        {
            get
            {
                return (string) this["AWSProfileName"];
            }
            set
            {
                this["name"] = value;
            }
        }
    }

   
}
