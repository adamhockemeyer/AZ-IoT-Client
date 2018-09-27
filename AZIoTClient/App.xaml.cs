using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AZIoTClient.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AZIoTClient
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();


            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts

            AppCenter.Start("android=147dbafd-57fb-4040-8c48-e97c62bac723;" +
                  "ios=0b8581ac-0432-4ee3-8d9b-57d3d39f40b7",
                  typeof(Analytics), typeof(Crashes));
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
