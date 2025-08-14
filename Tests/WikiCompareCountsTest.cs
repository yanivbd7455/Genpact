using Allure.NUnit;
using Allure.NUnit.Attributes;
using Genpact.Base;
using Genpact.POM;
using NLog;

namespace E2E.Tests;

[TestFixture]
[AllureNUnit]
[AllureParentSuite("Wiki")]
[AllureSuite("Wiki UI And API")]
public class WikiCompareCountsTest : BaseTest
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    WikiPage WikiPage { get; set; }

    [SetUp]
    [AllureBefore("Setup session")]
    public void Setup()
    {
        logger.Info("Setup Start.");
        this.WikiPage = new WikiPage(this.Page);
    }

    [TearDown]
    [AllureAfter("Teardown session")]
    public async Task Teardown()
    {
        logger.Info("Teardown Start.");
        await this.CaptureScreenshotOnFailureAsync();
        await this.BrowserContext.CloseAsync();
        await this.Browser.CloseAsync();
        logger.Info("Teardown End.");
    }

    [Test]
    [AllureName("UI and API count equal")]
    [AllureDescription("Compare text count of UI and API")]
    [AllureOwner("Yaniv Ben-Dror")]
    [AllureTag("functionality")]
    [Category("functionality")]
    public async Task CompareTextCount()
    {
        logger.Info("CompareTextCount(): Test Start.");
        await this.WikiPage.NavigateToWikiPage();
        await this.Page.PauseAsync();
        logger.Info("CompareTextCount(): Test End.");
    }
}
