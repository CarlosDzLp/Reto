using Reto.Models;
using Reto.ViewModels.Page;

namespace Reto.Views.Page;

public partial class AddSolicitudPage : ContentPage
{
	AddSolicitudViewModel addSolicitudViewModel;
    public AddSolicitudPage(AddSolicitudViewModel addSolicitudViewModel)
	{
		InitializeComponent();
		this.BindingContext = this.addSolicitudViewModel = addSolicitudViewModel;
    }

    private async void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            var pick = ((Picker)sender).SelectedItem as PiezasModel;
            if(pick != null)
            {
               await addSolicitudViewModel.LoadPiezaItem(pick);
            }
        }
        catch(Exception ex)
        {

        }
    }
}