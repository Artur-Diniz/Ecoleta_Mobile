using Ecoleta.Models;
using EcoletaApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecoleta.Services.Resgates
{
    public class ResgateService : Request
    {
        
        private readonly Request _request;
        private const string apiURLBase = "http://SustenTechDS.somee.com/Ecoleta/api/Resgates/";
        private const string UrlBrinde = "http://SustenTechDS.somee.com/Ecoleta/api/brinde/";


        public ResgateService()
        {
            _request = new Request();
        }

        public async Task<Resgate> PostResgateAsync(Resgate resgate)
        {
            string urlComplementar = "Post";
            return await _request.PostSemTokenAsync(apiURLBase + urlComplementar, resgate);
        }

        public async Task<Resgate> PutResgateAsync(Resgate resgate)
        {
            string urlComplementar = string.Format("Put/{0}", resgate);
            return await _request.PutSemTokenAsync(apiURLBase + urlComplementar, resgate);
        }

        public async Task<ObservableCollection<Resgate>> GetAllAsync(int id)
        {
            //http://localhost:5268/api/Resgate/GetAll?IdUtilizador=1
            string urlComplmentar = string.Format("GetAll?IdUtilizado={0}", id);
            return await _request.GetSemTokenAsync<ObservableCollection<Resgate>>(apiURLBase + urlComplmentar);
        }

        public async Task<Resgate> GetIdAsync(int id)
        {
            string urlcomplementar = string.Format("GetById/{0}", id );
            return await _request.GetSemTokenAsync<Resgate>(apiURLBase + urlcomplementar);
        }

        public async Task<int> DeleteAsync(int id)
        {
            string urlComplementar = string.Format("Delete/{0}", id);
            return await _request.DeleteSemTokenAsync(apiURLBase +urlComplementar);

        }
    }
}
