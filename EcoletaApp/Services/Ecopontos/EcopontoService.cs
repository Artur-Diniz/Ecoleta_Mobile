using EcoletaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcoletaApp.ViewModels;
using System.Collections.ObjectModel;

namespace EcoletaApp.Services.Ecopontos
{
    public class EcopontoService : Request
    {
        private readonly Request _request;
        private const string apiURLBase = "http://SustenTechDS.somee.com/Ecoleta/api/Ecoponto/";

        public EcopontoService()
        { 
            _request = new Request();
        }


        #region Métodos Get

        public async Task<ObservableCollection<Ecoponto>> GetAllEcopontosAsync()
        {
            string urlComplementar = "GetAll";
            ObservableCollection<Models.Ecoponto> listaEcopontos = await _request
                .GetSemTokenAsync<ObservableCollection<Models.Ecoponto>>(apiURLBase + urlComplementar);

            return listaEcopontos;
        }

        public async Task<Ecoponto> GetEcopontoAsync(int Id)
        {
            string urlComplementar = string.Format("GetbyId/{0}", Id);
            Ecoponto ecoponto = await _request.GetSemTokenAsync<Models.Ecoponto>(apiURLBase + urlComplementar);
            return ecoponto;
        }

        public async Task<Ecoponto> GetForIdFromUsername(string user)
        {
            ObservableCollection<Ecoponto> listEcoponto = await GetAllEcopontosAsync();
            Ecoponto ecoponto = listEcoponto.FirstOrDefault(u => u.Username == user);
            Ecoponto ecoponto1 = await GetEcopontoAsync(ecoponto.IdEcoponto);
            return ecoponto1;
        }

        #endregion



        #region métodos Post

        public async Task<Ecoponto> PostEcopontoAsync(Ecoponto e)
        {
            string urlComplementar = "/Registrar";
                return await _request.PostSemTokenAsync(apiURLBase + urlComplementar, e);            
        }

        public async Task<int> PostRegsistrarEcopontoAsync(Ecoponto e)
        {
            string urlComplementar = "CadastrarEcoponto";
            Ecoponto result = await _request.PostSemTokenAsync<Models.Ecoponto>(apiURLBase + urlComplementar, e);

            return result.IdEcoponto;
        }

        public async Task<bool> PostRegistrarUtilizadorAsync(Ecoponto e)
        {
            //  http://localhost:5268/api/EcoPonto/CadastrarEcoponto?username=artur&passwordString=123456
            
            string urlComplementar = string.Format("CadastrarEcoponto?username={0}&passwordString={1}", e.Username, e.PasswordString);
            bool nulo = new bool();
            bool test = await _request.PostAutenticarUtilizadorAsync(apiURLBase + urlComplementar, nulo);

            return test;
        }

        public async Task<bool> PostAutenticarEcopontoAsync(Ecoponto e)
        {
            //http://localhost:5268/api/EcoPonto/AutenticarEcoponto?username=artur&passwordString=123456

            string urlComplementar = string.Format("AutenticarEcoponto?username={0}&passwordString={1}", e.Username, e.PasswordString);
            bool isSuccessful = await PostAutenticarUtilizadorAsync(apiURLBase + urlComplementar, e);

            return isSuccessful;
        }

        #endregion


        #region Métodos Put

        public async Task<int> PutEcopontoAsync(Ecoponto e)
        {
            string urlComplementar = string.Format("Put/{0}", e.Username);
            var result = await _request.PutSemTokenAsync(apiURLBase +  urlComplementar, e);

           Ecoponto eco = await GetForIdFromUsername(e.Username);

            if (eco != null)
                return eco.IdEcoponto;
            else 
            {
                int mensagem = 0;
                return mensagem;  
            }
        }

        public async Task<int> PutEcopontoAlterarSenhaAsync(Ecoponto e)
        {
            string urlComplementar = string.Format("AlterarSenha/{0}", e.IdEcoponto);
            var result = await _request.PutSemTokenAsync(apiURLBase + urlComplementar, e);
            return result.IdEcoponto;
        }
        public async Task<int> PutEcopontoAlterarEmailAsync(Ecoponto e)
        {
            string urlComplementar = string.Format("AlterarEmail/{0}", e.IdEcoponto);
            var result = await _request.PutSemTokenAsync(apiURLBase + urlComplementar, e);
            return result.IdEcoponto;
        }

        #endregion

        public async Task<int> DeleteEcopontoAsync(int e)
        {
            string urlComplementar = string.Format("Delete/{0}", e);
            var result = await _request.DeleteSemTokenAsync( apiURLBase + urlComplementar);
            return result;
        }

        

      
    }
}
