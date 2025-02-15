using CommunityToolkit.Maui.Alerts;
using Microsoft.EntityFrameworkCore;
using Reto.Db.Repository;
using Reto.Models;
using Reto.Service;
using Reto.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Reto.ViewModels.Page
{
    public class AddSolicitudViewModel : BindableBase
    {
        
        #region Properties
        public TallerModel Taller { get; set; }
        public string Vin { get; set; }
        public ObservableCollection<PiezasModel> ListPiezas { get; set; }
        public string TallerNombre { get; set; }
        public PiezasModel Piezas { get; set; }
        public int Cantidad { get; set; }
        public int PiezasMaxima { get; set; }
        public ObservableCollection<PinModel> Pins { get; set; }
        public bool IsVisibleMap { get; set; }
        public string Mecanico { get; set; }
        public DateTime DateSolicitud { get; set; } = DateTime.UtcNow;
        #endregion

        #region Constructor
        private readonly IGenericRepository<Db.Entities.Taller> taller;
        private readonly IGenericRepository<Db.Entities.SolicitudPieza> solicitudPieza;
        private readonly IGenericRepository<Db.Entities.Piezas> piezas;
        private readonly INavigationService navigationService;
        public AddSolicitudViewModel(IGenericRepository<Db.Entities.Taller> taller,IGenericRepository<Db.Entities.SolicitudPieza> solicitudPieza,IGenericRepository<Db.Entities.Piezas>piezas,INavigationService navigationService)
        {
            this.taller = taller;
            this.solicitudPieza = solicitudPieza;
            this.piezas = piezas;
            this.navigationService = navigationService;
            OpenMapTallerSolicitudCommand = new Command(async () => await OpenMapTallerSolicitudCommandExecuted());
            OpenGoogleMapsCommand = new Command(async () => await OpenGoogleMapsCommandExecuted());
            SaveSolicitudCommand = new Command(async () => await SaveSolicitudCommandExecuted());
        }
        #endregion

        #region Commands
        public ICommand OpenMapTallerSolicitudCommand { get; set; }
        public ICommand OpenGoogleMapsCommand { get; set; }
        public ICommand SaveSolicitudCommand { get; set; }
        #endregion

        #region CommandExecuted
        private async Task SaveSolicitudCommandExecuted()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Vin))
                {
                    await Toast.Make("Debe agregar el vin del vehiculo", CommunityToolkit.Maui.Core.ToastDuration.Long, 16).Show();
                    return;
                }
                if (string.IsNullOrWhiteSpace(Mecanico))
                {
                    await Toast.Make("Debe agregar el nombre del mecánico", CommunityToolkit.Maui.Core.ToastDuration.Long, 16).Show();
                    return;
                }
                if(Piezas == null)
                {
                    await Toast.Make("Debe seleccionar una pieza", CommunityToolkit.Maui.Core.ToastDuration.Long, 16).Show();
                    return;
                }
                if(Cantidad <=0)
                {
                    await Toast.Make("La cantidad debe ser mayor a cero", CommunityToolkit.Maui.Core.ToastDuration.Long, 16).Show();
                    return;
                }
                var date = DateTime.UtcNow;
                if(date < DateSolicitud)
                {
                    await Toast.Make("La fecha de la solicitud debe ser mayor o igual a la actual", CommunityToolkit.Maui.Core.ToastDuration.Long, 16).Show();
                    return;
                }
                var db = await solicitudPieza.AddAsync(new Db.Entities.SolicitudPieza
                {
                    Cantidad = Cantidad,
                    FechaSolicitud = DateSolicitud,
                    Mecanico = Mecanico,
                    PiezaId = Piezas.Id,
                    TallerSolicitadoId = Piezas.TallerId,
                    TallerSolicitaId = Taller.Id,
                    Vin = Vin,
                    EstatusSolicitud = nameof(EstatusSolicitud.Pendiente)
                });
                await solicitudPieza.SaveCommitAsync();
                if(db != null && db.Id > 0)
                {
                    await Toast.Make("Se ha registrado la solicitud", CommunityToolkit.Maui.Core.ToastDuration.Long, 16).Show();
                    await navigationService.GoBack();
                }
                else
                {
                    await Toast.Make("Hubo un error, intente más tarde", CommunityToolkit.Maui.Core.ToastDuration.Long, 16).Show();
                }
            }
            catch(Exception ex)
            {

            }
        }
        private async Task OpenMapTallerSolicitudCommandExecuted()
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
        private async Task OpenGoogleMapsCommandExecuted()
        {
            var filter = await taller.FindAsync(c => c.Id == Piezas.TallerId);
            if (filter == null)
                return;
            var location = new Location(filter.Latitud, filter.Longitud);
            var options = new MapLaunchOptions { Name = filter.Nombre };
            try
            {
                await Map.OpenAsync(location, options);
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region Methods
        public async Task LoadPiezaItem(PiezasModel item)
        {
            try
            {
                if (item == null)
                    return;
                var filter = await taller.FindAsync(c => c.Id == item.TallerId);
                if (filter == null)
                    return;
                Pins = new ObservableCollection<PinModel>
                {
                    new PinModel 
                    { 
                        Latitude = filter.Latitud, 
                        Longitude = filter.Longitud
                    } 
                };
                IsVisibleMap = true;
                Cantidad = 0;
                Piezas = item;
                PiezasMaxima = item.Cantidad;
                TallerNombre = $"Taller: {item.TallerNombre}";
            }
            catch(Exception ex)
            {

            }
        }

        private async Task LoadPiezas()
        {
            try
            {
                var result = await piezas.GetAllAsync(c => c.TallerId != Taller.Id && c.Cantidad > 0, include: query => query.Include(t => t.Taller));
                ListPiezas = new ObservableCollection<PiezasModel>();
                foreach(var item in result)
                {
                    ListPiezas.Add(new PiezasModel
                    {
                        Cantidad = item.Cantidad,
                        Id = item.Id,
                        Nombre = $"{item.Nombre} - {item.Taller.Nombre}",
                        TallerNombre = item.Taller.Nombre,
                        TallerId = item.TallerId
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
                var filter = await taller.FindAsync(x => x.Id == idTaller);
                if (filter != null)
                {
                    Taller = new TallerModel
                    {
                        Id = filter.Id,
                        Nombre = filter.Nombre,
                        Icon = filter.Icon,
                        Latitud = filter.Latitud,
                        Longitud = filter.Longitud,
                    };
                    Title = $"Solicitante: Taller {Taller.Nombre}";
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
