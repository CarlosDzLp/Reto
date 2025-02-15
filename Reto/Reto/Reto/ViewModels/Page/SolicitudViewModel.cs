using Microsoft.EntityFrameworkCore;
using Reto.Db.Entities;
using Reto.Db.Repository;
using Reto.Models;
using Reto.Service;
using Reto.ViewModels.Base;
using Reto.Views.Page;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Reto.ViewModels.Page
{
    public class SolicitudViewModel : BindableBase
    {
        #region Properties
        public TallerModel Taller { get; set; }
        public ObservableCollection<SolicitudPiezaModel> ListSolicitudPieza { get; set; }
        #endregion

        #region Constructor
        private readonly IGenericRepository<Db.Entities.Taller> tallerEntities;
        private readonly IGenericRepository<Db.Entities.SolicitudPieza> solicitudPieza;
        private readonly IGenericRepository<Db.Entities.Piezas> piezas;
        private readonly INavigationService navigationService;

        public SolicitudViewModel(IGenericRepository<Db.Entities.Taller> taller, IGenericRepository<Db.Entities.SolicitudPieza> solicitudPieza, IGenericRepository<Db.Entities.Piezas> piezas, INavigationService navigationService)
        {
            this.tallerEntities = taller;
            this.solicitudPieza = solicitudPieza;
            this.piezas = piezas;
            this.navigationService = navigationService;
            AddSolicitudCommand = new Command(async () => await AddSolicitudCommandExecuted());
            RecibidoCommand = new Command<SolicitudPiezaModel>(async (e) => await RecibidoCommandExecuted(e));
        }
        #endregion

        #region Command
        public ICommand AddSolicitudCommand { get; set; }
        public ICommand RecibidoCommand { get; set; }
        #endregion

        #region CommandExcuted
        private async Task RecibidoCommandExecuted(SolicitudPiezaModel item)
        {
            try
            {
                if(item != null)
                {
                    bool answer = await App.Current.MainPage.DisplayAlert("Estatus", "¿Desea aceptar la pieza?", "Si", "No");
                    if (answer)
                    {
                        var soli = await solicitudPieza.FindAsync(x => x.Id == item.Id, include: query => query.Include(p => p.Pieza));
                        soli.EstatusSolicitud = nameof(EstatusSolicitud.Recibido);
                        await solicitudPieza.UpdateAsync(soli);
                        await solicitudPieza.SaveCommitAsync();

                        await piezas.AddAsync(new Piezas
                        {
                            Cantidad = soli.Cantidad,
                            Nombre = soli.Pieza.Nombre,
                            TallerId = Taller.Id,
                        });
                        await piezas.SaveCommitAsync();

                        await LoadSolicitudPieza();
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }
        private async Task AddSolicitudCommandExecuted()
        {
            try
            {
                await navigationService.NavigateToPage<AddSolicitudPage>(Taller);
            }
            catch(Exception ex)
            {

            }
        }
        #endregion

        #region Methods
        private async Task LoadSolicitudPieza()
        {
            try
            {
                ListSolicitudPieza = new ObservableCollection<SolicitudPiezaModel>();
                var result = await solicitudPieza.GetAllAsync(c => c.TallerSolicitaId == Taller.Id, include: query => query.Include(t => t.TallerSolicitado).Include(p => p.Pieza));
                foreach(var item in result)
                {
                    ListSolicitudPieza.Add(new SolicitudPiezaModel
                    {
                        Id = item.Id,//
                        Vin = item.Vin,//
                        Cantidad = item.Cantidad,//
                        Pieza = item.Pieza.Nombre,//
                        Mecanico = item.Mecanico,//
                        TallerSolicitado = item.TallerSolicitado.Nombre,//
                        EstatusSolicitud = item.EstatusSolicitud,//
                        FechaSolicitud = item.FechaSolicitud,//
                        IsVisibleBtn = item.EstatusSolicitud == nameof(EstatusSolicitud.Enviado) ? true : false
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
                        Nombre = filter.Nombre
                    };
                    Title = Taller.Nombre;
                }
                await LoadSolicitudPieza();
            }
            catch (Exception ex)
            {

            }
        }

        private TallerModel Item { get; set; }
        public override Task OnNavigatingTo(object? parameter)
        {
            if (parameter is TallerModel item)
            {
                Item = item;
                
            }
            return base.OnNavigatingTo(parameter);
        }

        public override Task OnNavigatedTo()
        {
            if (Item != null)
                _ = LoadTaller(Item.Id);
            return base.OnNavigatedTo();
        }
        #endregion
    }
}
