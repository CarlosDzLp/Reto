using Reto.ViewModels.Page;

namespace Reto.Views.Page;

public partial class RegistroPage : ContentPage
{
	public RegistroPage(RegistroViewModel registroViewModel)
	{
		InitializeComponent();
		this.BindingContext = registroViewModel;
	}
}