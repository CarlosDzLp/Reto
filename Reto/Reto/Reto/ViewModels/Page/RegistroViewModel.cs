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
    public class RegistroViewModel : BindableBase
    {
        #region Properties
        public TallerModel Taller { get; set; }
        public ObservableCollection<RefaccionModel> ListRefaccion { get; set; }
        #endregion

        #region Constructor
        private readonly IGenericRepository<Db.Entities.Taller> tallerEntities;
        private readonly INavigationService navigationService;
        private readonly IGenericRepository<Db.Entities.Refaccion> refaccion;

        public RegistroViewModel(IGenericRepository<Db.Entities.Taller> taller, IGenericRepository<Db.Entities.Refaccion> refaccion, INavigationService navigationService)
        {
            this.tallerEntities = taller;
            this.refaccion = refaccion;
            this.navigationService = navigationService;
            AddRegistroCommand = new Command(async () => await AddRegistroCommandExecuted());
        }
        #endregion

        #region CommandExecuted
        private async Task AddRegistroCommandExecuted()
        {
            try
            {
                if(Taller != null)
                {
                    await navigationService.NavigateToPage<AddRegistroPage>(Taller);
                }
            }
            catch(Exception ex)
            {

            }
        }
        #endregion

        #region Command
        public ICommand AddRegistroCommand { get; set; }       
        #endregion

        #region Methods
        private async Task LoadRefacion()
        {
            try
            {
                ListRefaccion = new ObservableCollection<RefaccionModel>();
                var result = await refaccion.GetAllAsync(c => c.IdTaller == Taller.Id, include: query => query.Include(p => p.Piezas).Include(t => t.Taller));
                foreach(var item in result)
                {
                    ListRefaccion.Add(new RefaccionModel
                    {
                        Id = item.Id,//
                        Cantidad = item.Cantidad, //
                        NombrePieza = item.Piezas.Nombre,//
                        FechaInstalacion = item.FechaInstalacion,
                        NombreTaller = item.Taller.Nombre,//
                        Estatus = item.Estatus,//
                        Solicitud = item.Solicitud,//
                        Imagen = item.Imagen,//
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
                await LoadRefacion();
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

        public override Task OnNavigatedFrom(bool isForwardNavigation)
        {
            return base.OnNavigatedFrom(isForwardNavigation);
        }
        #endregion
    }
}
