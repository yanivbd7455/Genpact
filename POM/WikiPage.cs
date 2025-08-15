using Genpact.Base;
using Genpact.Locators;
using Microsoft.Playwright;

namespace Genpact.POM;

public class WikiPage : BasePage
{
    public WikiPageLocators WikiPageLocators { get; }
    public WikiPage(IPage page) : base(page)
    {
        WikiPageLocators = new WikiPageLocators(page);
    }

    public async Task NavigateToWikiPage()
    {
        if (string.IsNullOrEmpty(this.BaseUrl))
        {
            throw new ArgumentNullException(nameof(this.BaseUrl), "Base URL is not set.");
        }
        await this.Page.GotoAsync(this.BaseUrl, new PageGotoOptions { WaitUntil = WaitUntilState.Load});
        await this.Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        await this.Page.WaitForLoadStateAsync(LoadState.Load);
        await this.Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
    }

/*    public async Task NavigateDebuggingFeaturesSection()
    {
        await this.WikiPageLocators.
    }*/
}