using Allure.NUnit;
using Allure.NUnit.Attributes;
using Genpact.Base;
using Genpact.Locators;
using Genpact.POM;
using Microsoft.Playwright;
using NLog;
using NUnit.Framework;
using static System.Net.Mime.MediaTypeNames;

namespace E2E.Tests;

[TestFixture]
[AllureNUnit]
[AllureParentSuite("Wiki")]
[AllureSuite("Wiki UI And API")]
public class WikiCompareCountsTest : BaseTest
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    private WikiPage WikiPage { get; set; }
    private WikiPageLocators WikiPageLoators { get; set; }
    private string RawText { get; set; }
    private string NormalizedText { get; set; }
    private HashSet<string> UIUniqueWords { get; set; }

    [SetUp]
    [AllureBefore("Setup session")]
    public void Setup()
    {
        logger.Info("Setup Start.");
        this.WikiPage = new WikiPage(this.Page);
        this.WikiPageLoators = new WikiPageLocators(this.Page);
        this.RawText = string.Empty;
        this.NormalizedText = string.Empty;
        this.UIUniqueWords = new HashSet<string>();
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
        await this.NavigateToWikiPageAsync();
        await this.NavigateToDebuggingFeaturesAsync();
        RawText = await this.GetDebuggingFeaturesTextAsync();
        NormalizedText = NormalizeRawTextAsync(RawText);
        UIUniqueWords = UICountUniqueWords(NormalizedText);
        //await this.Page.PauseAsync(); //TODO
        logger.Info("CompareTextCount(): Test End.");
    }

    [AllureStep("Navigate To Wiki Page")]
    protected async Task NavigateToWikiPageAsync()
    {
        logger.Info("NavigateToWikiPage(): Start.");
        await this.WikiPage.NavigateToWikiPage();
        await this.Expect(this.WikiPageLoators.PlaywrightSoftwareHeading).ToBeVisibleAsync();
        logger.Info("NavigateToWikiPage(): Done.");
    }

    [AllureStep("Navigate To Debugging Features")]
    protected async Task NavigateToDebuggingFeaturesAsync()
    {
        logger.Info("NavigateToDebuggingFeatures(): Start.");
        await this.Expect(this.WikiPageLoators.DebuggingFeaturesLink).ToBeEnabledAsync();
        await this.WikiPageLoators.DebuggingFeaturesLink.ClickAsync();
        await this.Expect(this.WikiPageLoators.DebuggingFeaturesHeading).ToBeVisibleAsync();
        logger.Info("NavigateToDebuggingFeatures(): Done.");
    }


    [AllureStep("Get 'Debugging Features' Text")]
    protected async Task<string> GetDebuggingFeaturesTextAsync()
    {
        logger.Info("GetDebuggingFeaturesText(): Start.");
        var DebuggingFeaturesHeadingtxt = await this.WikiPageLoators.DebuggingFeaturesHeading.InnerTextAsync();
        var DebuggingFeaturesSubHeadingtxt = await this.WikiPageLoators.DebuggingFeaturesSubHeading.InnerTextAsync();
        var listItems = await this.WikiPageLoators.DebuggingFeaturesBuiltInArray.AllInnerTextsAsync();
        var DebuggingFeaturesBuiltInArraytxt = string.Join(" ", listItems);

        var combinedText = $"{DebuggingFeaturesHeadingtxt} {DebuggingFeaturesSubHeadingtxt} {DebuggingFeaturesBuiltInArraytxt}";
        logger.Info($"Raw Combined Text: {combinedText} ");
        logger.Info("GetDebuggingFeaturesText(): Done.");
        return combinedText;
    }


    [AllureStep("Normalize Raw Text")]
    protected static string NormalizeRawTextAsync(string text)
    {
        logger.Info("NormalizeTextAsync(): Start.");
        // Convert to lowercase.
        var normalized = text.ToLowerInvariant();

        // Remove punctuation and numbers using a Regex.
        normalized = Regex.Replace(normalized, @"[^\p{L}\s]", "");

        // Collapse multiple spaces into a single space.
        normalized = Regex.Replace(normalized, @"\s+", " ").Trim();
        logger.Info($"Normalized Text: {normalized} ");
        logger.Info("NormalizeTextAsync(): Done.");
        return normalized;
    }

    [AllureStep("Count Unique Words")]
    protected static HashSet<string> UICountUniqueWords(string normalizedText)
    {
        var words = normalizedText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return new HashSet<string>(words);
    }
}
