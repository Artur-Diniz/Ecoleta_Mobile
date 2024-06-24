using EcoletaApp.Views.Utilizador;
using EcoletaApp.ViewModels.Utililizador;
using Ecoleta.Views.Ecoponto;


namespace EcoletaApp
{
    public partial class App : Application
    {
         utlizadorViewModel viewModel;
        LoginEcopontoView viewEcopontoModel;
        AppShell shell;

        public App()
        {

            InitializeComponent();
            viewModel = new utlizadorViewModel();  
            BindingContext = viewModel;


            MainPage = new LoginView();
        }
        
        protected override void OnStart()
        {
            viewEcopontoModel = new LoginEcopontoView();
            MainPage = new NavigationPage(new LoginView( ));
        }
    }
}
