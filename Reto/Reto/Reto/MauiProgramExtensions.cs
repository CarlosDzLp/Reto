using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Reto.Db;
using Reto.Db.Repository;
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
        mauiAppBuilder.Services.AddTransient<AddRegistroPage>();
        mauiAppBuilder.Services.AddTransient<AddSolicitudPage>();
        mauiAppBuilder.Services.AddTransient<DetalleSolicitudEnviaPage>();
        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<PrincipalViewModel>();
        mauiAppBuilder.Services.AddTransient<TallerViewModel>();
        mauiAppBuilder.Services.AddTransient<RegistroViewModel>();
        mauiAppBuilder.Services.AddTransient<SolicitudViewModel>();
        mauiAppBuilder.Services.AddTransient<AddPiezaViewModel>();
        mauiAppBuilder.Services.AddTransient<AddRegistroViewModel>();
        mauiAppBuilder.Services.AddTransient<AddSolicitudViewModel>();
        mauiAppBuilder.Services.AddTransient<DetalleSolicitudEnviaViewModel>();
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
