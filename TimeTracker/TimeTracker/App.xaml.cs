using DLToolkit.Forms.Controls;
using System;
using TimeTracker.Database;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TimeTracker
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("ODgyMUAzMTM2MmUzMjJlMzBqM1hJcFFlUm1YYmNoUlloVzU5bWNWWE5nQjZOWDJ2eFcwNXZicEM3YWFZPQ==");
            InitializeComponent();
            FlowListView.Init();
            DatabaseHelper.GetInstance().CreateDatabase();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
