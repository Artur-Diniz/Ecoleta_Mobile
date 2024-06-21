using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using EcoletaApp.Services.Brindes;
using EcoletaApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecoleta.ViewModels
{
    public class AppShellViewModel : BaseViewModel
    {
        private BrindesServices bService;
        private BrindesServices _service;
        private static string conexaoAzureStorage = "";
        private static string container = "sustentech";

        public AppShellViewModel()
        { 
            bService = new BrindesServices();

            CarregarUsuarioAzure();
        }

        #region Atributos

        private byte[] foto;

        public byte[] Foto { get => foto; set { foto = value; OnPropertyChanged(nameof(foto)); } }

        #endregion

        public async void CarregarUsuarioAzure()
        {
            try
            {
                int brindeId = Preferences.Get("BrindeId", 0);
                string filename = $"{brindeId}.jpg";

                var blobClient = new BlobClient(conexaoAzureStorage, container, filename);
                Byte[] fileBytes;

                using (MemoryStream ms = new MemoryStream())
                {
                    blobClient.OpenRead().CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
                Foto = fileBytes;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes :" + ex.InnerException, "Ok");
            }
        }
    }
}
