# ASP.NET Core 8 MVC 多國語系範例

加入多國語系資源大概有以下幾個步驟：

1. 設定 DI 與 Middleware ( Program.cs )

    服務註冊

    ```cs
    builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
    
    builder.Services.Configure<RequestLocalizationOptions>(options =>
    {
        var currentCulture = CultureInfo.CurrentCulture;
        var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("zh-TW"),
                new CultureInfo("en"),
            };
        options.DefaultRequestCulture = new RequestCulture(currentCulture);
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;
    
        // 在回應標頭中加入 Content-Language 標頭，告知用戶端此份 HTTP 內容的語言為何
        options.ApplyCurrentCultureToResponseHeaders = true;
    });
    ```

    設定 Middleware

    ```cs
    app.UseRequestLocalization();
    ```

1. 建立 Resources 資料夾，所有資源檔都放裡面，想要在地化的檔案，其放置路徑要跟原始檔案一樣

   例如：

   ```
   /Controllers/HomeController.cs
   ```

   其在地化的資源檔就位於：

   ```
   /Resources/Controllers/HomeController.*.resx
   ```

   如果是 Views 要在地化也是一樣，例如：

   ```
   /Views/Home/Privacy.cshtml
   ```

   其在地化的資源檔就位於：

   ```
   /Resources/Views/Home/Privacy*.resx
   ```
   
4. 建構式注入 `IStringLocalizer<T>`

    ```cs
    private readonly ILogger<HomeController> _logger;
    private readonly IStringLocalizer<HomeController> _localizer;
    
    public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> localizer)
    {
        _logger = logger;
        _localizer = localizer;
    }
    ```

5. 透過 `HttpContext.Features.Get<IRequestCultureFeature>()!.RequestCulture` 可以取得目前選中的 Culture 資訊
