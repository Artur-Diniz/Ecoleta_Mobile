﻿using EcoletaApp.Models;
using EcoletaApp.Services.UtilizadorService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EcoletaApp.Views.Utilizador;
using EcoletaApp.Services.Ecopontos;

namespace EcoletaApp.ViewModels.Utililizador
{
    public class utlizadorViewModel : BaseViewModel
    {
        private utilizadorService uService;
        
        public ICommand AutenticarCommand { get; set; }
        public ICommand RegistrarCommand { get; set; }
        public ICommand DirecionarCadastroCommand { get; set; }
        public ICommand DirecionaraParaEcopontoCommand {  get; set; }

        private CancellationToken _cancellationToken;
        private bool _isCheckingLocation;

        public utlizadorViewModel()
        {
            uService = new utilizadorService();

            InicializarCommands();
        }

        public void InicializarCommands()
        { 
            AutenticarCommand = new Command(async  () => await AutenticarUtilizador());
            RegistrarCommand = new Command(async () => await ResgistarUtilizador());
            DirecionarCadastroCommand = new Command(async () => await DirencionarParaCadastro());
            DirecionaraParaEcopontoCommand = new Command(async () => await DirecionaraParaEcoponto());
        }

        #region Atributos Utilizador
        private int idUtilizador = 0;
        private string nome = string.Empty;
        private string email = string.Empty;
        private bool situacaoEmail = false;
        private int totalEcopoints = 0;
        private string username = string.Empty;
        private string passwordstring = string.Empty;


        public int IdUtilizador { get => idUtilizador; set => idUtilizador = value; }
        public string Nome { get => nome; set => nome = value; }
        public string Email { get => email; set => email = value; }
        public bool SituacaoEmail { get => situacaoEmail; set => situacaoEmail = value; }
        public int TotalEcopoints { get => totalEcopoints; set => totalEcopoints = value; }
        public string Username { get { return username; } set { username = value; OnPropertyChanged(nameof(username)); } }
        public string Passwordstring { get { return passwordstring; } set{ passwordstring = value; OnPropertyChanged(nameof(passwordstring)); } }

        #endregion

        public async Task ResgistarUtilizador()
        {
            try
            {
                Utilizador u = new Utilizador();
                u.Username = Username;
                u.PasswordString = Passwordstring;
                u.Email = Email;
                u.SituacaoEmail = true;
                u.Nome = Nome;
                u.TotalEcoPoints = 0;
                u.DataAcesso = DateTime.Now;


                Utilizador uRegisterado = await uService.PostRegistrarUtilizadorAsync(u);

                if (uRegisterado.IdUtilizador != 0)
                {
                    string mensagem = $"Utilizador {uRegisterado.Username} foi registardo com sucesso.";
                    await Application.Current.MainPage.DisplayAlert("Infromação", mensagem, "OK");

                    await Application.Current.MainPage.Navigation.PopAsync();
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Informação", ex.Message + "Detalhes:" + ex.InnerException, "OK");
            }
        }
        
        public async Task AutenticarUtilizador() 
        {
            try
            {
                Utilizador u = new Utilizador();
                u.Email = Email;
                u.PasswordString = Passwordstring;

                Utilizador uAutenticado = await uService.PostAutenticarUtilizadorAsync(u);


                if (uAutenticado != null)
                {
                    _isCheckingLocation = true;
                    _cancellationToken = new CancellationToken();
                    GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                    Location location = await Geolocation.Default.GetLocationAsync(request);

                    Utilizador uLoc = new Utilizador();
                    uLoc.Username = uAutenticado.Username;
                    uLoc.Longitude = location.Longitude;
                    uLoc.Latitude = location.Latitude;

                    utilizadorService utilizadorService = new utilizadorService();
                    await uService.PutAtualizarLocalizacaoAsync(uLoc);




                    string mensagem = $"Bem-vindo(a) {uAutenticado.Username}.";

                    Preferences.Set("IdUtilizador", uAutenticado.IdUtilizador);
                    Preferences.Set("UtilizadorUsername", uAutenticado.Username);
                    Preferences.Set("UtilizadorEmail", uAutenticado.Email);

                    await Application.Current.MainPage.DisplayAlert("Informação", mensagem, "OK");

                    Application.Current.MainPage = new MainPage();
                }
                else
                    throw new Exception("nome de usuário ou Senha Incorreta");

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Informação", ex.Message + "Detalhes:" + ex.InnerException, "OK");
            }
        }

        public async Task DirencionarParaCadastro()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new CadastroView());
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Informação", ex.Message + "Detalhes:" + ex.InnerException, "OK");
            }
        }

        public async Task DirecionaraParaEcoponto()
        {
            try
            {
                await Shell.Current.GoToAsync("LoginEcoponto");                  
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Informação", ex.Message + "Detalhes:" + ex.InnerException, "OK");
            }
        }

     


    }
}
