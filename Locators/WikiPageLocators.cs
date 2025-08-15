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

    public ILocator PlaywrightSoftwareHeading => _page.GetByRole(AriaRole.Heading, new() { Name = "Playwright (software)" }).Locator("span");
    public ILocator DebuggingFeaturesLink => _page.GetByRole(AriaRole.Link, new() { Name = "Debugging features" });
    public ILocator DebuggingFeaturesHeading => _page.GetByRole(AriaRole.Heading, new() { Name = "Debugging features" });
    public ILocator DebuggingFeaturesSubHeading => _page.GetByText("Playwright includes built-in");
    public ILocator DebuggingFeaturesBuiltInArray => _page.Locator("//*[@id='mw-content-text']/div[1]/ul[2]");
    public ILocator DebuggingFeaturesBuiltInArrayCss => _page.Locator("#mw-content-text > div.mw-content-ltr.mw-parser-output > ul:nth-child(27)");

}