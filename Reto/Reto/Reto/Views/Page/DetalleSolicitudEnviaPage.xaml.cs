using Reto.ViewModels.Page;

namespace Reto.Views.Page;

public partial class DetalleSolicitudEnviaPage : ContentPage
{
	DetalleSolicitudEnviaViewModel detalleSolicitudEnvia;
    public DetalleSolicitudEnviaPage(DetalleSolicitudEnviaViewModel detalleSolicitudEnviaViewModel)
	{
		InitializeComponent();
		this.BindingContext = this.detalleSolicitudEnvia = detalleSolicitudEnviaViewModel;
    }
}