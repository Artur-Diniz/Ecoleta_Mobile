using EcoletaApp.Models;
using EcoletaApp.Services.UtilizadorService;
using EcoletaApp.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Ecoleta.ViewModels.Utililizador
{
    public class UtilizadorProfileViewModel : BaseViewModel
    {
        private readonly utilizadorService uService;
        private int id = Preferences.Get("IdUtilizador", 0);

        public UtilizadorProfileViewModel()
        {
            uService = new utilizadorService();
            Utilizadores = new ObservableCollection<Utilizador>();
            _ = ObterUtilizador();
        }

        public ObservableCollection<Utilizador> Utilizadores { get; set; }

        private Utilizador utilizadorSelecionado;
        public Utilizador UtilizadorSelecionado
        {
            get { return utilizadorSelecionado; }
            set
            {
                if (value != null)
                {
                    utilizadorSelecionado = value;
                    OnPropertyChanged(nameof(UtilizadorSelecionado));
                    _ = ExibirOpcoesAsync(utilizadorSelecionado);
                }
            }
        }

        public async Task ObterUtilizador()
        {
            try
            {
                Utilizador u = await uService.GetUtilizadorAsync(id);
                Utilizadores.Clear();
                Utilizadores.Add(u);
                OnPropertyChanged(nameof(Utilizadores));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Informação", ex.Message + " Detalhes: " + ex.InnerException, "OK");
            }
        }

        public async void ProcessarOpcao(Utilizador u, string result)
        {
            switch (result)
            {
                case "Atualizar Utilizador":
                    await ObterUtilizador();
                    break;
                case "Remover Utilizador":
                    await RemoverUtilizador(u);
                    break;
              
            }
        }

        public async Task RemoverUtilizador(Utilizador u)
        {
            try
            {
                if (await Application.Current.MainPage.DisplayAlert("Confirmação", $"Confirma a remoção do Utilizador {u.Nome}?", "Sim", "Não"))
                {
                    if (await Application.Current.MainPage.DisplayAlert("Atenção", $"A remoção do Utilizador {u.Nome} é permanente. Deseja confirmar?", "Sim", "Não"))
                    {
                        await uService.DeleteUtilizadorAsync(u.IdUtilizador);
                        await Application.Current.MainPage.DisplayAlert("Mensagem", "Utilizador Removido com sucesso. Redirecionando.", "OK");
                        await Shell.Current.GoToAsync("HomePage");
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "OK");
            }
        }

        public async Task ExibirOpcoesAsync(Utilizador u)
        {
            try
            {
                UtilizadorSelecionado = null;
                string result = await Application.Current.MainPage.DisplayActionSheet("Opções para Utilizador " + u.Nome, "Cancelar", null,
                   "Atualizar Utilizador", "Remover Utilizador");
                if (result != null)
                {
                    ProcessarOpcao(u, result);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "OK");
            }
        }
    }
}
