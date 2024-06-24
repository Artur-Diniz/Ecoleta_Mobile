namespace Ecoleta.Views.Utilizador;

public partial class profileView : ContentPage
{
    utilizadorProfileView viewModel;
    public profileView()
	{
		InitializeComponent();
        viewModel = new utilizadorProfileView();
        BindingContext = viewModel;
        Title = "Utilizador - Ecoleta";
    }
}