using Reto.Views;

namespace Reto
{
    public partial class App : Application
    {
        private PrincipalPage tallerPage;
        public App(PrincipalPage page)
        {
            InitializeComponent();
            tallerPage = page;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new NavigationPage(tallerPage));
        }
    }
}
