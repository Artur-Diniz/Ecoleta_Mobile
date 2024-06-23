using EcoletaApp.Models;
using EcoletaApp.Services.Coletas;
using EcoletaApp.ViewModels.Coletas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EcoletaApp.ViewModels.Coletas
{
    class ExibirColetasViewModel : BaseViewModel
    {
        private ColetaService cService;

        public ObservableCollection<Coleta> Coletas { get; set; }
        public ICommand RegistrarCommand { get; }
        public ICommand RemoverColetaCommand { get; }



        public ExibirColetasViewModel()
        {
            cService = new ColetaService();
            Coletas = new ObservableCollection<Coleta>();

            _ = ObterColetas();

            RegistrarCommand = new Command(async () => { await ExibirTelaRegistro(); });
            RemoverColetaCommand = new Command<Coleta>(async (Coleta c) => { await RemoverColeta(c); });
        }


        #region Atributos


        private int idColeta;
        private int idEcoponto;
        private int idUtilizador;
        private int codigoUtilizador;
        private int codigoEcoponto;
        private DateTime dataColeta;
        private float totalEcopoints;
        private Double peso;
        private string situacaoColeta;

        public int IdColeta { get => idColeta; set { idColeta = value; OnPropertyChanged(nameof(IdColeta)); } }
        public int IdEcoponto { get => idEcoponto; set { idEcoponto = value; OnPropertyChanged(nameof(IdEcoponto)); } }
        public int IdUtilizador { get => idUtilizador; set { idUtilizador = value; OnPropertyChanged(nameof(IdUtilizador)); } }
        public int CodigoUtilizador { get => codigoUtilizador; set { codigoUtilizador = value; OnPropertyChanged(nameof(CodigoUtilizador)); } }
        public int CodigoEcoponto { get => codigoEcoponto; set { codigoEcoponto = value; OnPropertyChanged(nameof(CodigoEcoponto)); } }
        public DateTime DataColeta { get => dataColeta; set { dataColeta = value; OnPropertyChanged(nameof(DataColeta)); } }
        public float TotalEcopoints { get => totalEcopoints; set { totalEcopoints = value; OnPropertyChanged(nameof(TotalEcopoints)); } }
        public double Peso { get => peso; set { peso = value; OnPropertyChanged(nameof(Peso)); } }
        public string SituacaoColeta { get => situacaoColeta; set { situacaoColeta = value; OnPropertyChanged(nameof(SituacaoColeta)); } }

        private int coletaid;
        private Coleta coletaSelecionada;
        public Coleta ColetaSelecionada
        {
            get { return coletaSelecionada; }
            set
            {
                if (value != null)
                {
                    coletaSelecionada = value;
                    Preferences.Set("ColetaId", coletaSelecionada.IdColeta);



                    _ = ExibirOpcoes(coletaSelecionada);
                }
            }
        }

        #endregion

        public async Task ObterColetas()
        {
            try
            {
                Coletas = await cService.GetColetaAllAsync();
                OnPropertyChanged(nameof(Coletas));
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
                await Shell.Current.GoToAsync("cadColetaView");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes :" + ex.InnerException, "Ok");
            }
        }

        public async Task RemoverColeta(Coleta c)
        {
            try
            {
                if (await Application.Current.MainPage.DisplayAlert("Confirmação", $"Confirmação a remoção da coleta com Id: {c.IdColeta}?", "Sim", "Não"));
                {
                    await cService.DeleteColetaAsync(c.IdColeta);

                    await ObterColetas();

                    await Application.Current.MainPage.DisplayAlert("Mensagem", "Coleta Removida Com sucesso!", "OK");

                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes :" + ex.InnerException, "Ok");
            }
        }

        public async Task StatusColeta(Coleta c)
        {
            try
            {
                coletaSelecionada = null;
                string result = string.Empty;

                result = await Application.Current.MainPage
                    .DisplayActionSheet("Alterar Status da Coleta para:  ",
                    "Cancelar",
                    "Invalida",
                    "Pendente",
                    "Concluida"
                    );

                if(result != "Cancelar")
                AlerarStatusColeta(result);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes :" + ex.InnerException, "Ok");
            }
        }


        public async Task AlerarStatusColeta(string result)
        {
           
               Coleta c = await cService.GetColetaAsync(Preferences.Get("ColetaId",0));
                c.SituacaoColeta = result;
                c.IdColeta = c.IdColeta;
                c.IdEcoponto = c.IdEcoponto;
                c.IdUtilizador = c.IdUtilizador;
                c.DataColeta = c.DataColeta;
                c.Peso = c.Peso;
                c.SituacaoColeta = c.SituacaoColeta;

                await cService.putColeta(c);

                   await Application.Current.MainPage.DisplayAlert("Mensagem", "Status Alterado com sucesso!", "OK");

            await ObterColetas();


        }



        public async Task ExibirOpcoes(Coleta c)
        {
            try
            {
                coletaSelecionada = null;
                string result = string.Empty;

                result = await Application.Current.MainPage
                    .DisplayActionSheet("Opções para Coleta:  " ,    
                    "Cancelar",
                    "Editar Coleta",
                    "Remover Coleta",
                    "Alterar Status da Coleta"
                    );

                if (result != null)
                    ProcessarOpcaoAsync(c, result);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + "Detalhes" + ex.InnerException, "OK");
            }
        }

        public async void ProcessarOpcaoAsync(Coleta coleta, string result)
        {

            if (result.Equals("Editar Coleta"))
            {
                await Shell.Current
                    .GoToAsync($"cadColetaView?cId={coleta.IdColeta}");
                await ObterColetas();

            }
            else if (result.Equals("Remover Coleta"))
            {
                await RemoverColeta(coleta);
                await ObterColetas();

            }
            else if (result.Equals("Alterar Status da Coleta"))
            {
                await StatusColeta(coletaSelecionada);
                await ObterColetas();
            }
        }

    }
}
