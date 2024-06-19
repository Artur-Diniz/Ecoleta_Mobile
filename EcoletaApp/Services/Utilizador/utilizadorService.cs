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
            string urlComplementar = "Registrar";
            u.IdUtilizador = await _request.PostReturnIntAsync(ApiUrlBase + urlComplementar, u);

            return u;
        }

        public async Task<Utilizador> PostAutenticarUtilizadorAsync(Utilizador u)
        {
            string urlComplementar = "Autenticar";
            u = await _request.PostSemTokenAsync(ApiUrlBase + urlComplementar, u);

            return u;
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


    }
}
