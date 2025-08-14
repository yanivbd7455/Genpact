using Microsoft.Playwright;
using NLog;
using NUnit.Framework.Interfaces;
using Genpact.Conf;


namespace Genpact.Base;

public class BaseTest
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    
    public BaseTest()
    {
        LogManager.Setup().LoadConfigurationFromFile("nlog.config");
        this.ValidateTestingDetails();
    }

    #region - class properties
    public IPlaywright PlaywrightInstance { get; set; }
    public IPage Page { get; set; }
    public IBrowser Browser { get; set; }
    public IBrowserContext BrowserContext { get; set; }
    public ILocatorAssertions Expect(ILocator locator) => Assertions.Expect(locator);
    #endregion



    protected void ValidateTestingDetails()
    {
        if (string.IsNullOrEmpty(Config.WikiWebBaseUrl))
        {
            logger.Info("BaseTest: Some of UAT details are null or empty!");
        }
    }


    [SetUp]
    public async Task BaseTestSetup()
    {
        var (browser, page, browserContext, playwrightInstance) = await PlaywrightDriver.InitializePlaywrightAsync();
        this.PlaywrightInstance = playwrightInstance;
        this.BrowserContext = browserContext;
        this.Browser = browser;
        this.Page = page;
        if (this.Page is null)
        {
            logger.Info("BaseTestSetup(): Parameter 'Page' seems to be null");
            throw new ArgumentNullException(nameof(this.Page));
        }
    }


    protected async Task CaptureScreenshotOnFailureAsync()
    {
        var imagesFolder = Path.Combine(TestContext.CurrentContext.WorkDirectory, "TestsFailureImages");
        if(!Directory.Exists(imagesFolder))
        {
            Directory.CreateDirectory(imagesFolder);
        }
        var filePath = Path.GetFullPath(Path.Combine(imagesFolder, $"test_failure_{TestContext.CurrentContext.Test.FullName}.png"));
        if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
        {   
            if (this.Page != null)
            {
                logger.Info("BaseTest: About to take screenshot due to failure at: {TestName}.", TestContext.CurrentContext.Test.FullName);
                await this.Page.ScreenshotAsync(new PageScreenshotOptions { Path = filePath ,FullPage = true});
            }
        }
    }
    
    protected async Task WaitAndExpectLocatorForActionAsync(ILocator locator)
    {
        await locator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Attached});
        await locator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible});
        await this.Expect(locator).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 20000});
        await this.Expect(locator).ToBeEnabledAsync(new LocatorAssertionsToBeEnabledOptions { Timeout = 20000});
        await locator.HoverAsync();
        await locator.FocusAsync();
    }
}