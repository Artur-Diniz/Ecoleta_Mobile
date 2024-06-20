using Azure.Storage.Blobs;
using EcoletaApp.Models;
using EcoletaApp.Services.Brindes;
using EcoletaApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ecoleta.ViewModels.Brindes
{
    public class ImageBrindeViewModel : BaseViewModel
    {
        private BrindesServices _service;
        private static string conexaoAzureStorage = "";
        private static string container = "";

        public ImageBrindeViewModel()
        { 
        _service= new BrindesServices();

            FotografarCommand = new Command(Fotografar);
            SalvarCommand = new Command(SalvarImagem);
        }

        public ICommand FotografarCommand { get; }
        public ICommand SalvarCommand { get; }

        #region Image 
        private ImageSource fonteImage;

        public ImageSource FonteImage { get { return fonteImage; } set { fonteImage = value; OnPropertyChanged(nameof(FonteImage)); } }

        private byte[] foto;

        public byte[] Foto { get { return foto; } set { foto = value; OnPropertyChanged(nameof(Foto)); } }


        #endregion

        public async void Fotografar()
        {
            try
            {
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                    if (photo != null) 
                    {
                        using (Stream sourceStream = await photo.OpenReadAsync())
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                await sourceStream.CopyToAsync(ms);

                                Foto =ms.ToArray();

                                Foto = ms.ToArray();

                                FonteImage = ImageSource.FromStream(()=> new MemoryStream(ms.ToArray()));
                            }
                        }   
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes :" + ex.InnerException, "Ok");
            }
        }

        public async void SalvarImagem()
        {
            try
            {

                Brinde b = new Brinde();
                b.Imagem = Foto;
                b.IdBrinde = Preferences.Get("BrindeId", 0);

                string fileName = $"{b.IdBrinde}.jpg";

                var blobClient = new BlobClient(conexaoAzureStorage, container, fileName);

                if (blobClient.Exists())
                    blobClient.Delete();

                using (var stream = new MemoryStream(b.Imagem))
                { 
                        blobClient.Upload(stream);
                }

                await Application.Current.MainPage.DisplayAlert("Mensagem", "Dados salvos com sucesso", "ok");
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes :" + ex.InnerException, "Ok");
            }
        }
    }
}
