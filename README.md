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

    請安排在在 Endpoint Middleware 之前，也就是所有 `app.Map*()` 的這堆之前

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

   若有模型類別，裡面的驗證屬性如果有有訊息字串，也是相同的設定方式。
   
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

5. 透過 View (`*.cshtml`) 注入

    ```cs
    @inject IViewLocalizer Localizer
    ```

    建議也可以在 `/Views/_ViewImports.cshtml` 加入：


    ```cs
    @using Microsoft.AspNetCore.Mvc.Localization
    ```

5. 透過 `HttpContext.Features.Get<IRequestCultureFeature>()!.RequestCulture` 可以取得目前選中的 Culture 資訊

## 相關連結

- [ASP.NET Core 全球化和當地語系化](https://learn.microsoft.com/zh-tw/aspnet/core/fundamentals/localization?view=aspnetcore-8.0&WT.mc_id=DT-MVP-4015686) (老實說，這部分的官方文件寫的真是爛)
  - [讓 ASP.NET Core 應用程式的內容可當地語系化](https://learn.microsoft.com/zh-tw/aspnet/core/fundamentals/localization/make-content-localizable?view=aspnetcore-8.0&WT.mc_id=DT-MVP-4015686)
  - [在 ASP.NET Core 應用程式中提供語言和文化特性的當地語系化資源](https://learn.microsoft.com/zh-tw/aspnet/core/fundamentals/localization/provide-resources?view=aspnetcore-8.0&WT.mc_id=DT-MVP-4015686)
  - [在當地語系化的 ASP.NET Core 應用程式中實作可依據每項要求選取語言/文化特性的策略](https://learn.microsoft.com/zh-tw/aspnet/core/fundamentals/localization/select-language-culture?view=aspnetcore-8.0&WT.mc_id=DT-MVP-4015686)
  - [使用 ASP.NET Core 設定可攜式物件當地語系化](https://learn.microsoft.com/zh-tw/aspnet/core/fundamentals/portable-object-localization?view=aspnetcore-8.0&WT.mc_id=DT-MVP-4015686)
  - [當地語系化擴充性](https://learn.microsoft.com/zh-tw/aspnet/core/fundamentals/localization-extensibility?view=aspnetcore-8.0&WT.mc_id=DT-MVP-4015686)
  - [針對 ASP.NET Core 當地語系化進行疑難排解](https://learn.microsoft.com/zh-tw/aspnet/core/fundamentals/troubleshoot-aspnet-core-localization?view=aspnetcore-8.0&WT.mc_id=DT-MVP-4015686)
 
## 範例程式

- [Localization](https://github.com/dotnet/aspnetcore/tree/release/8.0/src/Middleware/Localization)

   這裡有許多相當不錯的進階範例！👍

