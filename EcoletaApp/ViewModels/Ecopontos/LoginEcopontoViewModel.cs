using EcoletaApp;
using EcoletaApp.Models;
using EcoletaApp.Services.Ecopontos;
using EcoletaApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ecoleta.ViewModels.Ecopontos
{
    public class LoginEcopontoViewModel : BaseViewModel
    {
        private EcopontoService eService;
        public ICommand AutenticarCommand { get;  set; }
        public ICommand RegistarCommand { get; set; }


        public LoginEcopontoViewModel()
        {
            eService = new EcopontoService();
            InicializarCommannds();
        }

        public void InicializarCommannds()
        {
            AutenticarCommand = new Command(async()=> await AutenticarEcopontoUtilizador());
            RegistarCommand = new Command(async () => await RedirecionarCadastroEcoponto());
        }

        #region Atributos

        private int idEcoponto;
        private string username = string.Empty;
        private string passwordString = string.Empty;
        private string email  = string.Empty;


        public int IdEcoponto { get { return idEcoponto; } set { idEcoponto = value; } }
        public string Username { get { return username; } set { username = value; } }
        public string PasswordString { get { return passwordString; } set { passwordString = value; } }
        public string Email { get { return email; } set { email = value; } }
   
        #endregion

        public async Task RedirecionarCadastroEcoponto()
        {
            try
            {
                await Shell.Current.GoToAsync("cadEcopontoView");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Informação:", ex.Message + " Detalhes: " + ex.InnerException, "OK");
            }
        }

        public async Task AutenticarEcopontoUtilizador()
        {
            try
            {
                Ecoponto e = new Ecoponto();
                e.Username = username;
                e.PasswordString = passwordString;
                e.Email = email;
                e.PasswordHash = null;
                e.PasswordSalt = null;
       

                int eAutenticadoId = await eService.PostAutenticarEcopontoAsync(e);
                Ecoponto eAutenticado = await eService.GetEcopontoAsync(eAutenticadoId);

                if (eAutenticado.PasswordSalt != null || eAutenticado.PasswordHash != null)
                { 
                    string mensagem = $"bem-Vindo(a) {eAutenticado.Username}.";

                    Preferences.Set("utilizadorEcopontoId", eAutenticado.IdEcoponto);
                    Preferences.Set("utilizadorUsernameEcoponto", eAutenticado.Username);
                    Preferences.Set("utilizadorEmailEcoponto", eAutenticado.Email);

                    await Application.Current.MainPage.DisplayAlert("Informação", mensagem, "Ok");

                     Application.Current.MainPage = new AppShell();
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Informação:", ex.Message + " Detalhes: " + ex.InnerException, "OK");
            }
        }
    }
}
