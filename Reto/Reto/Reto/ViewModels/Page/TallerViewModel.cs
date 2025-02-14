using Reto.Db.Entities;
using Reto.Db.Repository;
using Reto.Models;
using Reto.Service;
using Reto.ViewModels.Base;
using Reto.Views.Page;
using System.Windows.Input;

namespace Reto.ViewModels.Page
{
    public class TallerViewModel : BindableBase
    {
        #region Properties
        public TallerModel Taller { get; set; }
        #endregion

        #region Constructor
        private readonly IGenericRepository<Taller> tallerEntities;
        private readonly INavigationService navigationService;

        public TallerViewModel(IGenericRepository<Db.Entities.Taller> taller, INavigationService navigationService)
        {
            this.tallerEntities = taller;
            this.navigationService = navigationService;
            SolicitudCommand = new Command(async () => await SolicitudCommandExecuted());
            InstalacionCommand = new Command(async () => await InstalacionCommandExecuted());
            AddPiezaCommand = new Command(async () => await AddPiezaCommandExecuted());
        }
        #endregion

        #region Command
        public ICommand SolicitudCommand { get; set; }
        public ICommand InstalacionCommand { get; set; }
        public ICommand AddPiezaCommand { get; set; }
        #endregion

        #region CommandExecuted
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
