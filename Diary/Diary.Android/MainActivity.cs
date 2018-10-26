using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Diary.Droid.Service;
using Diary.Service;
using Xamarin.Forms;

namespace Diary.Droid
{
    [Activity(Label = "Diary", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App(new AndroidInitializer(this)));
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        private readonly Activity _activity;

        public AndroidInitializer(Activity activity)
        {
            _activity = activity;
        }
        public void RegisterTypes(IDependencyContainer container)
        {
            container.RegisterSingleton(typeof(IUIService),() => new UIService(_activity));
        }
    }
}