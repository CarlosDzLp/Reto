using CommunityToolkit.Maui.Alerts;
using Microsoft.EntityFrameworkCore;
using Reto.Db.Entities;
using Reto.Db.Repository;
using Reto.Models;
using Reto.Service;
using Reto.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Reto.ViewModels.Page
{
    public class AddPiezaViewModel : BindableBase
    {
        #region Properties
        public string Nombre { get; set; }
        public int Piezas { get; set; }
        public TallerModel Taller { get; set; }
        public ObservableCollection<PiezasModel> ListPiezas { get; set; }
        #endregion

        #region Constructor
        private readonly IGenericRepository<Taller> tallerEntities;
        private readonly IGenericRepository<Piezas> piezas;
        private readonly INavigationService navigationService;

        public AddPiezaViewModel(IGenericRepository<Db.Entities.Taller> taller,IGenericRepository<Db.Entities.Piezas> piezas, INavigationService navigationService)
        {
            this.tallerEntities = taller;
            this.piezas = piezas;
            this.navigationService = navigationService;
            SaveCommand = new Command(async () => await SaveCommandExecuted());
            
        }
        #endregion

        #region Command
        public ICommand SaveCommand { get; set; }
        #endregion

        #region CommandExecuted
        private async Task SaveCommandExecuted()
        {
            try
            {
                if(string.IsNullOrWhiteSpace(Nombre))
                {
                    await Toast.Make("Nombre es requerido", CommunityToolkit.Maui.Core.ToastDuration.Long, 16).Show();
                    return;
                }
                if (Piezas == 0 || Piezas < 0)
                {
                    await Toast.Make("Las piezas son requeridas", CommunityToolkit.Maui.Core.ToastDuration.Long, 16).Show();
                    return;
                }
                if(Taller == null)
                {
                    await Toast.Make("No se encuentra el taller", CommunityToolkit.Maui.Core.ToastDuration.Long, 16).Show();
                    return;
                }

                var pi = await piezas.AddAsync(new Db.Entities.Piezas
                {
                    Nombre = Nombre,
                    Cantidad = Piezas,
                    TallerId = Taller.Id,
                });
                await piezas.SaveCommitAsync();
                if(pi != null && pi.Id > 0)
                {
                    await Toast.Make("Se ha registrado la pieza", CommunityToolkit.Maui.Core.ToastDuration.Long, 16).Show();
                    Nombre = string.Empty;
                    Piezas = 0;
                    await LoadPiezas();
                }
                else
                {
                    await Toast.Make("No se ha registrado la pieza", CommunityToolkit.Maui.Core.ToastDuration.Long, 16).Show();
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
                ListPiezas = new ObservableCollection<PiezasModel>();
                var search = await piezas.GetAllAsync(c => c.TallerId == Taller.Id, include: query => query.Include(t => t.Taller));
                foreach(var item in search)
                {
                    ListPiezas.Add(new PiezasModel
                    {
                        Id = item.Id,
                        Nombre = item.Nombre,
                        Cantidad = item.Cantidad,
                        TallerNombre = item.Taller.Nombre
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
