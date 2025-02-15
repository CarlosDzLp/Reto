using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Reto.Db.Repository;
using Reto.Helpers;
using Reto.Models;
using Reto.Service;
using Reto.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Reto.ViewModels.Page
{
    public class AddRegistroViewModel : BindableBase
    {
        #region Properties
        public TallerModel Taller { get; set; }
        public ObservableCollection<PiezasModel> ListPieza { get; set; }
        public ObservableCollection<PinModel> Pins { get; set; }
        public DateTime DateInstalacion { get; set; } = DateTime.UtcNow;
        public byte[] Image { get; set; }
        public int Cantidad { get; set; } = 0;
        public string ErrorPiezas { get; set; }
        public int MaximusPiezas { get; set; }
        public string Solicitud { get; set; }
        public PiezasModel Pieza { get; set; }

        #endregion

        #region Constructor
        private readonly IGenericRepository<Db.Entities.Taller> tallerEntities;
        private readonly INavigationService navigationService;
        public IGenericRepository<Db.Entities.Piezas> piezas;
        private readonly IGenericRepository<Db.Entities.Refaccion> refaccion;

        public AddRegistroViewModel(IGenericRepository<Db.Entities.Taller> taller, IGenericRepository<Db.Entities.Piezas> piezas, IGenericRepository<Db.Entities.Refaccion> refaccion,INavigationService navigationService)
        {
            this.tallerEntities = taller;
            this.piezas = piezas;
            this.refaccion = refaccion;
            this.navigationService = navigationService;
            OpenGoogleMapsCommand = new Command(async () => await OpenGoogleMapsCommandExecuted());
            AddImageCommand = new Command(async () => await AddImageCommandExecuted());
            SaveCommand = new Command(async () => await SaveCommandExecuted());
        }
        #endregion

        #region Command
        public ICommand AddImageCommand { get; set; }
        public ICommand OpenGoogleMapsCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        #endregion

        #region CommandExecuted
        private async Task AddImageCommandExecuted()
        {
            try
            {
                string action = await  App.Current.MainPage.DisplayActionSheet("Cargar Imagen", "Cancelar", null, "Cámara", "Galeria");
                if(action == "Galeria")
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
                else if(action == "Cámara")
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
        private async Task OpenGoogleMapsCommandExecuted()
        {
            var location = new Location(Taller.Latitud, Taller.Longitud);
            var options = new MapLaunchOptions { Name = Taller.Nombre };

            try
            {
                await Map.Default.OpenAsync(location, options);
            }
            catch (Exception ex)
            {
                
            }
        }

        private async Task SaveCommandExecuted()
        {
            try
            {
                ToastDuration duration = ToastDuration.Short;
                double fontSize = 14;
                if (string.IsNullOrWhiteSpace(Solicitud))
                {
                    await Toast.Make("Debe agregar una solicitud", duration, fontSize).Show();
                    return;
                }
                if(Pieza == null)
                {
                    await Toast.Make("Debe seleccionar una pieza", duration, fontSize).Show();
                    return;
                }
                if(Cantidad <= 0)
                {
                    await Toast.Make("No hay inventario", duration, fontSize).Show();
                    return;
                }
                if(Taller == null)
                {
                    await Toast.Make("No hay taller seleccionado", duration, fontSize).Show();
                    return;
                }
                var date = DateTime.UtcNow;
                if(date < DateInstalacion)
                {
                    await Toast.Make("La fecha debe ser mayor o igual a la actual", duration, fontSize).Show();
                    return;
                }
                if (Image == null || Image.Length == 0)
                {
                    await Toast.Make("Cargue una imagen", duration, fontSize).Show();
                    return;
                }
                string fileName = $"{Guid.NewGuid().ToString()}.png";
                string pathImage = await ConvertStream.SaveImageFromBytesAsync(Image, fileName);
                var db = await refaccion.AddAsync(new Db.Entities.Refaccion
                {
                    Estatus = "Instalado",
                    FechaInstalacion = DateInstalacion,
                    IdPieza = Pieza.Id,
                    IdTaller = Taller.Id,
                    Solicitud = Solicitud,
                    Cantidad = Cantidad,
                    Imagen = pathImage
                });
                await refaccion.SaveCommitAsync();
                if(db != null && db.Id > 0)
                {
                    //se descuenta de pezas inventario
                    var resultPiezas = await piezas.FindAsync(c => c.Id == Pieza.Id && c.TallerId == Taller.Id);
                    if(resultPiezas != null)
                    {
                        int resta = resultPiezas.Cantidad - Cantidad;
                        resultPiezas.Cantidad = resta;
                        await piezas.UpdateAsync(resultPiezas);
                        await piezas.SaveCommitAsync();
                        await Toast.Make("Se ha agregado la instalación", duration, fontSize).Show();
                        await navigationService.GoBack();
                    }
                }
                else
                {
                    await Toast.Make("Hubo un error, intente más tarde", duration, fontSize).Show();
                }
            }
            catch(Exception ex)
            {

            }
        }
        #endregion

        #region Methods
        private async Task LoadPiezas()
        {
            try
            {
                ListPieza = new ObservableCollection<PiezasModel>();
                var result = await piezas.GetAllAsync(c => c.TallerId == Taller.Id);
                foreach(var item in result)
                {
                    ListPieza.Add(new PiezasModel
                    {
                        Id = item.Id,
                        Nombre = item.Nombre,
                        Cantidad = item.Cantidad
                    });
                }
            }
            catch(Exception ex)
            {

            }
        }

        private async Task LoadTaller(int idTaller)
        {
            try
            {
                var filter = await tallerEntities.FindAsync(x => x.Id == idTaller);
                if (filter != null)
                {
                    Taller = new TallerModel
                    {
                        Id = filter.Id,
                        Nombre = filter.Nombre,
                        Latitud = filter.Latitud,
                        Longitud = filter.Longitud
                    };
                    Title = Taller.Nombre;
                    Pins = new ObservableCollection<PinModel>()
                    {
                        new PinModel()
                        {
                            Latitude = filter.Latitud,
                            Longitude = filter.Longitud
                        },
                    };
                }

                await LoadPiezas();
            }
            catch (Exception ex)
            {

            }
        }
        public override Task OnNavigatingTo(object? parameter)
        {
            if (parameter is TallerModel item)
            {
                _ = LoadTaller(item.Id);
            }
            return base.OnNavigatingTo(parameter);
        }
        #endregion
    }
}
