using Plugin.Geolocator;
using PrevisaoCerta.Business;
using PrevisaoCerta.Model.AlertWeather;
using PrevisaoCerta.Model.CurrentWeather;
using System;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace PrevisaoCerta
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        #region Constructor
        public MainPage()
        {
            InitializeComponent();

            IsLoading = false;
            BindingContext = this;
        }
        #endregion

        #region Contants
        private string TEMPERATURE_HIGH_WITH_RAIN  = "Temperatura alta, ideal para aproveitar bastante se não estivesse chovendo então, se for sair, não esqueça o guarda-chuva.";
        private string TEMPERATURE_HIGH_WITHOUT_RAIN  = "Temperatura alta, ideal para ir à praia, passear ao ar livre e/ou tomar um sorvete.";
        private string TEMPERATURE_MEDIA_WITH_RAIN = "Temperatura média, ideal para quem não gosta de muito calor e nem de muito frio, mas está chovendo, cuidado para não se molhar.";
        private string TEMPERATURE_MEDIA_WITHOUT_RAIN = "Temperatura média, ideal para quem não gosta de muito calor e nem de muito frio, adequada para ser mais produtivo(a).";
        private string TEMPERATURE_LOW_WITH_RAIN = "Tempatura baixa e tempo chuvoso, se for sair não esqueça o guarda-chuva, senão, fique em casa e tome um chocolate quente.";
        private string TEMPERATURE_LOW_WITHOUT_RAIN = "Tempatura baixa, sugerimos se agasalhar e, se possível, ficar em casa curtindo seu streaming preferido.";
        private string HUMIDITY_RELATIVE_OF_AIR_BAD = "A umidade relativa do ar não está boa no momento, então se hidrate bastante";
        private string HUMIDITY_RELATIVE_OF_AIR_GOOD  = "A umidade relativa do ar está no nível adequado no momento.";
        #endregion

        #region Variables
        private bool isLoading;
        private string latitude;
        private string longitude;
        private string city;
        #endregion

        #region Properties
        public bool IsLoading
        {
            get
            {
                return this.isLoading;
            }
            set
            {
                this.isLoading = value;
                RaisePropertyChanged("IsLoading");
            }
        }

        private string Latitude
        {
            get
            {
                return this.latitude;
            }
            set
            {
                this.latitude = value;
                RaisePropertyChanged("Latitude");
            }
        }

        private string Longitude
        {
            get
            {
                return this.longitude;
            }
            set
            {
                this.longitude = value;
                RaisePropertyChanged("Longitude");
            }
        }

        private string City
        {
            get
            {
                return this.city;
            }
            set
            {
                this.city = value;
                RaisePropertyChanged("City");
            }
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;

        private async void GetLocalization(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtCity.Text))
                {
                    IsLoading = true;

                    var locator = CrossGeolocator.Current;
                    locator.DesiredAccuracy = 50;

                    var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));

                    var address = await locator.GetAddressesForPositionAsync(position);

                    Plugin.Geolocator.Abstractions.Address addressInfo = address.FirstOrDefault();

                    if (addressInfo.CountryName != "Brasil")
                    {
                        await DisplayAlert("Ops!", "Você deve informar uma cidade nacional.", "OK");
                        IsLoading = false;
                        return;
                    }

                    City = txtCity.Text = addressInfo.SubAdminArea;
                    Latitude = addressInfo.Latitude.ToString().Replace(",", ".");
                    Longitude = addressInfo.Longitude.ToString().Replace(",", ".");

                    IsLoading = false;
                }
            }
            catch (Exception)
            {
                IsLoading = false;
                return;
            }
        }

        private async void GetForecast(object sender, EventArgs e)
        {
            try
            {
                IsLoading = true;
                txtCity.IsEnabled = btnForecast.IsEnabled = false;

                if (string.IsNullOrEmpty(txtCity.Text))
                {
                    await DisplayAlert("Ops!", "Você deve informar a cidade que deseja saber a previsão do tempo.", "OK");
                    slResult.IsVisible = IsLoading = false;
                    txtCity.IsEnabled = btnForecast.IsEnabled = true;
                    return;
                }

                bool doSearchByCity = (!string.IsNullOrEmpty(City) && txtCity.Text != City);

                Weather objWeather = new Weather();

                if ((string.IsNullOrEmpty(Latitude) && string.IsNullOrEmpty(Longitude)) || doSearchByCity)
                {
                    OutAlertWeather alert = await objWeather.GetAlertWeatherByCity(txtCity.Text);

                    FillAndShowControls(alert, true);
                }
                else
                {
                    OutAlertWeather alert = await objWeather.GetAlertWeatherByLatLon(Latitude, Longitude);

                    FillAndShowControls(alert, false);
                }
            }
            catch (Exception ex)
            {
                slResult.IsVisible = IsLoading = false;
                txtCity.IsEnabled = btnForecast.IsEnabled = true;
                await DisplayAlert("Viish", $"Deu ruim na sua consulta: {ex.Message}", "Tentar Novamente");
            }

        }
        #endregion

        #region Methods
        public void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private async void FillAndShowControls(OutAlertWeather alert, bool isSearchByCity)
        {
            if (alert != null)
            {
                if (alert.Data.Count > 0)
                {
                    Model.AlertWeather.Data alertInfo = alert.Data.FirstOrDefault();

                    lblCityAlert.Text = alert.CityName.ToUpper() == txtCity.Text.ToUpper() ? alert.CityName : txtCity.Text;
                    lblSeverityAlert.Text = alertInfo.Severity.Contains("Watch") ? "Observação" : "Aviso";
                    lblExpireAlert.Text = alertInfo.ExpiresLocal.ToShortDateString();
                    
                    slResult.IsVisible = frmAlert.IsVisible = txtCity.IsEnabled = btnForecast.IsEnabled = true;
                    frmForecast.IsVisible = IsLoading = false;
                }
                else
                {
                    Weather objWeather = new Weather();
                    OutCurrentWeather currentWeather = new OutCurrentWeather();

                    if (isSearchByCity)
                        currentWeather = await objWeather.GetCurrentWeatherByCity(txtCity.Text);
                    else
                        currentWeather = await objWeather.GetCurrentWeatherByLatLon(Latitude, Longitude);

                    if (currentWeather != null)
                    {
                        if (Convert.ToInt32(currentWeather.Count) > 0)
                        {
                            Model.CurrentWeather.Data currentWeatherInfo = currentWeather.Data.FirstOrDefault();

                            imgIcone.Source = GetIconOrDescForecast(currentWeatherInfo.WeatherCondition.Code, currentWeatherInfo.Pod);
                            lblWeather.Text = GetIconOrDescForecast(currentWeatherInfo.WeatherCondition.Code, string.Empty);

                            lblCityForecast.Text = currentWeatherInfo.CityName.ToUpper() == txtCity.Text.ToUpper() ? currentWeatherInfo.CityName : txtCity.Text;
                            lblTemp.Text = $"{Convert.ToInt16(currentWeatherInfo.Temp)}ºC";
                            lblFeelsTemp.Text = $"{Convert.ToInt16(currentWeatherInfo.AppTemp)}ºC";
                            lblPrecip.Text = $"{currentWeatherInfo.Precip} mm/h";
                            lblHumidity.Text = $"{Convert.ToInt16(currentWeatherInfo.Rh)}%";

                            lblSuggestionForecast.Text = GetSuggestionForecast();
                            
                            slResult.IsVisible = frmForecast.IsVisible = txtCity.IsEnabled = btnForecast.IsEnabled = true;
                            frmAlert.IsVisible = IsLoading = false;
                        }
                        else
                        {
                            if (isSearchByCity)
                                await DisplayAlert("Ops!", "A fonte de previsão do tempo não retornou nenhuma informação para a cidade que você informou. Por favor, tente novamente com outra cidade próxima.", "OK");
                            else
                                await DisplayAlert("Ops!", "A fonte de previsão do tempo não retornou nenhuma informação para o local em que você está. Por favor, tente novamente em um local distinto.", "OK");

                            IsLoading = false;
                            txtCity.IsEnabled = btnForecast.IsEnabled = true;
                            return;
                        }
                    }
                    else
                    {
                        await DisplayAlert("Ops!", "Ocorreu um erro ao consultar a previsão do tempo. Por favor, tente novamente daqui há alguns minutos.", "OK");
                        IsLoading = false;
                        txtCity.IsEnabled = btnForecast.IsEnabled = true;
                        return;
                    }
                }
            }
            else
            {
                await DisplayAlert("Ops!", "Ocorreu um erro ao consultar a previsão do tempo. Por favor, tente novamente daqui há alguns minutos.", "OK");
                IsLoading = false;
                txtCity.IsEnabled = btnForecast.IsEnabled = true;
                return;
            }
        }

        private string GetIconOrDescForecast(string codeForecast, string partDay)
        {
            switch (codeForecast)
            {
                case "200":
                    if (string.IsNullOrEmpty(partDay))
                        return "Trovoada com chuva fraca";
                    else
                        return $"t01{partDay}";
                case "201":
                    if (string.IsNullOrEmpty(partDay))
                        return "Trovoada com chuva";
                    else
                        return $"t02{partDay}";
                case "202":
                    if (string.IsNullOrEmpty(partDay))
                        return "Trovoada com chuva forte";
                    else
                        return $"t03{partDay}";
                case "230":
                    if (string.IsNullOrEmpty(partDay))
                        return "Trovoada com garoa leve";
                    else
                        return $"t04{partDay}";
                case "231":
                    if (string.IsNullOrEmpty(partDay))
                        return "Trovoada com garoa";
                    else
                        return $"t04{partDay}";
                case "232":
                    if (string.IsNullOrEmpty(partDay))
                        return "Trovoada com garoa forte";
                    else
                        return $"t04{partDay}";
                case "233":
                    if (string.IsNullOrEmpty(partDay))
                        return "Trovoada com granizo";
                    else
                        return $"t05{partDay}";
                case "300":
                    if (string.IsNullOrEmpty(partDay))
                        return "Leve garoa";
                    else
                        return $"d01{partDay}";
                case "301":
                    if (string.IsNullOrEmpty(partDay))
                        return "Chuvisco";
                    else
                        return $"d02{partDay}";
                case "302":
                    if (string.IsNullOrEmpty(partDay))
                        return "Garoa forte";
                    else
                        return $"d03{partDay}";
                case "500":
                    if (string.IsNullOrEmpty(partDay))
                        return "Chuva leve";
                    else
                        return $"r01{partDay}";
                case "501":
                    if (string.IsNullOrEmpty(partDay))
                        return "Chuva moderada";
                    else
                        return $"r02{partDay}";
                case "502":
                    if (string.IsNullOrEmpty(partDay))
                        return "Chuva pesada";
                    else
                        return $"r03{partDay}";
                case "511":
                    if (string.IsNullOrEmpty(partDay))
                        return "Chuva congelante";
                    else
                        return $"f01{partDay}";
                case "520":
                    if (string.IsNullOrEmpty(partDay))
                        return "Chuva fraca";
                    else
                        return $"r04{partDay}";
                case "521":
                    if (string.IsNullOrEmpty(partDay))
                        return "Chuva de banho";
                    else
                        return $"r05{partDay}";
                case "522":
                    if (string.IsNullOrEmpty(partDay))
                        return "Chuva forte";
                    else
                        return $"r06{partDay}";
                case "600":
                    if (string.IsNullOrEmpty(partDay))
                        return "Pouca neve";
                    else
                        return $"s01{partDay}";
                case "601":
                    if (string.IsNullOrEmpty(partDay))
                        return "Neve";
                    else
                        return $"s02{partDay}";
                case "602":
                    if (string.IsNullOrEmpty(partDay))
                        return "Neve pesada";
                    else
                        return $"s03{partDay}";
                case "610":
                    if (string.IsNullOrEmpty(partDay))
                        return "Mistura de neve/chuva";
                    else
                        return $"s04{partDay}";
                case "611":
                    if (string.IsNullOrEmpty(partDay))
                        return "Granizo";
                    else
                        return $"s05{partDay}";
                case "612":
                    if (string.IsNullOrEmpty(partDay))
                        return "Granizo forte";
                    else
                        return $"s05{partDay}";
                case "621":
                    if (string.IsNullOrEmpty(partDay))
                        return "Chuva de neve";
                    else
                        return $"s01{partDay}";
                case "622":
                    if (string.IsNullOrEmpty(partDay))
                        return "Chuva forte de neve";
                    else
                        return $"s02{partDay}";
                case "623":
                    if (string.IsNullOrEmpty(partDay))
                        return "Rajadas";
                    else
                        return $"s06{partDay}";
                case "700":
                    if (string.IsNullOrEmpty(partDay))
                        return "Névoa";
                    else
                        return $"a01{partDay}";
                case "711":
                    if (string.IsNullOrEmpty(partDay))
                        return "Fumaça";
                    else
                        return $"a02{partDay}";
                case "721":
                    if (string.IsNullOrEmpty(partDay))
                        return "Confusão climática";
                    else
                        return $"a03{partDay}";
                case "731":
                    if (string.IsNullOrEmpty(partDay))
                        return "Areia/poeira";
                    else
                        return $"a04{partDay}";
                case "741":
                    if (string.IsNullOrEmpty(partDay))
                        return "Névoa";
                    else
                        return $"a05{partDay}";
                case "751":
                    if (string.IsNullOrEmpty(partDay))
                        return "Nevoeiro congelante";
                    else
                        return $"a06{partDay}";
                case "800":
                    if (string.IsNullOrEmpty(partDay))
                        return "Céu limpo";
                    else
                        return $"c01{partDay}";
                case "801":
                    if (string.IsNullOrEmpty(partDay))
                        return "Poucas nuvens";
                    else
                        return $"c02{partDay}";
                case "802":
                    if (string.IsNullOrEmpty(partDay))
                        return "Nuvens dispersas";
                    else
                        return $"c02{partDay}";
                case "803":
                    if (string.IsNullOrEmpty(partDay))
                        return "Nuvens quebradas";
                    else
                        return $"c03{partDay}";
                case "804":
                    if (string.IsNullOrEmpty(partDay))
                        return "Nuvens encobertas";
                    else
                        return $"c04{partDay}";
                case "900":
                    if (string.IsNullOrEmpty(partDay))
                        return "Precipitação desconhecida";
                    else
                        return $"u00{partDay}";
                default:
                    if (string.IsNullOrEmpty(partDay))
                        return "Clima sem descrição no momento";
                    else
                        return "atencao";
            }
        }

        private string GetSuggestionForecast()
        {
            string suggestion = string.Empty;
            short temp = Convert.ToInt16(lblTemp.Text.Replace("ºC", ""));
            short humidity = Convert.ToInt16(lblHumidity.Text.Replace("%", ""));
            int precip = Convert.ToInt32(lblPrecip.Text.Replace(" mm/h", ""));

            if (temp >= 25 && precip <= 5)
                suggestion = humidity >= 60 ? $"{TEMPERATURE_HIGH_WITHOUT_RAIN} {HUMIDITY_RELATIVE_OF_AIR_GOOD}" : $"{TEMPERATURE_HIGH_WITHOUT_RAIN} {HUMIDITY_RELATIVE_OF_AIR_BAD}";
            else if (temp >= 25 && precip > 5)
                suggestion = humidity >= 60 ? $"{TEMPERATURE_HIGH_WITH_RAIN} {HUMIDITY_RELATIVE_OF_AIR_GOOD}" : $"{TEMPERATURE_HIGH_WITH_RAIN} {HUMIDITY_RELATIVE_OF_AIR_BAD}";
            else if ((temp >= 20 && temp <= 24) && precip <= 5)
                suggestion = humidity >= 60 ? $"{TEMPERATURE_MEDIA_WITHOUT_RAIN} {HUMIDITY_RELATIVE_OF_AIR_GOOD}" : $"{TEMPERATURE_MEDIA_WITHOUT_RAIN} {HUMIDITY_RELATIVE_OF_AIR_BAD}";
            else if ((temp >= 20 && temp <= 24) && precip > 5)
                suggestion = humidity >= 60 ? $"{TEMPERATURE_MEDIA_WITH_RAIN} {HUMIDITY_RELATIVE_OF_AIR_GOOD}" : $"{TEMPERATURE_MEDIA_WITH_RAIN} {HUMIDITY_RELATIVE_OF_AIR_BAD}";
            else if (temp < 20 && precip <= 5)
                suggestion = humidity >= 60 ? $"{TEMPERATURE_LOW_WITHOUT_RAIN} {HUMIDITY_RELATIVE_OF_AIR_GOOD}" : $"{TEMPERATURE_LOW_WITHOUT_RAIN} {HUMIDITY_RELATIVE_OF_AIR_BAD}";
            else if (temp < 20 && precip > 5)
                suggestion = humidity >= 60 ? $"{TEMPERATURE_LOW_WITH_RAIN} {HUMIDITY_RELATIVE_OF_AIR_GOOD}" : $"{TEMPERATURE_LOW_WITH_RAIN} {HUMIDITY_RELATIVE_OF_AIR_BAD}";

            return suggestion;
        }
        #endregion
    }
}
