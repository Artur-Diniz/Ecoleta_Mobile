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

       

        public AppShellViewModel()
        { 
            bService = new BrindesServices();

           
        }
    }
}
