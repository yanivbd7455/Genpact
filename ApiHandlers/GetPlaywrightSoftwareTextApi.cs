using Genpact.Base;
using Genpact.Conf;
using Microsoft.Playwright;
using NLog;
using System.Text.Json.Nodes;

namespace Genpact.ApiHandlers;

public class GetPlaywrightSoftwareTextApi
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public async Task<(string, Boolean)> GetPlaywrightSoftwareTextAsync(IBrowserContext contex, string action ,string pageName,string prop,string format)
    {
        
        var url = $"{Config.MediaWikiBaseUrl}action={action}&page={pageName}&prop={prop}&format={format}";

        APIRequestContextOptions customeApiRequestContextOptions = PlaywrightDriver.GetApiRequestContextOptions();

        _logger.Info($"About to send request: {url}");
        var response = await contex.APIRequest.GetAsync($"{url}", customeApiRequestContextOptions);
        var playwrightSoftwareJsonResponse = await response.JsonAsync<JsonObject?>();
        if (playwrightSoftwareJsonResponse == null || playwrightSoftwareJsonResponse.Count == 0)
        {
            _logger.Info("The JSON response is Null or Empty");
            throw new InvalidOperationException();
        }
        else if (!response.Ok)
        {
            _logger.Info($"Response Status Code Is: {response.Status.ToString()}");
            Assert.Fail("Wrong response status code");
        }
        string responseStr = playwrightSoftwareJsonResponse.ToJsonString();
        return (responseStr, true);
    }
}