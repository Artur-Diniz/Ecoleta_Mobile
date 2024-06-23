using EcoletaApp.Services.Coletas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcoletaApp.Models;
using System.Windows.Input;
using Ecoleta.Models.Enuns;
using Ecoleta.Models;

namespace EcoletaApp.ViewModels.Coletas
{
    [QueryProperty("ColetaSelencionadaId", "cId")]
    public class RegistrarColetaViewModel : BaseViewModel
    {
        private ColetaService cService;

        public ICommand RegistrarCommand { get; }
        public ICommand CancelarCommand { get; }

        public RegistrarColetaViewModel()
        {
            cService = new ColetaService();

            

            RegistrarCommand = new Command(async () => { await RegistrarColetaAsync(); });
            CancelarCommand = new Command(async () => { await CancelarCadastro(); });

            _ = ObterClasses();
        }


        #region Atributos

        private int idColeta;
        private int idEcoponto;
        private int idUtilizador;
        private DateTime dataColeta;
        private float totalEcopoints;
        private Double peso;
        private string situacaoColeta;
        private MateriaisEnuns classe;

        public int IdColeta { get => idColeta; set { idColeta = value; OnPropertyChanged(nameof(IdColeta)); } }
        public int IdEcoponto { get => idEcoponto; set { idEcoponto = value; OnPropertyChanged(nameof(IdEcoponto)); } }
        public int IdUtilizador { get => idUtilizador; set { idUtilizador = value; OnPropertyChanged(nameof(IdUtilizador)); } }
        public DateTime DataColeta { get => dataColeta; set { dataColeta = value; OnPropertyChanged(nameof(DataColeta)); } }
        public float TotalEcopoints { get => totalEcopoints; set { totalEcopoints = value; OnPropertyChanged(nameof(TotalEcopoints)); } }
        public double Peso { get => peso; set { peso = value; OnPropertyChanged(nameof(Peso)); } }
        public string SituacaoColeta { get => situacaoColeta; set{ situacaoColeta = value; OnPropertyChanged(nameof(SituacaoColeta)); } }
        public MateriaisEnuns Classe { get => classe; set { classe = value; OnPropertyChanged(nameof(Classe)); } }


        private ObservableCollection<Materiais> tipoClasse;
        public ObservableCollection<Materiais> TipoClasse { get => tipoClasse; set { tipoClasse = value; OnPropertyChanged(nameof(TipoClasse)); } }

        private Materiais tipoClasseSelecionando;
        public Materiais TipoClasseSelecionando { get => tipoClasseSelecionando; set { tipoClasseSelecionando = value; OnPropertyChanged(nameof(TipoClasseSelecionando));  } }


        private string coletaSelecionadaId;

        public string ColetaSelencionadaId { set { if (value != null) { coletaSelecionadaId = Uri.UnescapeDataString(value); CarregarColetaAsync(); } } }




        #endregion

        public async Task RegistrarColetaAsync()
        {
            try
            {
                Coleta coleta = new Coleta
                {
                    IdColeta = this.IdColeta,
                    IdEcoponto = this.IdEcoponto,
                    IdUtilizador = this.IdUtilizador,
                    DataColeta = DateTime.Now,
                    Peso = this.Peso,
                    SituacaoColeta = this.SituacaoColeta,

                };
                coleta.Classe = (MateriaisEnuns)TipoClasseSelecionando.IdMaterial;

                if (coleta != null)
                {
                    if (coleta.IdColeta == 0)
                        await cService.PostColetaIndIdAsync(coleta);
                    else
                        await cService.putColeta(coleta);

                    if (coleta.SituacaoColeta == "Concluida")
                    { await cService.ConfirmarColetaAsync(coleta); }

                    await Shell.Current.GoToAsync("..");

                    await Application.Current.MainPage.DisplayAlert("Mensagem", "Dados Salvos Com Sucesso!", "OK");
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Ops", "não é possivél enviar todos os campos nulos", "Ok");

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes :" + ex.InnerException, "Ok"); 
                await Shell.Current.GoToAsync("..");

            }
        }

        public async void CarregarColetaAsync()
        {
            try
            {
                Coleta c = await cService.GetColetaAsync(int.Parse(coletaSelecionadaId));

                this.IdColeta = c.IdColeta;
                this.IdEcoponto = c.IdEcoponto;
                this.IdUtilizador = c.IdUtilizador; 
                this.DataColeta = c.DataColeta;
                this.Peso = c.Peso;
                this.SituacaoColeta = c.SituacaoColeta;
                this.Classe = c.Classe;

                TipoClasseSelecionando = this.TipoClasse.FirstOrDefault(tc => tc.IdMaterial == (int)c.Classe);

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

        public async Task ObterClasses()
        {
            try
            {
                TipoClasse = new ObservableCollection<Materiais>();
                TipoClasse.Add(new Materiais() { IdMaterial = 1, DescricaoMaterial = "Papel", MateriaisEnuns = MateriaisEnuns.Papel });
                TipoClasse.Add(new Materiais() { IdMaterial = 2, DescricaoMaterial = "Plastico", MateriaisEnuns = MateriaisEnuns.Plastico });
                TipoClasse.Add(new Materiais() { IdMaterial = 3, DescricaoMaterial = "Vidro", MateriaisEnuns = MateriaisEnuns.Vidro });
                TipoClasse.Add(new Materiais() { IdMaterial = 4, DescricaoMaterial = "Metal", MateriaisEnuns = MateriaisEnuns.Metal });
                TipoClasse.Add(new Materiais() { IdMaterial = 5, DescricaoMaterial = "Organico", MateriaisEnuns = MateriaisEnuns.Organico });
                TipoClasse.Add(new Materiais() { IdMaterial = 6, DescricaoMaterial = "Eletronico", MateriaisEnuns = MateriaisEnuns.Eletronico });
                TipoClasse.Add(new Materiais() { IdMaterial = 7, DescricaoMaterial = "Pilha", MateriaisEnuns = MateriaisEnuns.Pilha });
                TipoClasse.Add(new Materiais() { IdMaterial = 8, DescricaoMaterial = "Bateria", MateriaisEnuns = MateriaisEnuns.Bateria });
                TipoClasse.Add(new Materiais() { IdMaterial = 9, DescricaoMaterial = "Oleo", MateriaisEnuns = MateriaisEnuns.Oleo });
                TipoClasse.Add(new Materiais() { IdMaterial = 10, DescricaoMaterial = "Medicamento", MateriaisEnuns = MateriaisEnuns.Medicamento });
                TipoClasse.Add(new Materiais() { IdMaterial = 11, DescricaoMaterial = "Outros", MateriaisEnuns = MateriaisEnuns.Outro });
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes :" + ex.InnerException, "Ok");
            }
        }
    }
}
