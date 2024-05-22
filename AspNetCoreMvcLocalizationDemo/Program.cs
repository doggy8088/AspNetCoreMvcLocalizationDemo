using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var currentCulture = CultureInfo.CurrentCulture;
    var supportedCultures = new List<CultureInfo>
        {
            new CultureInfo("zh-TW"),
            new CultureInfo("en"),
            new CultureInfo("ja"), // Added Japanese as a supported culture
        };
    options.DefaultRequestCulture = new RequestCulture(currentCulture);
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    // �b�^�����Y���[�J Content-Language ���Y�A�i���Τ�ݦ��� HTTP ���e���y������
    options.ApplyCurrentCultureToResponseHeaders = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseRequestLocalization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
