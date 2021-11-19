using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Locations;
using Android.OS;
using Plugin.CurrentActivity;
using Xamarin.Forms;

namespace PrevisaoCerta.Droid
{
    [Activity(Label = "PrevisaoCerta", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            //Inicializa a atividade atual para funcionamento correto do plugin de geolocalização
            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            //Verifica se o GPS está ligado ao inciar a aplicação, caso não esteja, solicita a ativação ao usuário
            LocationManager locationManager = (LocationManager)Forms.Context.GetSystemService(LocationService);
            if (locationManager.IsProviderEnabled(LocationManager.GpsProvider) == false)
            {
                AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                AlertDialog alert = dialog.Create();
                alert.SetTitle("Ops!");
                alert.SetMessage("Seu GPS não está ativado. Por favor, ative o GPS para te darmos resultados mais precisos.");
                alert.SetButton("Ativar", (c, ev) =>
                {
                    Intent gpsSettingIntent = new Intent(Android.Provider.Settings.ActionLocationSourceSettings);
                    Forms.Context.StartActivity(gpsSettingIntent);
                });
                alert.Show();
            }

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}