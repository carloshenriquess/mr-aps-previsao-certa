using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace APIAPS.Repositorio
{
    public class RepositorioWeather
    {
        #region "construtores"
        private readonly Dados dados = new Dados();
        private static CookieContainer cookieContainer { get; set; }
        #endregion
        #region "metodos publicos"
        public async Task<JObject> CurrentWeather(string lat, string lon)
        {
            var conexao = dados.url + "current?lat=" + lat + "&lon=" + lon + "&key=" + dados.key;
            GerarLog("Requisicao pra API de clima:\r\n" + conexao);
            var json = await ConexaoGet(conexao);
            GerarLog("Retorno API do clima:\r\n" + json);
            dynamic str = JsonConvert.DeserializeObject(json);
            return str;
        }
        public async Task<JObject> currentWeatherByCityCountry(string city, string country)
        {
            var conexao = dados.url + "current?&city=" + city + "," + country + "&key=" + dados.key;
            GerarLog("Requisicao pra API de clima:\r\n" + conexao);
            var json = await ConexaoGet(conexao);
            GerarLog("Retorno API do clima:\r\n" + json);
            dynamic str = JsonConvert.DeserializeObject(json);
            return str;
        }
        public async Task<JObject> alertWeather(string lat, string lon)
        {
            var conexao = dados.url + "alerts?lat=" + lat + "&lon=" + lon + "&key=" + dados.key;
            GerarLog("Requisicao pra API de clima:\r\n" + conexao);
            var json = await ConexaoGet(conexao);
            GerarLog("Retorno API do clima:\r\n" + json);
            dynamic str = JsonConvert.DeserializeObject(json);
            return str;
        }
        public async Task<JObject> alertWeatherByCityCountry(string city, string country)
        {
            var conexao = dados.url + "alerts?&city=" + city + "," + country + "&key=" + dados.key;
            GerarLog("Requisicao pra API de clima:\r\n" + conexao);
            var json = await ConexaoGet(conexao);
            
            GerarLog("Retorno API do clima:\r\n" + json);
          
            dynamic str = JsonConvert.DeserializeObject(json);
            return str;
        }
        public void GerarLog(string texto)
        {
            DateTime data = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
            var rootPath = AppDomain.CurrentDomain.BaseDirectory;
            string titulo = "Log";
            var sw = File.AppendText(rootPath + "\\" + titulo + ".txt");
            //StreamWriter sw = new StreamWriter(rootPath + "\\" + titulo + ".txt");
            //Write a line of text
            sw.WriteLine(texto + "\r\n" + data.ToString());
            //Close the file
            sw.Close();
        }
        #endregion
        #region "metodos privados"
        private async Task<string> ConexaoGet(String conexao)
        {
            var handler = getHttpHandler();
            HttpClient httpClient = new HttpClient(handler);
            HttpResponseMessage httpResponse = null;

            httpResponse = await httpClient.GetAsync(conexao);
            if (!httpResponse.IsSuccessStatusCode)
            {
                return "Erro de comunicação com o servidor";
            }
            // We assume that if the server responds at all, it responds with valid JSON.
            return await httpResponse.Content.ReadAsStringAsync();
        }
        private async Task<string> ConexaoPost(string conexao)
        {
            HttpContent content = null;
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponse = null;
            httpResponse = await httpClient.PostAsync(conexao, content);
            readCookies(httpResponse, conexao);

            // We assume that if the server responds at all, it responds with valid JSON.
            return await httpResponse.Content.ReadAsStringAsync();
        }
        private void readCookies(HttpResponseMessage response, string url)
        {
            var pageUri = new Uri(url);

            IEnumerable<string> cookies;
            if (response.Headers.TryGetValues("Set-Cookie", out cookies))
            {
                cookieContainer = new CookieContainer();

                foreach (var c in cookies)
                {
                    cookieContainer.SetCookies(pageUri, c);
                }
            }
        }
        private HttpClientHandler getHttpHandler()
        {
            var handler = new HttpClientHandler();

            if (cookieContainer != null)
            {
                handler.CookieContainer = cookieContainer;
            }

            return handler;
        }
        #endregion
    }
}