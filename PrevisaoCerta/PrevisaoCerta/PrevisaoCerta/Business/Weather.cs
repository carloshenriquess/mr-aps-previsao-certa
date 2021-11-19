using Newtonsoft.Json;
using PrevisaoCerta.Model.AlertWeather;
using PrevisaoCerta.Model.CurrentWeather;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PrevisaoCerta.Business
{
    public class Weather
    {
        private string urlBase = "https://currentweatherapi.azurewebsites.net/";
        private string country = "Brazil";

        public async Task<OutCurrentWeather> GetCurrentWeatherByLatLon(string lat, string lon)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"currentWeather?lat={lat}&lon={lon}");

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<OutCurrentWeather>(response.Content.ReadAsStringAsync().Result);

                return null;
            }
        }

        public async Task<OutCurrentWeather> GetCurrentWeatherByCity(string city)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"currentWeatherByCityCountry?city={city}&country={country}");

                if (response.IsSuccessStatusCode)
                {
                    if (response.Content.ReadAsStringAsync().Result == null)
                        throw new Exception("A cidade que você informou é inválida.");

                    return JsonConvert.DeserializeObject<OutCurrentWeather>(response.Content.ReadAsStringAsync().Result);

                }

                return null;
            }
        }

        public async Task<OutAlertWeather> GetAlertWeatherByLatLon(string lat, string lon)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"alertWeather?lat={lat}&lon={lon}");

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<OutAlertWeather>(response.Content.ReadAsStringAsync().Result);

                return null;
            }
        }

        public async Task<OutAlertWeather> GetAlertWeatherByCity(string city)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"alertWeatherByCityCountry?city={city}&country={country}");

                if (response.IsSuccessStatusCode)
                {
                    if (response.Content.ReadAsStringAsync().Result == null)
                        throw new Exception("A cidade que você informou é inválida.");

                    return JsonConvert.DeserializeObject<OutAlertWeather>(response.Content.ReadAsStringAsync().Result);
                }

                return null;
            }
        }
    }
}
