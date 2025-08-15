using Genpact.Base;
using Genpact.Conf;
using Microsoft.Playwright;
using NLog;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Genpact.ApiHandlers;

public class GetPlaywrightSoftwareTextApi
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public async Task<(JsonObject, Boolean)> GetPlaywrightSoftwareTextAsync(IBrowserContext contex, string action ,string pageName,string prop,string formatVersion)
    {
        
        var url = $"{Config.MediaWikiBaseUrl}action={action}&page={pageName}&prop={prop}&formatversion={formatVersion}";

        APIRequestContextOptions customeApiRequestContextOptions = PlaywrightDriver.GetApiRequestContextOptions();

        _logger.Info($"About to send request: {url}");
        var response = await contex.APIRequest.GetAsync($"{url}", customeApiRequestContextOptions);
        var linesForDlrPageJsonResponse = await response.JsonAsync<JsonObject?>();
        if (linesForDlrPageJsonResponse == null || linesForDlrPageJsonResponse.Count == 0)
        {
            _logger.Info("The JSON response is Null or Empty");
            throw new InvalidOperationException();
        }
        else if (!response.Ok)
        {
            _logger.Info($"Response Status Code Is: {response.Status.ToString()}");
            Assert.Fail("Wrong response status code");
        }
        string responseStr = JsonSerializer.Serialize(response);
        _logger.Info($"Response body is: {responseStr}");
        return (linesForDlrPageJsonResponse, true);
    }
}