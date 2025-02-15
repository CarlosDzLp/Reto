using CommunityToolkit.Maui.Views;
using Microsoft.EntityFrameworkCore;
using Reto.Db.Repository;
using Reto.Models;
using Reto.Service;
using Reto.ViewModels.Base;
using Reto.Views.Page;
using Reto.Views.Page.Popup;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Reto.ViewModels.Page
{
    public class TallerViewModel : BindableBase
    {
        #region Properties
        public TallerModel Taller { get; set; }
        public bool IsVisibleSolicitud { get; set; }
        private List<SolicitudPiezaModel>ListSolicitudPieza { get; set; }
        #endregion

        #region Constructor
        private readonly IGenericRepository<Db.Entities.Taller> tallerEntities;
        private readonly IGenericRepository<Db.Entities.SolicitudPieza> solicitudPiezas;
        private readonly INavigationService navigationService;

        public TallerViewModel(IGenericRepository<Db.Entities.Taller> taller, IGenericRepository<Db.Entities.SolicitudPieza> solicitudPiezas, INavigationService navigationService)
        {
            this.tallerEntities = taller;
            this.solicitudPiezas = solicitudPiezas;
            this.navigationService = navigationService;
            SolicitudCommand = new Command(async () => await SolicitudCommandExecuted());
            InstalacionCommand = new Command(async () => await InstalacionCommandExecuted());
            AddPiezaCommand = new Command(async () => await AddPiezaCommandExecuted());
            SendPiezaCommand = new Command(async () => await SendPiezaCommandExecuted());
        }
        #endregion

        #region Command
        public ICommand SolicitudCommand { get; set; }
        public ICommand InstalacionCommand { get; set; }
        public ICommand AddPiezaCommand { get; set; }
        public ICommand SendPiezaCommand { get; set; }
        #endregion

        #region CommandExecuted
        SolicitudPiezasPopup popup;
        private async Task SendPiezaCommandExecuted()
        {
            try
            {
                popup = new SolicitudPiezasPopup(ListSolicitudPieza);
                popup.SelectionChanged += Popup_SelectionChanged;
                await App.Current.MainPage.ShowPopupAsync(popup, CancellationToken.None);
            }
            catch(Exception ex)
            {

            }
        }

        private async void Popup_SelectionChanged(object? sender, SolicitudPiezaModel e)
        {
            try
            {
                popup.SelectionChanged -= Popup_SelectionChanged;
                popup = null;
                if (e!= null)
                {
                    await navigationService.NavigateToPage<DetalleSolicitudEnviaPage>(e);
                }
            }
            catch(Exception ex)
            {

            }
        }

        private async Task SolicitudCommandExecuted()
        {
            try
            {
                if(Taller != null)
                {
                    await navigationService.NavigateToPage<SolicitudPage>(Taller);
                }
            }
            catch(Exception ex)
            {

            }
        }

        private async Task InstalacionCommandExecuted()
        {
            try
            {
                if (Taller != null)
                {
                    await navigationService.NavigateToPage<RegistroPage>(Taller);
                }
            }
            catch(Exception ex)
            {

            }
        }

        private async Task AddPiezaCommandExecuted()
        {
            try
            {
                if (Taller != null)
                {
                    await navigationService.NavigateToPage<AddPiezaPage>(Taller);
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Methods
        private async Task LoadSolicitudPendientes()
        {
            try
            {
                ListSolicitudPieza = new List<SolicitudPiezaModel>();
                IsVisibleSolicitud = false;
                var result = await solicitudPiezas.GetAllAsync(c => c.TallerSolicitadoId == Taller.Id && c.EstatusSolicitud != nameof(EstatusSolicitud.Recibido), include: query => query.Include(p => p.Pieza).Include(t => t.TallerSolicita));
                if(result != null && result.Count() > 0)
                {
                    IsVisibleSolicitud = true;
                }
                foreach(var item in result)
                {
                    ListSolicitudPieza.Add(new SolicitudPiezaModel
                    {
                        Id = item.Id,
                        Pieza = item.Pieza.Nombre,
                        Mecanico = item.Mecanico,
                        TallerSolicitado = item.TallerSolicita.Nombre
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
                await LoadSolicitudPendientes();
            }
            catch(Exception ex)
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
