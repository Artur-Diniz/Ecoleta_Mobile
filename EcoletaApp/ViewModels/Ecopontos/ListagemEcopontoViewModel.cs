using EcoletaApp.Models;
using EcoletaApp.Services.Ecopontos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace EcoletaApp.ViewModels.Ecopontos
{
    public class ListagemEcopontoViewModel : BaseViewModel
    {
        private EcopontoService _service;

        public ObservableCollection<Ecoponto> Ecopontos { get; set; }

        public ListagemEcopontoViewModel()
        {
            _service = new EcopontoService();
            Ecopontos = new ObservableCollection<Ecoponto>();

            _ = ObterEcopontos();

            NovoEcoponto = new Command(async () => { await ExibirCadastroEcoponto(); });
            RemoverEcopontoCommand = new Command<Ecoponto>(async (Ecoponto e) => { await RemoverEcoponto(e); });
        }

        public ICommand NovoEcoponto { get; }
        public ICommand RemoverEcopontoCommand { get; }



        private Ecoponto ecopontoSelecionado;
        public Ecoponto EcopontoSelecionado { 
            get { return ecopontoSelecionado; }
            set
            {
                if(value != null)
                {
                    ecopontoSelecionado = value;
                     GravarLocAsync(ecopontoSelecionado);

                    _ = ExibirOpcoes(ecopontoSelecionado);
                }
            }
        }


        public async Task ObterEcopontos()
        {
            try
            { 
                Ecopontos = await _service.GetAllEcopontosAsync();
                OnPropertyChanged(nameof(Ecopontos));
            }
            catch (Exception ex) 
            {
                await Application.Current.MainPage.DisplayAlert("OPS",
                    ex.Message + "Detalhes:" + ex.InnerException, "OK");
            }
        }

        public async Task ExibirCadastroEcoponto()
        {
            try
            {
                await Shell.Current.GoToAsync("cadLoginEcopontoView");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + "Detalhes" + ex.InnerException, "OK");
            }

        }

        public async Task RemoverEcoponto(Ecoponto e)
        {
            try
            {
                if (await Application.Current.MainPage.DisplayAlert("Confirmação", $"Confirma a remoção do ecoponto {e.Nome}?", "Sim", "Não")) ;
                {
                    await _service.DeleteEcopontoAsync(e.IdEcoponto);

                    await Application.Current.MainPage.DisplayAlert("Mensagem", "Ecoponto Removido com sucesso", "OK");

                    _ = ObterEcopontos();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + "Detalhes" + ex.InnerException, "OK");
            }
        }

        public async Task ExibirOpcoes(Ecoponto e)
        {
            try
            {
                ecopontoSelecionado = null;
                string result = string.Empty;

                result = await Application.Current.MainPage
                    .DisplayActionSheet("Opções para Ecoponto:  " + e.Nome,
                    "Observar no Mapa",
                    "Cancelar",
                    "Remover Ecoponto",
                    "Editar Ecoponto");

                if (result != null)
                    ProcessarOpcaoAsync(e, result);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + "Detalhes" + ex.InnerException, "OK");
            }
        }



        public async void ProcessarOpcaoAsync(Ecoponto ecoponto, string result)
        {

            if (result.Equals("Editar Ecoponto"))
            {
                await Shell.Current
                    .GoToAsync($"cadEcopontoView?eId={ecoponto.IdEcoponto}");
            }
            else if (result.Equals("Remover Ecoponto"))
            {
                 await RemoverEcoponto(ecoponto); 
                 await ObterEcopontos();
                
            }
            else if (result.Equals("Observar no Mapa"))
            {
                Shell.Current.GoToAsync("LocEcoponto");
            }
        }

        public async void GravarLocAsync(Ecoponto e)
        {
            Preferences.Set("LatitudeEcoponto", e.Latitude);
            Preferences.Set("LongitudeEcoponto", e.Longitude);
            Preferences.Set("NomeEcoponto", e.Nome);
            Preferences.Set("EnderecoEcoponto", e.Endereco);
        }
    }  
}
