using EcoletaApp.Models;
using EcoletaApp.Services.UtilizadorService;
using EcoletaApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecoleta.ViewModels.Utililizador
{
    public  class utilizadorProfileViewModel : BaseViewModel
    {
        private utilizadorService uService;


        public utilizadorProfileViewModel()
        {
            uService = new utilizadorService();

        }

        private Utilizador utilizadorSelecionado;

        public Utilizador UtilizadorSelecionado { get{ return utilizadorSelecionado; } set {if(value != null){ utilizadorSelecionado = value;  _ = ExibirOpcoesAsync(utilizadorSelecionado); } } }


        public async Task ObterUtilizador(int id)
        {
            try
            {
                Utilizador u = await uService.GetUtilizadorAsync(id);
                OnPropertyChanged(nameof(u));

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Informação", ex.Message + "Detalhes:" + ex.InnerException, "OK");
            }
        }

        public async void ProcessarOpcao(Utilizador u, string result)
        {
            if (result.Equals("Editar Utilizador"))
            {
                await Shell.Current
                .GoToAsync($"cadPersonagemView?pId={u.IdUtilizador}");
            }
            else if (result.Equals("Remover Utilizador"))
            {
                await RemoverUtilizador(u); 
            }
            else if (result.Equals("Alterar Email"))
            {
                if (await Application.Current.MainPage.DisplayAlert("Confirmação",
                $"deseja Alterar o email da conta: {u.Nome.ToUpper()}?", "Yes", "No"))
                {
                    await uService.PutAlterarEmailAsync(u);
                    await Application.Current.MainPage.DisplayAlert("Informação",
                     "A Senha foi alterar com Suecesso", "Ok");
                }
            }            
            else if (result.Equals("Alterar Senha"))
            {
                if (await Application.Current.MainPage.DisplayAlert("Confirmação",
                $"deseja Alterar o Senha da conta: {u.Nome.ToUpper()}?", "Yes", "No"))
                {
                    await uService.PutAlterarSenhaAsync(u);
                    await Application.Current.MainPage.DisplayAlert("Informação",
                     "A Senha foi alterar com Suecesso", "Ok");
                }
            }
        }

        public async Task RemoverUtilizador(Utilizador u)
        {
            try
            {
                if (await Application.Current.MainPage.DisplayAlert("Confirmação", $"Confirma a remoção do Utilizador {u.Nome}?", "Sim", "Não"));
                {
                    if (await Application.Current.MainPage.DisplayAlert("Atenção", $"A remoção do Utilizador {u.Nome} é permanente deseja Confirmar?", "Sim", "Não"));
                    {
                    await uService.DeleteUtilizadorAsync(u.IdUtilizador);

                        await Application.Current.MainPage.DisplayAlert("Mensagem", "Utilizador Removido com sucesso Redirecionando", "OK");
                        await Shell.Current.GoToAsync("HomePage");
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + "Detalhes" + ex.InnerException, "OK");
            }
        }

        public async Task ExibirOpcoesAsync(Utilizador u)
        {
            try
            {
                UtilizadorSelecionado = null;
                string result = await Application.Current.MainPage
                    .DisplayActionSheet("Opções para Utilizador" + u.Nome,
                    "Editar Utilizador", "Alterar Email", "Alterar Senha", "Remover Utilizador");

                if (result != null)
                    ProcessarOpcao(u, result);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + "Detalhes" + ex.InnerException, "OK");
            }
        }
    }
}
