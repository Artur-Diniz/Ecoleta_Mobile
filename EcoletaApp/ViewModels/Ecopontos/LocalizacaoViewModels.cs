using EcoletaApp.Models;
using EcoletaApp.Services.Ecopontos;
using EcoletaApp.Services.UtilizadorService;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Map = Microsoft.Maui.Controls.Maps.Map;


namespace EcoletaApp.ViewModels.Ecopontos
{
    public class LocalizacaoViewModels : BaseViewModel
    {

        private utilizadorService uService;
        private EcopontoService eService;
        private Map meuMapa;

        public LocalizacaoViewModels( )
        { 
            uService = new utilizadorService();
            eService = new EcopontoService();
        }


        public Map MeuMapa { get => meuMapa; set { if (value != null) { meuMapa = value; OnPropertyChanged(nameof(meuMapa)); }  } }



        public async void InicializarMapa()
        {
            try
            {
                double lat = Preferences.Get("LatitudeEcoponto", 0.0);
                double lon = Preferences.Get("LongitudeEcoponto", 0.0);

                if (lat != 0.0 && lon != 0.0)
                {
                    Location location = new Location(lat, lon);
                    Pin pin = new Pin()
                    {
                        Type = PinType.Place,
                        Label = Preferences.Get("NomeEcoponto", ""),
                        Address = Preferences.Get("EnderecoEcoponto", ""),
                        Location = location
                    };

                    if (MeuMapa != null)
                    {
                        MeuMapa.Pins.Add(pin);
                        MeuMapa.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(0.8)));
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        public async void ExibirUsuariosnoMapa()
        {
            try
            {
                ObservableCollection<Utilizador> ocUtilizador = await uService.GetUtilizadoresAsync();
                List<Utilizador> listUtilizador = new List<Utilizador>(ocUtilizador);

                foreach (Utilizador u in listUtilizador)
                {
                    if (u.Latitude != null && u.Longitude != null)
                    {
                        double latitude = (double)u.Latitude;
                        double longitude = (double)u.Longitude;
                        Location location = new Location(latitude, longitude);

                        Pin pin = new Pin()
                        {
                            Type = PinType.Place,
                            Label = u.Username,
                            Address = $"E-mail: {u.Email}",
                            Location = location
                        };

                        if (MeuMapa != null)
                        {
                            MeuMapa.Pins.Add(pin);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Erro", ex.Message, "OK");
            }
        }
    }
}
