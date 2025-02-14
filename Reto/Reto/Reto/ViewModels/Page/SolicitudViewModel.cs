using Reto.Db.Entities;
using Reto.Db.Repository;
using Reto.Models;
using Reto.Service;
using Reto.ViewModels.Base;

namespace Reto.ViewModels.Page
{
    public class SolicitudViewModel : BindableBase
    {
        #region Properties
        public TallerModel Taller { get; set; }
        #endregion

        #region Constructor
        private readonly IGenericRepository<Taller> tallerEntities;
        private readonly INavigationService navigationService;

        public SolicitudViewModel(IGenericRepository<Db.Entities.Taller> taller, INavigationService navigationService)
        {
            this.tallerEntities = taller;
            this.navigationService = navigationService;
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
