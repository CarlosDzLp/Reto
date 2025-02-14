using Reto.Models;
using Reto.ViewModels;

namespace Reto.Views;

public partial class PrincipalPage : ContentPage
{
    private readonly PrincipalViewModel tallerView;

    public PrincipalPage(PrincipalViewModel tallerView)
    {
        InitializeComponent();
        this.BindingContext = this.tallerView = tallerView;
    }

    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            if(e.CurrentSelection != null && e.CurrentSelection.Count > 0)
            {
                var taller = e.CurrentSelection.FirstOrDefault() as TallerModel;
                if (taller != null)
                {
                    await tallerView.NavigateToDetail(taller);
                }
                ((CollectionView)sender).SelectedItem = null;
            }
        }
        catch(Exception ex)
        {

        }
    }
}