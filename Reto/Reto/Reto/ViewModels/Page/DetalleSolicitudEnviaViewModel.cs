using CommunityToolkit.Maui.Alerts;
using Microsoft.EntityFrameworkCore;
using Reto.Db.Entities;
using Reto.Db.Repository;
using Reto.Helpers;
using Reto.Models;
using Reto.Service;
using Reto.ViewModels.Base;
using Reto.Views.Page.Popup;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Reto.ViewModels.Page
{
    public class DetalleSolicitudEnviaViewModel : BindableBase
    {

        #region Properties
        public SolicitudPiezaModel Solicitud { get; set; }
        public ObservableCollection<PinModel>PinSolicita { get; set; }
        public ObservableCollection<PinModel> PinSolicitado { get; set; }
        public bool IsVisibleTracker { get; set; }
        public bool BtnAceptado { get; set; }
        public bool BtnRechazado { get; set; }
        public bool BtnEnviado { get; set; }
        public bool ContentProcesoEnvio { get; set; }
        public EnvioProcesadoModel Proceso { get; set; }
        #endregion

        #region Constructor
        private readonly IGenericRepository<Db.Entities.Taller> taller;
        private readonly IGenericRepository<Db.Entities.SolicitudPieza> solicitudPieza;
        private readonly IGenericRepository<ProcesoEnvio> procesoEnvio;
        private readonly IGenericRepository<Db.Entities.Piezas> pieza;
        private readonly INavigationService navigationService;
        public DetalleSolicitudEnviaViewModel(IGenericRepository<Db.Entities.Taller> taller, IGenericRepository<Db.Entities.SolicitudPieza> solicitudPieza, IGenericRepository<Db.Entities.ProcesoEnvio> procesoEnvio, IGenericRepository<Db.Entities.Piezas> pieza, INavigationService navigationService)
        {
            this.taller = taller;
            this.solicitudPieza = solicitudPieza;
            this.procesoEnvio = procesoEnvio;
            this.pieza = pieza;
            this.navigationService = navigationService;
            OpenGoogleMapsSolicitadoCommand = new Command(async () => await OpenGoogleMapsSolicitadoCommandExecuted());
            OpenGoogleMapsSolicitaCommand = new Command(async () => await OpenGoogleMapsSolicitaCommandExecuted());
            AceptadoCommand = new Command(async () => await AceptadoCommandExecuted());
            RechazadoCommand = new Command(async () => await RechazadoCommandExecuted());
            EnviadoCommand = new Command(async () => await EnviadoCommandExecuted());
        }
        #endregion

        #region Command
        public ICommand OpenGoogleMapsSolicitadoCommand { get; set; }
        public ICommand OpenGoogleMapsSolicitaCommand { get; set; }
        public ICommand AceptadoCommand { get; set; }
        public ICommand RechazadoCommand { get; set; }
        public ICommand EnviadoCommand { get; set; }
        #endregion

        #region CommandExecuted
        EnvioProcesadoPopup envioProcesadoPopup;
        private async Task EnviadoCommandExecuted()
        {
            try
            {
                var result = await solicitudPieza.FindAsync(c => c.Id == Solicitud.Id);
                if (result != null)
                {
                    bool answer = await App.Current.MainPage.DisplayAlert("Estatus", "¿Desea cambiar el estatus a ENVIADO?", "Si", "No");
                    if (answer)
                    {
                        envioProcesadoPopup = new EnvioProcesadoPopup(Solicitud);
                        envioProcesadoPopup.SelectionChanged += EnvioProcesadoPopup_SelectionChanged;
                        var page = Application.Current?.MainPage ?? throw new NullReferenceException();
                        await page.Navigation.PushAsync(envioProcesadoPopup);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private async Task RechazadoCommandExecuted()
        {
            try
            {
                var result = await solicitudPieza.FindAsync(c => c.Id == Solicitud.Id);
                if (result != null)
                {
                    bool answer = await App.Current.MainPage.DisplayAlert("Estatus", "¿Desea cambiar el estatus a RECHAZADO?", "Si", "No");
                    if (answer)
                    {
                        result.EstatusSolicitud = nameof(EstatusSolicitud.Rechazado);
                        await solicitudPieza.UpdateAsync(result);
                        await solicitudPieza.SaveCommitAsync();
                        await Toast.Make("Se ha cambiado el estatus", CommunityToolkit.Maui.Core.ToastDuration.Long, 16).Show();
                        await LoadSolicitud(Solicitud.Id);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        private async Task AceptadoCommandExecuted()
        {
            try
            {
                var result = await solicitudPieza.FindAsync(c => c.Id == Solicitud.Id);
                if(result != null)
                {
                    bool answer = await App.Current.MainPage.DisplayAlert("Estatus", "¿Desea cambiar el estatus a ACEPTADO?", "Si", "No");
                    if (answer)
                    {
                        result.EstatusSolicitud = nameof(EstatusSolicitud.Aceptado);
                        await solicitudPieza.UpdateAsync(result);
                        await solicitudPieza.SaveCommitAsync();
                        await Toast.Make("Se ha cambiado el estatus", CommunityToolkit.Maui.Core.ToastDuration.Long, 16).Show();
                        await LoadSolicitud(Solicitud.Id);
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }
        private async Task OpenGoogleMapsSolicitadoCommandExecuted()
        {
            var location = new Location(Solicitud.TSolicitadoLatitud, Solicitud.TSolicitadoLongitud);
            var options = new MapLaunchOptions { Name = Solicitud.TallerSolicitado };

            try
            {
                await Map.Default.OpenAsync(location, options);
            }
            catch (Exception ex)
            {

            }
        }

        private async Task OpenGoogleMapsSolicitaCommandExecuted()
        {
            var location = new Location(Solicitud.TSolicitaLatitud, Solicitud.TSolicitaLongitud);
            var options = new MapLaunchOptions { Name = Solicitud.TallerSolicita };

            try
            {
                await Map.Default.OpenAsync(location, options);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Methods
        private async Task LoadProcesoEnvio()
        {
            try
            {
                if(Solicitud != null)
                {
                    var result = await procesoEnvio.FindAsync(c => c.SolicitudId == Solicitud.Id);
                    if(result != null)
                    {
                        Proceso = new EnvioProcesadoModel
                        {
                            Id = result.Id,
                            Estatus = result.Estatus,
                            FechaEnvio = result.FechaEnvio,
                            ImagenPath = result.Imagen,
                            Mecanico = result.Mecanico,
                            SolicitudId = result.SolicitudId,
                        };
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }

        private async void EnvioProcesadoPopup_SelectionChanged(object? sender, EnvioProcesadoModel e)
        {
            try
            {
                envioProcesadoPopup.SelectionChanged -= EnvioProcesadoPopup_SelectionChanged;
                envioProcesadoPopup = null;
                if (e != null)
                {
                    var result = await solicitudPieza.FindAsync(c => c.Id == Solicitud.Id);
                    if (result != null)
                    {
                        var pza = await pieza.FindAsync(c => c.Id == result.PiezaId);
                        if (pza != null)
                        {
                            int resta = pza.Cantidad - result.Cantidad;
                            if (resta < 0)
                            {
                                await App.Current.MainPage.DisplayAlert("Error", "No hay suficientes piezas en inventario", "Aceptar");
                                return;
                            }
                            result.EstatusSolicitud = nameof(EstatusSolicitud.Enviado);
                            await solicitudPieza.UpdateAsync(result);
                            await solicitudPieza.SaveCommitAsync();

                            pza.Cantidad = resta;
                            await pieza.UpdateAsync(pza);
                            await pieza.SaveCommitAsync();

                            string fileName = $"{Guid.NewGuid().ToString()}.png";
                            string pathImage = await ConvertStream.SaveImageFromBytesAsync(e.Imagen, fileName);
                            await procesoEnvio.AddAsync(new ProcesoEnvio
                            {
                                Imagen = pathImage,
                                Estatus = nameof(EstatusSolicitud.Enviado),
                                FechaEnvio = e.FechaEnvio,
                                Mecanico = e.Mecanico,
                                SolicitudId = Solicitud.Id,
                                TallerEnviaId = result.TallerSolicitadoId,
                            });
                            await procesoEnvio.SaveCommitAsync();

                            await Toast.Make("Se ha cambiado el estatus", CommunityToolkit.Maui.Core.ToastDuration.Long, 16).Show();
                            await LoadSolicitud(Solicitud.Id);
                        }
                        else
                            await Toast.Make("Hubo un error, intente más tarde", CommunityToolkit.Maui.Core.ToastDuration.Long, 16).Show();
                    }                       
                }
            }
            catch (Exception ex)
            {

            }
        }


        private async Task LoadSolicitud(int IdSolicitud)
        {
            try
            {
                ContentProcesoEnvio = false;
                var result = await solicitudPieza.FindAsync(x => x.Id == IdSolicitud, include: query => query.Include(p => p.Pieza).Include(t => t.TallerSolicita).Include(t => t.TallerSolicitado));
                if(result != null)
                {
                    Solicitud = new SolicitudPiezaModel
                    {
                        Cantidad = result.Cantidad,//
                        EstatusSolicitud = result.EstatusSolicitud,//
                        FechaSolicitud = result.FechaSolicitud,//
                        Id = result.Id,//
                        IdPieza = result.PiezaId,//
                        Mecanico = result.Mecanico,//
                        Vin = result.Vin,//
                        Pieza = result.Pieza.Nombre,//
                        TallerSolicitado = result.TallerSolicitado.Nombre,//
                        TallerSolicita = result.TallerSolicita.Nombre,//
                        TSolicitadoLatitud = result.TallerSolicitado.Latitud,//
                        TSolicitadoLongitud = result.TallerSolicitado.Longitud,//
                        TSolicitaLatitud = result.TallerSolicita.Latitud,//
                        TSolicitaLongitud = result.TallerSolicita.Longitud//
                    };
                    PinSolicita = new ObservableCollection<PinModel>
                    {
                        new PinModel
                        {
                            Latitude = Solicitud.TSolicitaLatitud,
                            Longitude = Solicitud.TSolicitaLongitud
                        }
                    };
                    PinSolicitado = new ObservableCollection<PinModel>
                    {
                        new PinModel
                        {
                            Latitude = Solicitud.TSolicitadoLatitud,
                            Longitude = Solicitud.TSolicitadoLongitud
                        }
                    };
                    if (Solicitud.EstatusSolicitud == nameof(EstatusSolicitud.Recibido) || Solicitud.EstatusSolicitud == nameof(EstatusSolicitud.Rechazado))
                        IsVisibleTracker = false;
                    else
                        IsVisibleTracker = true;

                    if(Solicitud.EstatusSolicitud == nameof(EstatusSolicitud.Pendiente))
                    {
                        BtnAceptado = true;
                        BtnEnviado = false;
                        BtnRechazado = true;
                    }
                    else if(Solicitud.EstatusSolicitud == nameof(EstatusSolicitud.Aceptado))
                    {
                        BtnAceptado = false;
                        BtnEnviado = true;
                        BtnRechazado = true;
                    }
                    else if(Solicitud.EstatusSolicitud == nameof(EstatusSolicitud.Enviado))
                    {                        
                        BtnAceptado = false;
                        BtnEnviado = false;
                        BtnRechazado = false;                       
                    }
                    if(Solicitud.EstatusSolicitud == nameof(EstatusSolicitud.Enviado) || Solicitud.EstatusSolicitud == nameof(EstatusSolicitud.Recibido))
                    {
                        ContentProcesoEnvio = true;
                        await LoadProcesoEnvio();
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }
        private SolicitudPiezaModel Item { get; set; }
        public override Task OnNavigatingTo(object? parameter)
        {
            if (parameter is SolicitudPiezaModel item)
            {
                Item = item;

            }
            return base.OnNavigatingTo(parameter);
        }

        public override Task OnNavigatedTo()
        {
            if (Item != null)
                _ = LoadSolicitud(Item.Id);
            return base.OnNavigatedTo();
        }
        #endregion
    }
}
