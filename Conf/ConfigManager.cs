using Microsoft.Extensions.Configuration;

namespace Genpact.Conf
{
    public static class ConfigManager
    {
        private static readonly string _settingPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(),@"Conf/appsettings.json"));
        private static readonly IConfiguration _config = new ConfigurationBuilder().AddJsonFile(_settingPath).Build();

        public static string? GetPropertyValue(string key)
        {
            return _config[key];
        }
    }
}