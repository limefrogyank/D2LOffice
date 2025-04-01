using CommunityToolkit.Maui;
using D2LOffice.Service;
using MauiIcons.Fluent;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;
using Syncfusion.Blazor;

namespace D2LOffice;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mzc4NTY5NkAzMjM5MmUzMDJlMzAzYjMzMzMzYk9jS3A3U0phUnBqdkd5YTRzOUg3dVhWQVpCcmNGT1lpOW9LaG9SYnA1V2M9");

        var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseAppBootstrapper()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});
        builder.Services.AddFluentUIComponents();
        builder.Services.AddMauiBlazorWebView();

        builder.UseFluentMauiIcons();

        builder.Services.AddSyncfusionBlazor();

        builder.Services.AddMauiBlazorWebView();

        builder.Services.AddScoped<Navigation>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		var host = builder.Build();
        //Initialize(host);
        return host;
    }

    //private static void Initialize(MauiApp host)
    //{
    //    host.Services.GetService<Navigation>();
    //    // .. other initialization calls.
    //}
}
