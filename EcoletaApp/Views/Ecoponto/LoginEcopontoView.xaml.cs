using Ecoleta.ViewModels.Ecopontos;

namespace Ecoleta.Views.Ecoponto;

public partial class LoginEcopontoView : ContentPage
{
	LoginEcopontoViewModel viewModel;
	public LoginEcopontoView()
	{
		InitializeComponent();

		viewModel = new LoginEcopontoViewModel();
		BindingContext = viewModel;
	}
}