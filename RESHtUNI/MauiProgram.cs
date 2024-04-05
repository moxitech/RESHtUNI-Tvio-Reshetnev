using Microsoft.Extensions.Logging;
using RESHtUNI.Data;
using RESHtUNI.Services;

namespace RESHtUNI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });
            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSingleton<IOnlineCheckService, OnlineCheckService>();
            builder.Services.AddSingleton<ISearchGroupService, SearchGroupService>();
            builder.Services.AddSingleton<ITimeTableService, TimetableService>();
            builder.Services.AddSingleton<JsonRepository>();
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
