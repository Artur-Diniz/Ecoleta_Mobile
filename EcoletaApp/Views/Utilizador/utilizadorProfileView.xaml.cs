namespace Ecoleta.Views.Utilizador;

public partial class utilizadorProfileView : ContentPage
{
	utilizadorProfileView viewModel;
	public utilizadorProfileView()
	{
		InitializeComponent();

		viewModel = new utilizadorProfileView();
		BindingContext = viewModel;
		Title = "Utilizador - Ecoleta";
	}


}