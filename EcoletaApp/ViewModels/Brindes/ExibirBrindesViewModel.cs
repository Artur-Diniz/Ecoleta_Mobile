using EcoletaApp.Services.Brindes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcoletaApp.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EcoletaApp.ViewModels.Brindes
{
    public class ExibirBrindesViewModel : BaseViewModel
    {
        private BrindesServices bService;
        public ExibirBrindesViewModel()
        {
            bService = new BrindesServices();
            Brindes = new ObservableCollection<Brinde>();

            _ = ObterBrindes();

            RegistrarCommand = new Command(async () => { await ExibirTelaRegistro(); });
            RemoverCommand = new Command<Brinde>(async (Brinde b) => { await RemoverBrinde(b); });
        }

        public ObservableCollection<Brinde> Brindes {  get; set; }
        public ICommand RegistrarCommand { get; }
        public ICommand RemoverCommand { get; }


        private Brinde brindeSelecionado;

        public Brinde BrindeSelecionado 
        { 
            get {  return brindeSelecionado; }                    
            set { if (value != null) brindeSelecionado = value; Preferences.Set("BrindeId", brindeSelecionado.IdBrinde);  _ = ExibirOpcoes(brindeSelecionado); } 
        }

        public async Task ObterBrindes()
        {
            try
            { 
                Brindes = await bService.GetAllBrindeAsync();
                
                OnPropertyChanged(nameof(Brindes));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes :" + ex.InnerException, "Ok");
            }
        }

        public async Task ExibirTelaRegistro()
        {
            try
            {
                await Shell.Current.GoToAsync("cadBrindeView");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes :" + ex.InnerException, "Ok");
            }
        }


        public async Task RemoverBrinde(Brinde b)
        {
            try
            {
                if (await Application.Current.MainPage.DisplayAlert("Confirmação", $"Confirmação a remoção da Brinde com Id: {b.IdBrinde}?", "Sim", "Não")) ;
                {
                    await bService.DeleteBrindeAsync(b.IdBrinde);

                    await Application.Current.MainPage.DisplayAlert("Mensagem", "Coleta Removida Com sucesso!", "OK");

                    _ = ObterBrindes();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes :" + ex.InnerException, "Ok");
            }
        }


        public async Task ExibirOpcoes(Brinde b)
        {
            try
            {
                brindeSelecionado = null;
                string result = string.Empty;

                result = await Application.Current.MainPage
                    .DisplayActionSheet("Opções para Coleta:  ",
                    "Cancelar",
                    "Editar Brinde",
                    "Remover Brinde",
                    "Alterar Foto do Brinde"
                    );

                if (result != null)
                    ProcessarOpcaoAsync(b, result);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + "Detalhes" + ex.InnerException, "OK");
            }
        }

        public async void ProcessarOpcaoAsync(Brinde brinde, string result)
        {

            if (result.Equals("Editar Brinde"))
            {
                await Shell.Current
                    .GoToAsync($"cadBrindeView?bId={brinde.IdBrinde}");
            }
            else if (result.Equals("Remover Brinde"))
            {
                await RemoverBrinde(brinde);
                await ObterBrindes();

            }
            else if (result.Equals("Alterar Foto do Brinde"))
            {
                await Shell.Current.GoToAsync("ImagemBrinde");
            }
        }

    }
}
