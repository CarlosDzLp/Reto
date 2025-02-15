using Microsoft.EntityFrameworkCore;
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
        private readonly INavigationService navigationService;

        public SolicitudViewModel(IGenericRepository<Db.Entities.Taller> taller, IGenericRepository<Db.Entities.SolicitudPieza> solicitudPieza,INavigationService navigationService)
        {
            this.tallerEntities = taller;
            this.solicitudPieza = solicitudPieza;
            this.navigationService = navigationService;
            AddSolicitudCommand = new Command(async () => await AddSolicitudCommandExecuted());
        }
        #endregion

        #region Command
        public ICommand AddSolicitudCommand { get; set; }
        #endregion

        #region CommandExcuted
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
