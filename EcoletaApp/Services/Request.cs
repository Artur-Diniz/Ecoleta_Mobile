﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EcoletaApp.Services
{
    public class Request
    {
        public async Task<int> PostReturnIntAsync<TResult>(string uri, TResult data)
        {
            HttpClient httpClient = new HttpClient();

            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PostAsync(uri, content);
            
            
            string serialized = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.Created || response.StatusCode == System.Net.HttpStatusCode.OK)
                return int.Parse(serialized);
            else
                throw new Exception(serialized);
        }

        public async Task<TResult> PostSemTokenAsync<TResult>(string uri, TResult data)
        {
            HttpClient httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PostAsync(uri, content);
            string serialized = await response.Content.ReadAsStringAsync();
            TResult result = data;




            if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Created)
                result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized));

            return result;
        }
        public async Task<TResult> PutSemTokenAsync<TResult>(string uri, TResult data)
        {
            HttpClient httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PutAsync(uri, content);
            var serialized = await response.Content.ReadAsStringAsync();
            TResult result = data;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized));
                return result;
                 }
            else
                throw new Exception(serialized);
        }

        public async Task<bool> PostAutenticarUtilizadorAsync(string uri, object data)
        {
            HttpClient httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PostAsync(uri, content);

            if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public async Task<int> PutAsync<TResult>(string uri, TResult data, string token)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization
            = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PutAsync(uri, content);
            string serialized = await response.Content.ReadAsStringAsync();
           
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return int.Parse(serialized);
            else
                throw new Exception(serialized);
        }

        public async Task<TResult> GetSemTokenAsync<TResult>(string uri)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(uri);

            var serialized = await response.Content.ReadAsStringAsync();
            TResult result;
            result = JsonConvert.DeserializeObject<TResult>(serialized);

            if (serialized.Contains("\"value\""))
            {
                var wrapper = JsonConvert.DeserializeObject<Wrapper<TResult>>(serialized);
                result = wrapper.Value;
            }

            if (response.IsSuccessStatusCode)
            {
                return result;
            }
            else
            {
                throw new Exception(serialized);
            }
        }

        public async Task<TResult> GetAsync<TResult>(string uri, string token)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization
            = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            string serialized = await response.Content.ReadAsStringAsync();
            TResult result = await Task.Run(() =>
            JsonConvert.DeserializeObject<TResult>(serialized));
            throw new Exception(serialized);
        }


        public async Task<int> DeleteAsync(string uri, string token)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new
           AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await httpClient.DeleteAsync(uri);
            string serialized = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return int.Parse(serialized);
            else
                throw new Exception(serialized);
        }

        public async Task<int> DeleteSemTokenAsync(string uri)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.DeleteAsync(uri);
            string serialized = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return int.Parse(serialized);
            else
                throw new Exception(serialized);
        }

        public class Wrapper<T>
        {
            public T Value { get; set; }
        }
    }
}
