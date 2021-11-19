using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace APIAPS.Controllers
{
    public class homeController : ApiController
    {
        #region "Construtor"
        private readonly WeatherController weatherController = new WeatherController();
        #endregion

        #region "Metodos publicos"
        [Route("currentWeather")]
        [HttpGet]
        public async Task<JObject> currentWeather(string lat, string lon)
        {

            weatherController.GerarLog("Requisicao do App:\r\n" + weatherController.dados.nossaApi + "/currentWeather?lat=" + lat + "&lon=" + lon);
            var result = await weatherController.CurrentWeather(lat, lon);
            weatherController.GerarLog("Retorno pro App:\r\n" + result);
            return result;
        }
        [Route("currentWeatherByCityCountry")]
        [HttpGet]
        public async Task<JObject> currentWeatherByCityCountry(string city, string country)
        {
            weatherController.GerarLog("Requisicao do App:\r\n" + weatherController.dados.nossaApi + "/currentWeatherByCityCountry?city=" + city + "&country=" + country);
            var result = await weatherController.currentWeatherByCityCountry(city, country);
            weatherController.GerarLog("Retorno pro App:\r\n" + result);
            return result;
        }
        [Route("alertWeather")]
        [HttpGet]
        public async Task<JObject> alertWeather(string lat, string lon)
        {
            weatherController.GerarLog("Requisicao do App:\r\n" + weatherController.dados.nossaApi + "/alertWeather?lat=" + lat + "&lon=" + lon);
            var result = await weatherController.alertWeather(lat, lon);
            weatherController.GerarLog("Retorno pro App:\r\n" + result);
            return result;
        }
        [Route("alertWeatherByCityCountry")]
        [HttpGet]
        public async Task<JObject> alertWeatherByCityCountry(string city, string country)
        {
            weatherController.GerarLog("Requisicao do App:\r\n" + weatherController.dados.nossaApi + "/alertWeatherByCityCountry?city=" + city + "&country=" + country);
            var result = await weatherController.alertWeatherByCityCountry(city, country);
            //var sim = "{\"country_code\":\"BR\",\"lon\":-43.31167,\"timezone\":\"America Sao_Paulo\",\"lat\":-22.78556,\"alerts\":[{\"regions\":[\"Madison\",\" Franklin\",\" Scioto\",\" Hardin\",\" Logan\",\" Licking\",\" Montgomery\",\" Clermont\",\" Fayette\",\" Brown\",\" Mercer\",\" Wayne\",\" Adams\",\" Union\",\" Warren\",\" Highland\",\" Lewis\",\" Ross\",\" Robertson\",\" Clark\",\" Fayette\",\" Preble\",\" Shelby\",\" Clinton\",\" Franklin\",\" Pike\",\" Miami\",\" Delaware\",\" Bracken\",\" Greene\",\" Auglaize\",\" Hocking\",\" Fairfield\",\" Butler\",\" Champaign\",\" Pickaway\",\" Darke\",\" Mason\",\" Union\"],\"ends_utc\":\"2020-10-17T13:00:00\",\"effective_local\":\"2020-10-16T14:17:00\",\"onset_utc\":\"2020-10-17T06:00:00\",\"expires_local\":\"2020-10-16T22:30:00\",\"expires_utc\":\"2020-10-17T02:30:00\",\"ends_local\":\"2020-10-17T09:00:00\",\"uri\":\"httpsapi.weather.gov/alerts/NWS-IDP-PROD-4486346-3727122\",\"onset_local\":\"2020-10-17T02:00:00\",\"effective_utc\":\"2020-10-16T18:17:00\",\"severity\":\"Watch\",\"title\":\"Freeze Warning issued October 16 at 2:17PM EDT until October 17 at 9:00AM EDT by NWS Wilmington OH\",\"description\":\"* WHAT...Sub-freezing temperatures as low as 30 expected.* WHERE...Portions of East Central and Southeast Indiana,Northeast and Northern Kentucky and Central, South Central,Southwest and West Central Ohio.* WHEN...From 2 AM to 9 AM EDT Saturday.* IMPACTS...Frost and freeze conditions will kill crops, other sensitive vegetation and possibly damage unprotected outdoor plumbing.\"}],\"city_name\":\"Duque de Caxias\",\"state_code\":\"21\"}";
            //result = JObject.Parse(sim);
            weatherController.GerarLog("Retorno pro App:\r\n" + result);
            return result;
        }
        #endregion
    }
}