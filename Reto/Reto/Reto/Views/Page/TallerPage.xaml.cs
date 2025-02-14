using Reto.ViewModels.Page;

namespace Reto.Views.Page;

public partial class TallerPage : ContentPage
{
	public TallerPage(TallerViewModel tallerView)
	{
		InitializeComponent();
		this.BindingContext = tallerView;
    }
}