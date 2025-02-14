namespace Reto.Service
{
    public interface INavigationService
    {
        Task GoBack();
        Task NavigateToPage<T>(object? parameter = null) where T : Page;
    }
}
