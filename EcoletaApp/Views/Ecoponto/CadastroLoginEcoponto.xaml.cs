using EcoletaApp.ViewModels.Ecopontos;

namespace Ecoleta.Views.Ecoponto;

public partial class CadastroLoginEcoponto : ContentPage
{
    private CadastroEcopontoViewModel cadViewModel;

    public CadastroLoginEcoponto()
	{
		InitializeComponent();

        cadViewModel = new CadastroEcopontoViewModel();
        BindingContext = cadViewModel;
        Title = "Novo Ecoponto";
    }
}