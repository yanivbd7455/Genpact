using Genpact.Conf;
using Microsoft.Playwright;
using NLog;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Genpact.ApiHandlers;

public class GetPlaywrightSoftwareText
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public async Task<(JsonObject, Boolean)> GetPlaywrightSoftwareTextAsync(IBrowserContext contex, string action ,string pageName,string prop,string formatVersion)
    {
        
        var url = $"{Config.MediaWikiBaseUrl}action={action}&page={pageName}&prop={prop}&formatversion={formatVersion}";
        
        
    }
}