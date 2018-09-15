using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using CarouselView.FormsPlugin.Android;

namespace TimeTracker.Droid
{
    [Activity(Label = "TimeTracker", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            XamForms.Controls.Droid.Calendar.Init();
            global::Xamarin.Forms.Forms.Init(this, bundle);
            Rg.Plugins.Popup.Popup.Init(this, bundle);
            CarouselViewRenderer.Init();
            LoadApplication(new App());
        }
    }
}

