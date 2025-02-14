using Reto.Models;
using Reto.ViewModels.Page;

namespace Reto.Views.Page;

public partial class AddRegistroPage : ContentPage
{
	private AddRegistroViewModel addRegistroViewModel = null;
    public AddRegistroPage(AddRegistroViewModel addRegistroView)
	{
		InitializeComponent();
		this.BindingContext = addRegistroViewModel = addRegistroView;
	}

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
		try
		{
			var pick = ((Picker)sender);
			var item = pick.SelectedItem as PiezasModel;
			if(item != null)
			{
				addRegistroViewModel.Pieza = item;
				addRegistroViewModel.MaximusPiezas = item.Cantidad;
				if (item.Cantidad <= 0)
					addRegistroViewModel.ErrorPiezas = "No hay inventario";
				else
					addRegistroViewModel.ErrorPiezas = string.Empty;
            }
		}
		catch(Exception ex)
		{

		}
    }
}