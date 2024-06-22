using EcoletaApp.ViewModels.Ecopontos;

namespace EcoletaApp.Views.Ecoponto;

public partial class LocalizacaoEcopontosView : ContentPage
{
    LocalizacaoViewModels viewModels;
    public LocalizacaoEcopontosView()
    {
        InitializeComponent();

        viewModels = new LocalizacaoViewModels();
        BindingContext = viewModels;

        viewModels.MeuMapa = mapa;

        viewModels.InicializarMapa();
        viewModels.ExibirUsuariosnoMapa();
    }
}
