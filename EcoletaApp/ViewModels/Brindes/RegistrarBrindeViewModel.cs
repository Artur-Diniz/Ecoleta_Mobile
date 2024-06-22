using EcoletaApp.Models;
using EcoletaApp.Services.Brindes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EcoletaApp.ViewModels.Brindes
{
    [QueryProperty("BrindeSelecionadoId","bId")]
    public class RegistrarBrindeViewModel : BaseViewModel
    {
        private BrindesServices bService;

        public RegistrarBrindeViewModel()
        { 
            bService = new BrindesServices();

            SalvarCommand = new Command(async ()=> { await RegistrarBrinde(); });
            CancelarCommand = new Command(async () => { await CancelarCadastro(); });
        }

        public ICommand CancelarCommand { get; }
        public ICommand SalvarCommand { get; }


        #region Atributos

        private int idBrinde;
        private string descricaoBrinde;
        private string nomeBrinde;
        private char cadastro; // Char(1);
        private DateTime validade;
        private int quantidade;
        private int valorEcopoints;

        public int IdBrinde { get => idBrinde; set { idBrinde = value; OnPropertyChanged(nameof(IdBrinde)); } }
        public string DescricaoBrinde { get => descricaoBrinde; set { descricaoBrinde = value; OnPropertyChanged(nameof(DescricaoBrinde)); } }
        public string NomeBrinde { get => nomeBrinde; set { nomeBrinde = value; OnPropertyChanged(nameof(NomeBrinde)); } }
        public char Cadastro { get => cadastro; set{  cadastro = value; OnPropertyChanged(nameof(Cadastro)); } }
        public DateTime Validade { get => validade; set { validade = value; OnPropertyChanged(nameof(Validade)); } }
        public int Quantidade { get => quantidade; set {quantidade = value; OnPropertyChanged(nameof(Quantidade)); } }
        public int ValorEcopoints { get => valorEcopoints; set { valorEcopoints = value; OnPropertyChanged(nameof(ValorEcopoints)); } }



        private string brindeSelecionadoId;
        public string BrindeSelecionadoId { set { if (value != null) { brindeSelecionadoId = Uri.UnescapeDataString(value);  CarregarBrinde(); } } }


        #endregion

        public async Task RegistrarBrinde()
        {
            try
            {
                Brinde brinde = new Brinde();
                {
                    IdBrinde = this.IdBrinde;
                    DescricaoBrinde = this.DescricaoBrinde;
                    NomeBrinde = this.NomeBrinde;
                    Cadastro = this.Cadastro;
                    Validade = this.Validade;
                    Quantidade = this.Quantidade;
                    ValorEcopoints = this.ValorEcopoints;
                }

                if (brinde.IdBrinde == 0)
                {
                    await bService.PostBrindeAsync(brinde);
                }
                else
                { 
                    await bService.PutBrindeAsync(brinde);
                }

                await Application.Current.MainPage.DisplayAlert("Mensagem", "Brinde Salvo com Sucesso", "Ok");

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes :" + ex.InnerException, "Ok");
            }
        }

        public async Task CarregarBrinde()
        {
            try
            {
                Brinde b = await bService.GetBrindeIdAsync(Preferences.Get("BrindeId",0));
                
                this.IdBrinde = b.IdBrinde;
                this.NomeBrinde = b.NomeBrinde;
                this.DescricaoBrinde = b.DescricaoBrinde;
                this.Cadastro = b.Cadastro;
                this.Validade = b.Validade;
                this.Quantidade = b.Quantidade;
                this.ValorEcopoints = b.ValorEcopoints;

                

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes :" + ex.InnerException, "Ok");
            }
        }

        public async Task CancelarCadastro()
        {
            await Shell.Current.GoToAsync("..");
        }

    }
}
