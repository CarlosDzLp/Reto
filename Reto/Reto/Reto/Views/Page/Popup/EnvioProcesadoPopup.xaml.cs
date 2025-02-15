using CommunityToolkit.Maui.Alerts;
using Reto.Db.Entities;
using Reto.Helpers;
using Reto.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Reto.Views.Page.Popup;

public partial class EnvioProcesadoPopup : ContentPage
{
    public event EventHandler<EnvioProcesadoModel> SelectionChanged;
    public string Solicitud { get; set; }
    public string Mecanico { get; set; }
    public string TallerEnvia { get; set; }
    public ObservableCollection<PinModel> Pin { get; set; }
    public DateTime DateEnvio { get; set; } = DateTime.UtcNow;
    public byte[] Image { get; set; }
    public SolicitudPiezaModel SolicitudPieza { get; set; }
    public EnvioProcesadoPopup(SolicitudPiezaModel solicitudPieza)
	{
		InitializeComponent();
        this.BindingContext = this;
        this.SolicitudPieza = solicitudPieza;
        TallerEnvia = solicitudPieza.TallerSolicitado;
        Pin = new ObservableCollection<PinModel>
        {
            new PinModel
            {
                Latitude = solicitudPieza.TSolicitadoLatitud,
                Longitude = solicitudPieza.TSolicitadoLongitud
            }
        };
        OpenGoogleMapsCommand = new Command(async () => await OpenGoogleMapsCommandExecuted());
        SaveCommand = new Command(async () => await SaveCommandExecuted());
        AddImageCommand = new Command(async () => await AddImageCommandExecuted());
    }

    #region Command
    public ICommand OpenGoogleMapsCommand { get; set; }
    public ICommand AddImageCommand { get; set; }
    public ICommand SaveCommand { get; set; }
    #endregion

    #region CommandExecuted
    private async Task OpenGoogleMapsCommandExecuted()
    {
        var location = new Location(SolicitudPieza.TSolicitadoLatitud, SolicitudPieza.TSolicitadoLongitud);
        var options = new MapLaunchOptions { Name = SolicitudPieza.TallerSolicitado };

        try
        {
            await Map.Default.OpenAsync(location, options);
        }
        catch (Exception ex)
        {

        }
    }

    private async Task AddImageCommandExecuted()
    {
        try
        {
            string action = await App.Current.MainPage.DisplayActionSheet("Cargar Imagen", "Cancelar", null, "Cámara", "Galeria");
            if (action == "Galeria")
            {
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    FileResult photo = await MediaPicker.Default.PickPhotoAsync();

                    if (photo != null)
                    {
                        using Stream sourceStream = await photo.OpenReadAsync();
                        Image = ConvertStream.ConvertirStreamAByteArray(sourceStream);
                    }
                }
            }
            else if (action == "Cámara")
            {
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                    if (photo != null)
                    {
                        using Stream sourceStream = await photo.OpenReadAsync();
                        Image = ConvertStream.ConvertirStreamAByteArray(sourceStream);
                    }
                }
            }
        }
        catch(Exception ex)
        {

        }
    }

    private async Task SaveCommandExecuted()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(Solicitud))
            {
                await Toast.Make("Debe agregar la solicitud", CommunityToolkit.Maui.Core.ToastDuration.Long, 16).Show();
                return;
            }
            if (string.IsNullOrWhiteSpace(Mecanico))
            {
                await Toast.Make("Debe agregar el mecánico", CommunityToolkit.Maui.Core.ToastDuration.Long, 16).Show();
                return;
            }
            var date = DateTime.UtcNow;
            if (date < DateEnvio)
            {
                await Toast.Make("La fecha debe ser mayor o igual a la actual", CommunityToolkit.Maui.Core.ToastDuration.Long, 16).Show();
                return;
            }
            if(Image == null || Image.Length == 0)
            {
                await Toast.Make("Debe agregar una imagen", CommunityToolkit.Maui.Core.ToastDuration.Long, 16).Show();
                return; 
            }
            var procesado = new EnvioProcesadoModel
            {
                Solicitud = Solicitud,
                Mecanico = Mecanico,
                FechaEnvio = DateEnvio,
                Imagen = Image
            };
            SelectionChanged?.Invoke(this, procesado);
            Navigation.RemovePage(this);
        }
        catch(Exception ex)
        {

        }
    }
    #endregion

    protected override bool OnBackButtonPressed()
    {
        SelectionChanged?.Invoke(this, null);
        return false;
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            SelectionChanged?.Invoke(this, null);
            Navigation.RemovePage(this);
        }
        catch(Exception ex)
        {

        }
    }
}