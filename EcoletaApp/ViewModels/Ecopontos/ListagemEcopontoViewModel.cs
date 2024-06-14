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

                    Shell.Current.GoToAsync($"cadEcopontoView?eId={ecopontoSelecionado.IdEcoponto}");
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
                await Shell.Current.GoToAsync("cadEcopontoView");
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

        public async void ProcessarOpcaoAsync(Ecoponto ecoponto, string result)
        {

            if (result.Equals("Editar Ecoponto"))
            {
                await Shell.Current
                    .GoToAsync($"cadEcopontoView?eId={ecopontoSelecionado.IdEcoponto}");
            }
            else if (result.Equals("Remover Ecoponto"))
            {
                if (await Application.Current.MainPage.DisplayAlert("Confirmação",
                $"Deseja realmente remover o Ecoponto {ecoponto.Nome.ToUpper()}?",
               "Yes", "No"))
                {
                    await RemoverEcoponto(ecoponto);
                    await Application.Current.MainPage.DisplayAlert("Informação",
                     "Ecoponto removido com sucesso!", "Ok");
                    await ObterEcopontos();
                }
            }
            //aqui Vai ter uma opção para ao selecionar o ecoponto e fazer ele ir até o maps
            //else if (result.Equals("Encontrar Ecoponto no Mapa"))
            //{

            //    await RemoverEcoponto(ecoponto);
            //    await Application.Current.MainPage.DisplayAlert("Informação",
            //     "Ecoponto removido com sucesso!", "Ok");
            //    await ObterEcopontos();
            //}
                
        }
    }  
}
