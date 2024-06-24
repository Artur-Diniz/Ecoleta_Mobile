using Ecoleta.Models;
using Ecoleta.Services.Resgates;
using EcoletaApp.Services.Brindes;
using EcoletaApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ecoleta.ViewModels.Brindes
{
    public class ResgatesBrindes : BaseViewModel
    {
        private ResgateService rService;

        public ResgatesBrindes()
        {
            rService = new ResgateService();

            _=  ObterResgastesBrindes();

            BrindesCommand = new Command(async () => { await MeuBrinde(); });
        }

        public ICommand BrindesCommand { get; }
        public ObservableCollection<Resgate> Resgates { get; set; }



        public async Task ObterResgastesBrindes()
        {
            try
            {
                int id = Preferences.Get("IdUtilizador", 0);
                if (id != 0)
                    Resgates = await rService.GetAllAsync(id);
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Informação", "Você ainda Não resgatou Brinde Va " +
                        "para aba de Brinde caso queira resgatar seu proximo Brinde", "OK");
                }

                OnPropertyChanged(nameof(Brindes));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes :" + ex.InnerException, "Ok");
            }
        }

        public async Task MeuBrinde()
        {
            try
            {
                await Shell.Current.GoToAsync("Brindes");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes :" + ex.InnerException, "Ok");
            }
        }


    }
}
