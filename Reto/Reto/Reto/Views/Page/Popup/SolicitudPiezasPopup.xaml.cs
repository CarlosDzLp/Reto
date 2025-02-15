using CommunityToolkit.Maui.Views;
using Reto.Models;
using System.Collections.ObjectModel;

namespace Reto.Views.Page.Popup;

public partial class SolicitudPiezasPopup : CommunityToolkit.Maui.Views.Popup
{
    public event EventHandler<SolicitudPiezaModel> SelectionChanged;
    public ObservableCollection<SolicitudPiezaModel> SolicitudPiezas { get; set; }
    public SolicitudPiezasPopup(List<SolicitudPiezaModel> solicitudPiezas)
	{
		InitializeComponent();
		this.BindingContext = this;
        SolicitudPiezas = new ObservableCollection<SolicitudPiezaModel>(solicitudPiezas);
    }

    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            if(e.CurrentSelection.Count > 0)
            {
                var item = e.CurrentSelection.FirstOrDefault() as SolicitudPiezaModel;
                if(item != null)
                {
                    SelectionChanged?.Invoke(this, item);
                    CloseAsync();
                }
                ((CollectionView)sender).SelectedItem = null;
            }
        }
        catch(Exception ex)
        {

        }
    }

    private void Popup_Closed(object sender, CommunityToolkit.Maui.Core.PopupClosedEventArgs e)
    {
        SelectionChanged?.Invoke(this, null);
    }
}