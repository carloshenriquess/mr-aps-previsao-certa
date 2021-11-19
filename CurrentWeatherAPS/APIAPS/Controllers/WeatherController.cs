using APIAPS.Repositorio;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace APIAPS.Controllers
{

    public class WeatherController

    {
        public Dados dados = new Dados();
        private readonly RepositorioWeather _repositorioWeather;
        public WeatherController()
        {
            _repositorioWeather = new RepositorioWeather();
        }

        public async Task<JObject> CurrentWeather(string lat, string lon)
        {
            var currentWeather = await _repositorioWeather.CurrentWeather(lat, lon);
            return currentWeather;

        }
        public async Task<JObject> currentWeatherByCityCountry(string city, string country)
        {
            var currentWeather = await _repositorioWeather.currentWeatherByCityCountry(city, country);
            return currentWeather;

        }
        public async Task<JObject> alertWeather(string lat, string lon)
        {
            var currentWeather = await _repositorioWeather.alertWeather(lat, lon);
            return currentWeather;

        }
        public async Task<JObject> alertWeatherByCityCountry(string city, string country)
        {
            var currentWeather = await _repositorioWeather.alertWeatherByCityCountry(city, country);
            return currentWeather;
        }
        public void GerarLog(string texto) {
            _repositorioWeather.GerarLog(texto);
        }
    }
}