using Genpact.Conf;
using Microsoft.Playwright;
using NLog;


namespace Genpact.Base;

public class PlaywrightDriver
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    private static IPlaywright? playwrightInstance = null;
    private static readonly object _lock = new object();


    public async static Task<(IBrowser browser, IPage page, IBrowserContext browserContext, IPlaywright playwrightInstance)> InitializePlaywrightAsync()
    {
        playwrightInstance = ManagePlaywrightSingletone();    
        var browserInstance = await InitBrowserAsync(playwrightInstance);
        var browserContext = await InitBrowserContextNoTokenAsync(browserInstance);
        var page = await browserContext.NewPageAsync();
        var apiRequestContext = browserContext.APIRequest;
        return (browserInstance, page, browserContext, playwrightInstance);
    }

    
    public static IPlaywright ManagePlaywrightSingletone()
    {
        lock (_lock)
        {
            playwrightInstance = playwrightInstance ?? Playwright.CreateAsync().Result;
        }
        return playwrightInstance;
    }

    public async static Task<IBrowser> InitBrowserAsync(IPlaywright playwright)
    {
        string browserType = "Chromium";
        BrowserTypeLaunchOptions launchOptions = new BrowserTypeLaunchOptions { Headless = false, SlowMo = 50 };
        return browserType switch
        {
            "Chromium" => await playwright.Chromium.LaunchAsync(launchOptions),
            "Chrome" => await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                ExecutablePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe",
                Headless = false
            }),
            "Firefox" => await playwright.Firefox.LaunchAsync(launchOptions),
            "Webkit" => await playwright.Webkit.LaunchAsync(launchOptions),
            _ => await playwright.Chromium.LaunchAsync(launchOptions)
        };
    }

    public async static Task<IBrowserContext> InitBrowserContextNoTokenAsync(IBrowser browser)
    {
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            BaseURL = Config.WikiWebBaseUrl,
            IgnoreHTTPSErrors = true,
            ExtraHTTPHeaders = new Dictionary<string, string>
            {
                {"Content-Type","application/x-www-form-urlencoded; charset=utf-8"},
            }
        });
        #region BrowserNewContextOptions
        /*          AcceptDownloads = clone.AcceptDownloads;
                    BaseURL = clone.BaseURL;
                    BypassCSP = clone.BypassCSP;
                    ColorScheme = clone.ColorScheme;
                    DeviceScaleFactor = clone.DeviceScaleFactor;
                    ExtraHTTPHeaders = clone.ExtraHTTPHeaders;
                    ForcedColors = clone.ForcedColors;
                    Geolocation = clone.Geolocation;
                    HasTouch = clone.HasTouch;
                    HttpCredentials = clone.HttpCredentials;
                    IgnoreHTTPSErrors = clone.IgnoreHTTPSErrors;
                    IsMobile = clone.IsMobile;
                    JavaScriptEnabled = clone.JavaScriptEnabled;
                    Locale = clone.Locale;
                    Offline = clone.Offline;
                    Permissions = clone.Permissions;
                    Proxy = clone.Proxy;
                    RecordHarContent = clone.RecordHarContent;
                    RecordHarMode = clone.RecordHarMode;
                    RecordHarOmitContent = clone.RecordHarOmitContent;
                    RecordHarPath = clone.RecordHarPath;
                    RecordHarUrlFilter = clone.RecordHarUrlFilter;
                    RecordHarUrlFilterRegex = clone.RecordHarUrlFilterRegex;
                    RecordHarUrlFilterString = clone.RecordHarUrlFilterString;
                    RecordVideoDir = clone.RecordVideoDir;
                    RecordVideoSize = clone.RecordVideoSize;
                    ReducedMotion = clone.ReducedMotion;
                    ScreenSize = clone.ScreenSize;
                    ServiceWorkers = clone.ServiceWorkers;
                    StorageState = clone.StorageState;
                    StorageStatePath = clone.StorageStatePath;
                    StrictSelectors = clone.StrictSelectors;
                    TimezoneId = clone.TimezoneId;
                    UserAgent = clone.UserAgent;
                    ViewportSize = clone.ViewportSize;*/
        #endregion
        return context;
    }

    public static APIRequestContextOptions GetApiRequestContextOptions()
    {
        var headers = new Dictionary<string, string>();
        //headers.Add("Authorization", $"Bearer {token}");
        headers.Add("Content-Type", "application/x-www-form-urlencoded; charset=utf-8");

        var apiRequestContextOptions = new APIRequestContextOptions
        {
            Headers = headers,
        };
        #region APIRequestContextOptions
        /*Data = clone.Data;
        DataByte = clone.DataByte;
        DataObject = clone.DataObject;
        DataString = clone.DataString;
        FailOnStatusCode = clone.FailOnStatusCode;
        Form = clone.Form;
        Headers = clone.Headers;
        IgnoreHTTPSErrors = clone.IgnoreHTTPSErrors;
        MaxRedirects = clone.MaxRedirects;
        Method = clone.Method;
        Multipart = clone.Multipart;
        Params = clone.Params;
        Timeout = clone.Timeout;*/
        #endregion
        return apiRequestContextOptions;
    }
}