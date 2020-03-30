using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using FormsPinView.Droid;
using Xamarin.Forms;

namespace EWalletMD.Droid
{
    //old label EWalletMD
    [Activity(Label = "RMUTSB E Wallet", Icon = "@drawable/LOGO2", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::ZXing.Net.Mobile.Forms.Android.Platform.Init();
            
            PinItemViewRenderer.Init();
            //DatabaseAccess newDb = new DatabaseAccess();
            //string path = newDb.DatabasePath();
            if (Device.Idiom == TargetIdiom.Phone)              //Lock Screen
            {
                RequestedOrientation = ScreenOrientation.Portrait;
            }
            else
            {
                /*RequestedOrientation = ScreenOrientation.SensorLandscape*/;
                RequestedOrientation = ScreenOrientation.Portrait;
            }

            LoadApplication(new App());


        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

    }
    public class CloseApplication : ICloseApplication
    {
        public void closeApplication()
        {
            var activity = (Activity)Forms.Context;
            activity.FinishAffinity();
        }
    }

}