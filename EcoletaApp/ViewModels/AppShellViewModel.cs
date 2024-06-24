using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using EcoletaApp.Models;
using EcoletaApp.Services.Brindes;
using EcoletaApp.Services.UtilizadorService;
using EcoletaApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ecoleta.ViewModels
{
    public class AppShellViewModel : BaseViewModel
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        
    }
}
