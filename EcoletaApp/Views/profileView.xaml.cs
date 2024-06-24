using Ecoleta.ViewModels.Utililizador;
using EcoletaApp.ViewModels.Utililizador;

namespace Ecoleta.Views;

public partial class profileView : ContentPage
{
    UtilizadorProfileViewModel viewModel;
    public profileView()
	{
		InitializeComponent();
        viewModel = new UtilizadorProfileViewModel();
        BindingContext = viewModel;
        Title = "Utilizador ";
    }
}