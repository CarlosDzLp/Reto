using Reto.Db.Entities;
using Reto.Db.Repository;
using Reto.Models;
using Reto.Service;
using Reto.ViewModels.Base;
using Reto.Views.Page;
using System.Collections.ObjectModel;

namespace Reto.ViewModels
{
    public class PrincipalViewModel : BindableBase
    {
        #region Properties
        public ObservableCollection<TallerModel> ListTaller { get; set; }
        #endregion

        #region Constructor
        private readonly IGenericRepository<Taller> taller;
        private readonly INavigationService navigationService;

        public PrincipalViewModel(IGenericRepository<Taller> taller,INavigationService navigationService)
        {           
            this.taller = taller;
            this.navigationService = navigationService;
            _ = LoadData();
        }
        #endregion

        #region Methods
        private async Task LoadData()
        {
            try
            {
                var tall1 = await taller.FindAsync(x => x.Id == 1);
                var tall2 = await taller.FindAsync(x => x.Id == 2);
                if(tall1 == null)
                {
                    await taller.AddAsync(new Taller
                    {
                        Id = 1,
                        Nombre = "Juan Mecánico",
                        Icon = "taller1.png",
                        Latitud = 16.7408239,
                        Longitud = -93.1044611
                    });
                    await taller.SaveCommitAsync();
                }

                if (tall2 == null)
                {
                    await taller.AddAsync(new Taller
                    {
                        Id = 2,
                        Nombre = "La Bendición",
                        Icon = "taller2.png",
                        Latitud = 16.738954,
                        Longitud = -93.1023368
                    });
                    await taller.SaveCommitAsync();
                }
                var search = await taller.GetAllAsync();
                ListTaller = new ObservableCollection<TallerModel>();
                foreach (var item in search)
                {
                    ListTaller.Add(new TallerModel
                    {
                        Id = item.Id,
                        Nombre = item.Nombre,
                        Icon = item.Icon
                    });
                }
            }
            catch(Exception ex)
            {

            }
        }

        public async Task NavigateToDetail(TallerModel taller)
        {
            await navigationService.NavigateToPage<TallerPage>(taller);
        }
        #endregion
    }
}
