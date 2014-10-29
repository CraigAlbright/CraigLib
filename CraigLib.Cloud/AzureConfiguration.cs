using System.Configuration;

namespace CraigLib.Cloud
{
    public class AzureConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("AzureProfileName", DefaultValue = "development", IsRequired = true)]
        [StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\", MinLength = 1, MaxLength = 60)]
        public string AzureProfileName
        {
            get
            {
                return (string)this["AzureProfileName"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("AzureToken", DefaultValue = "", IsRequired = true)]
        [StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\", MinLength = 1, MaxLength = 60)]
        public string AzureToken
        {
            get
            {
                return (string)this["AzureToken"];
            }
            set
            {
                this["name"] = value;
            }
        }
    }
}
