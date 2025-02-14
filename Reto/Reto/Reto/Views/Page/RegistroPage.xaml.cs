using Reto.ViewModels.Page;

namespace Reto.Views.Page;

public partial class RegistroPage : ContentPage
{
    private RegistroViewModel registroViewModel;
    public RegistroPage(RegistroViewModel registroViewModel)
	{
		InitializeComponent();
		this.BindingContext = this.registroViewModel = registroViewModel;
	}
}