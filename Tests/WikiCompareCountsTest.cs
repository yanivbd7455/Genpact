using Allure.NUnit;
using Allure.NUnit.Attributes;
using Genpact.ApiHandlers;
using Genpact.Base;
using Genpact.Locators;
using Genpact.POM;
using HtmlAgilityPack;
using NLog;
using System.Net;

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
    private string UIRawText { get; set; }
    private string UINormalizedText { get; set; }
    private GetPlaywrightSoftwareTextApi GetPlaywrightSoftwareTextApi { get; set; }
    private HashSet<string> UIUniqueWords { get; set; }
    private string APIRawText { get; set; }
    private string APINormalizedText { get; set; }
    private HashSet<string> APIUniqueWords { get; set; }


    [SetUp]
    [AllureBefore("Setup session")]
    public void Setup()
    {
        logger.Info("Setup Start.");
        this.WikiPage = new WikiPage(this.Page);
        this.WikiPageLoators = new WikiPageLocators(this.Page);
        this.UIRawText = string.Empty;
        this.UINormalizedText = string.Empty;
        this.GetPlaywrightSoftwareTextApi = new GetPlaywrightSoftwareTextApi();
        this.UIUniqueWords = new HashSet<string>();
        this.APIRawText = string.Empty;
        this.APINormalizedText = string.Empty;
        this.APIUniqueWords = new HashSet<string>();
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
        UIRawText = await this.GetDebuggingFeaturesTextUIAsync();
        UINormalizedText = NormalizeRawTextAsync(UIRawText);
        UIUniqueWords = UICountUniqueWords(UINormalizedText);
        APIRawText = await this.GetDebuggingFeaturesTextAPIAsync();
        string debuggingSectionRaw = this.ExtractDebuggingSection(APIRawText);
        APINormalizedText = NormalizeRawHtmlTextAsync(debuggingSectionRaw);
        APIUniqueWords = UICountUniqueWords(APINormalizedText);
        bool areEqual = AreHashSetsEqual(APIUniqueWords, UIUniqueWords);
        Assert.That(areEqual, Is.True, "The unique words count from UI and API do not match.");
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


    [AllureStep("Get 'Debugging Features' Text - UI")]
    protected async Task<string> GetDebuggingFeaturesTextUIAsync()
    {
        logger.Info("GetDebuggingFeaturesTextUIAsync(): Start.");
        var DebuggingFeaturesHeadingtxt = await this.WikiPageLoators.DebuggingFeaturesHeading.InnerTextAsync();
        var DebuggingFeaturesSubHeadingtxt = await this.WikiPageLoators.DebuggingFeaturesSubHeading.InnerTextAsync();
        var listItems = await this.WikiPageLoators.DebuggingFeaturesBuiltInArray.AllInnerTextsAsync();
        var DebuggingFeaturesBuiltInArraytxt = string.Join(" ", listItems);

        var combinedText = $"{DebuggingFeaturesHeadingtxt} {DebuggingFeaturesSubHeadingtxt} {DebuggingFeaturesBuiltInArraytxt}";
        logger.Info($"Raw Combined Text: {combinedText} ");
        logger.Info("GetDebuggingFeaturesTextUIAsync(): Done.");
        return combinedText;
    }


    [AllureStep("Normalize Raw Text")]
    protected static string NormalizeRawTextAsync(string text)
    {
        logger.Info("NormalizeTextAsync(): Start.");
        var normalized = text.ToLowerInvariant();
        normalized = Regex.Replace(normalized, @"[^\p{L}\s]", "");
        normalized = Regex.Replace(normalized, @"\s+", " ").Trim();
        logger.Info($"Normalized Text: {normalized} ");
        logger.Info("NormalizeTextAsync(): Done.");
        return normalized;
    }

    [AllureStep("Count Unique Words - UI")]
    protected static HashSet<string> UICountUniqueWords(string normalizedText)
    {
        logger.Info("UICountUniqueWords(): Start.");
        var words = normalizedText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        logger.Info("UICountUniqueWords(): Start.");
        return new HashSet<string>(words);
    }

    [AllureStep("Get 'Debugging Features' Text - API")]
    protected async Task<string> GetDebuggingFeaturesTextAPIAsync()
    {
        logger.Info("GetDebuggingFeaturesTextAPIAsync(): Start.");
        string action = "parse";
        string pageName = "Playwright_(software)";
        string prop = "text";
        string format = "json";

        var (result, status_code) = await this.GetPlaywrightSoftwareTextApi.GetPlaywrightSoftwareTextAsync(this.BrowserContext, action, pageName, prop, format);
        Assert.That(status_code,Is.True, "API call failed or returned an unexpected status code.");
        logger.Info($"Raw API Result Text: {result} ");
        logger.Info("GetDebuggingFeaturesTextAPIAsync(): Done.");
        return result;
    }

    [AllureStep("Extract Debugging Section")]
    protected string ExtractDebuggingSection(string allRawText)
    {
        logger.Info("ExtractDebuggingSection(): Start.");
        string extracted = string.Empty;
        string startText = "Debugging_features";
        string endText = "monitoring";

        int startIndex = allRawText.IndexOf(startText);
        if (startIndex >= 0)
        {
            int endIndex = allRawText.IndexOf(endText, startIndex);
            if (endIndex > startIndex)
            {
                endIndex += endText.Length;
                extracted = allRawText.Substring(startIndex, endIndex - startIndex);
                logger.Info($"Extracted Text: {extracted} ");
            }
            else
            {
                logger.Error($"End text: {endText}, not found after start text.");
            }
        }
        else
        {
            logger.Error($"Start text: {startText}, not found.");
        }

        logger.Info("ExtractDebuggingSection(): Done.");
        return extracted;
    }


    [AllureStep("Normalize Raw HTML Text")]
    protected static string NormalizeRawHtmlTextAsync(string text)
    {
        logger.Info("NormalizeRawHtmlTextAsync(): Start.");
        string normalized = string.Empty;

        if (string.IsNullOrEmpty(text))
            return string.Empty;
        text = Regex.Replace(text, @"^Debugging_features", "", RegexOptions.IgnoreCase);
        string unicodeDecoded = Regex.Unescape(text);
        string htmlDecoded = WebUtility.HtmlDecode(unicodeDecoded);
        string noHtml = Regex.Replace(htmlDecoded, "<.*?>", " ");
        string lower = noHtml.ToLowerInvariant();
        string noPunct = Regex.Replace(lower, @"[^\w\s]", "");
        normalized = Regex.Replace(noPunct, @"\s+", " ").Trim();
        normalized = normalized.Replace("edit ","");
        logger.Info("NormalizeRawHtmlTextAsync(): Done.");
        return normalized;
    }


    [AllureStep("Compare two unique words structure")]
    public static bool AreHashSetsEqual(HashSet<string> apiWords, HashSet<string> uiWords)
    {
        return apiWords.SetEquals(uiWords);
    }
}
