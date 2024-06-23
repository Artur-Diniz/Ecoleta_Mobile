using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcoletaApp.Models;
using Microsoft.Maui.ApplicationModel.Communication;

namespace EcoletaApp.Services.UtilizadorService
{
    public class utilizadorService : Request
    {
        private readonly Request _request;
        private const string ApiUrlBase = "http://SustenTechDS.somee.com/Ecoleta/api/Utilizador/";

        public utilizadorService()
        {
            _request = new Request();
        }

        #region métodos Get
        public async Task<ObservableCollection<Utilizador>> GetUtilizadoresAsync()
        {
            string urlComplementar = "GetAll";
            ObservableCollection<Models.Utilizador> listaUsuarios = await
            _request.GetSemTokenAsync<ObservableCollection<Models.Utilizador>>(ApiUrlBase + urlComplementar);
            return listaUsuarios;
        }

        public async Task<Utilizador> GetUtilizadorAsync(int uId)
        {
            string urlComplementar = string.Format("GetbyId/{0}", uId);
            Utilizador u = await _request.GetSemTokenAsync<Models.Utilizador>(ApiUrlBase + urlComplementar);

            return u;
        }
        #endregion

        #region métodos Post
        public async Task<Utilizador> PostUtilizadorAsync(Utilizador u)
        {
            string urlComplementar = "Post";
            u.IdUtilizador = await _request.PostReturnIntAsync(ApiUrlBase + urlComplementar, u);

            return u;
        }
        public async Task<Utilizador> PostRegistrarUtilizadorAsync(Utilizador u)
        {
            //  'http://localhost:5268/api/Utilizador/Registrar?username=caio&passwordString=123456'
            string urlComplementar = string.Format("Registrar?username={0}&passwordString={1}",u.Username, u.PasswordString );
            u.IdUtilizador = await _request.PostReturnIntAsync(ApiUrlBase + urlComplementar, u);

            return u;
        }

        public async Task<bool> PostAutenticarUtilizadorAsync(Utilizador u)
        {
            // http://SustenTechDS.somee.com/Ecoleta/api/Utilizador/Autenticar?username=artur123&passwordString=123456
            string urlComplementar = string.Format("Autenticar?username={0}&passwordString={1}", u.Username, u.PasswordString);
            bool isSuccessful = await PostAutenticarUtilizadorAsync(ApiUrlBase + urlComplementar, u);

            return isSuccessful;
        }

        public async Task<string> PostbrindeAsync(int uId, int bId)
        {
            // http://localhost:5268/api/Utilizador/ResgatarBrinde/1/1
            string urlComplementar = string.Format("ResgatarBrinde/{0}/{1}", uId, bId);
            int[] meuArray = null;

            string isSuccessful = await PostReturnstringAsync<int[]>(ApiUrlBase + urlComplementar, meuArray);

            return isSuccessful;
        }

        #endregion

        #region Métodos Put
        public async Task<int> PutUtilizadorAsync(Utilizador u)
        {
            int id = u.IdUtilizador;
            string urlComplementar = string.Format("Put/{0}", id);
            var result = await _request.PutSemTokenAsync(ApiUrlBase + urlComplementar, u);
            return result.IdUtilizador;
        }

        public async Task<int> PutAlterarSenhaAsync(Utilizador u)
        {
            string urlComplementar = "/AlterarSenha";
            var result = await _request.PutSemTokenAsync(ApiUrlBase + urlComplementar, u);
            return result.IdUtilizador;
        }
        public async Task<int> PutAlterarEmailAsync(Utilizador u)
        {
            string urlComplementar = "/AtualizarEmail";
            var result = await _request.PutSemTokenAsync(ApiUrlBase + urlComplementar, u);
            return result.IdUtilizador;
        }

        public async Task<int> PutAtualizarLocalizacaoAsync(Utilizador u)
        {
            string urlComplementar = "/AtualizarLocalizacao";
            var result = await _request.PutSemTokenAsync(ApiUrlBase + urlComplementar, u);
            return result.IdUtilizador;
        }

        #endregion


        public async Task<int> DeleteUtilizadorAsync(int id)
        {
            string urlComplementar = string.Format("Delete/{0}", id);
            var result = await _request.DeleteSemTokenAsync(ApiUrlBase + urlComplementar);

            return id;
        }

        public async Task<Utilizador> GetForIdFromUsername(string user)
        {
            ObservableCollection<Utilizador> listaUsuarios = await GetUtilizadoresAsync();
            Utilizador utilizador = listaUsuarios.FirstOrDefault(u => u.Username == user);
            Utilizador user1 = await GetUtilizadorAsync(utilizador.IdUtilizador);
            return user1;
        }

    }
}
