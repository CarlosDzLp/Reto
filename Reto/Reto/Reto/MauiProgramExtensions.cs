using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Reto.Db;
using Reto.Db.Repository;
using Reto.Helpers;
using Reto.Service;
using Reto.ViewModels;
using Reto.ViewModels.Page;
using Reto.Views;
using Reto.Views.Page;

namespace Reto;

public static class MauiProgramExtensions
{
	public static MauiAppBuilder UseSharedMauiApp(this MauiAppBuilder builder)
	{
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .RegisterViewModels()
            .RegisterViews()
            .RegisterService()
            
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("RobotoBlack.ttf", "RobotoBlack");
				fonts.AddFont("RobotoBold.ttf", "RobotoBold");
                fonts.AddFont("RobotoMedium.ttf", "RobotoMedium");
                fonts.AddFont("RobotoRegular.ttf", "RobotoRegular");

                fonts.AddFont("FontAwesomeProLight.otf", "FontAwesomeProLight");
                fonts.AddFont("FontAwesomeProRegular.otf", "FontAwesomeProRegular");
                fonts.AddFont("FontAwesomeProSolid.otf", "FontAwesomeProSolid");
                fonts.AddFont("FontAwesomeProThin.otf", "FontAwesomeProThin");
            });
#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder;
	}
    

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<NavigationPage>();
        mauiAppBuilder.Services.AddTransient<PrincipalPage>();
        mauiAppBuilder.Services.AddTransient<TallerPage>();
        mauiAppBuilder.Services.AddTransient<RegistroPage>();
        mauiAppBuilder.Services.AddTransient<SolicitudPage>();
        mauiAppBuilder.Services.AddTransient<AddPiezaPage>();
        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<PrincipalViewModel>();
        mauiAppBuilder.Services.AddSingleton<TallerViewModel>();
        mauiAppBuilder.Services.AddSingleton<RegistroViewModel>();
        mauiAppBuilder.Services.AddSingleton<SolicitudViewModel>();
        mauiAppBuilder.Services.AddSingleton<AddPiezaViewModel>();
        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterService(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<ApplicationContext>();
        mauiAppBuilder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        mauiAppBuilder.Services.AddTransient<INavigationService, NavigationService>();
        return mauiAppBuilder;
    }
}
