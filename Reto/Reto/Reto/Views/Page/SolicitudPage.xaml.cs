using Reto.ViewModels.Page;

namespace Reto.Views.Page;

public partial class SolicitudPage : ContentPage
{
	public SolicitudPage(SolicitudViewModel solicitudViewModel)
	{
		InitializeComponent();
		this.BindingContext = solicitudViewModel;
    }
}