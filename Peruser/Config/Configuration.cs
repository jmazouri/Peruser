using Newtonsoft.Json;

namespace Peruser
{
    public class Configuration
    {
        public string[] AllowedFileTypes { get; set; }
        public string DefaultSort { get; set; }

        public static string Serialize(Configuration thisConfiguration)
        {
            return JsonConvert.SerializeObject(thisConfiguration);
        }

        public static Configuration Deserialize(string thisConfiguration)
        {
            return JsonConvert.DeserializeObject<Configuration>(thisConfiguration);
        }
    }
}
