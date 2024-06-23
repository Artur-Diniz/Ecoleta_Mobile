using Ecoleta.ViewModels;
using EcoletaApp.Views.Ecoponto;
using EcoletaApp.Views.Utilizador;

namespace EcoletaApp
{
    public partial class AppShell : Shell
    {
        AppShellViewModel viewModel;
        public AppShell()
        {
            InitializeComponent();

            viewModel = new AppShellViewModel();
            BindingContext = viewModel;

            Routing.RegisterRoute("cadEcopontoView", typeof(Views.Ecoponto.CadastroView));
            Routing.RegisterRoute("cadLoginEcopontoView", typeof(Ecoleta.Views.Ecoponto.CadastroLoginEcoponto));
            Routing.RegisterRoute("cadColetaView", typeof(Views.Coletas.RegistrarColetasView));
            Routing.RegisterRoute("cadBrindeView", typeof(Views.Brinde.RegistrarBrindeView));
            Routing.RegisterRoute("LoginEcoponto", typeof(Ecoleta.Views.Ecoponto.LoginEcopontoView)); 
            Routing.RegisterRoute("LoginUtilizador", typeof(Views.Utilizador.LoginView));
            Routing.RegisterRoute("HomePage", typeof(MainPage));
            Routing.RegisterRoute("ImagemBrinde", typeof(Ecoleta.Views.Brinde.ImagemBrindeView));
            Routing.RegisterRoute("LocEcoponto", typeof(Views.Ecoponto.LocalizacaoEcopontosView));
      
        }
    }
}
