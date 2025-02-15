using Reto.Db.Repository;
using Reto.Models;
using Reto.Service;
using Reto.ViewModels.Base;

namespace Reto.ViewModels.Page
{
    public class DetalleSolicitudEnviaViewModel : BindableBase
    {

        #region Properties

        #endregion

        #region Constructor
        private readonly IGenericRepository<Db.Entities.Taller> taller;
        private readonly IGenericRepository<Db.Entities.SolicitudPieza> solicitudPieza;
        private readonly INavigationService navigationService;
        public DetalleSolicitudEnviaViewModel(IGenericRepository<Db.Entities.Taller> taller, IGenericRepository<Db.Entities.SolicitudPieza> solicitudPieza, INavigationService navigationService)
        {
            this.taller = taller;
            this.solicitudPieza = solicitudPieza;
            this.navigationService = navigationService;
        }
        #endregion

        #region Command

        #endregion

        #region CommandExecuted

        #endregion

        #region Methods
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
            //if (Item != null)
                //_ = LoadTaller(Item.Id);
            return base.OnNavigatedTo();
        }
        #endregion
    }
}
