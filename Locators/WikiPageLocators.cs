using Microsoft.Playwright;
using Genpact.Base;

namespace Genpact.Locators;

public class WikiPageLocators
{
    private readonly IPage _page;

    public WikiPageLocators(IPage page)
    {
        _page = page;
    }

    public ILocator DebuggingFeatures => _page.Locator("//input[@id='username']");
}