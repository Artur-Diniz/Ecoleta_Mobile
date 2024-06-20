using Ecoleta.ViewModels.Brindes;

namespace Ecoleta.Views.Brinde;

public partial class ImagemBrindeView : ContentPage
{
	ImageBrindeViewModel viewModel;
	public ImagemBrindeView()
	{
		InitializeComponent();

		viewModel = new ImageBrindeViewModel();
		Title = "Imagem Brinde";
		BindingContext = viewModel;
	}
}