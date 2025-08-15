namespace Genpact.Conf
{
    public static class Config
    {
        public static readonly string? WikiWebBaseUrl = ConfigManager.GetPropertyValue("wikiWebBaseUrl");
        public static readonly string? MediaWikiBaseUrl = ConfigManager.GetPropertyValue("mediaWikiBaseUrl");
    }
}