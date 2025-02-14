using Reto.ViewModels.Page;

namespace Reto.Views.Page;

public partial class AddPiezaPage : ContentPage
{
	public AddPiezaPage(AddPiezaViewModel addPiezaView)
	{
		InitializeComponent();
		this.BindingContext = addPiezaView;
    }
}