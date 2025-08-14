
using Microsoft.Playwright;
using NLog;

namespace Genpact.Conf;

public class BasePage
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public BasePage(IPage page)
    {
        this.Page = page;
        this.BaseUrl = Config.WikiWebBaseUrl;
    }

    public IPage Page { get; }
    public string? BaseUrl { get; }
}