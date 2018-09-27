using System;
using System.Collections.Generic;
using AZIoTClient.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace AZIoTClient.Views
{
    public partial class BatteryPage : ContentPage
    {
        public BatteryPage()
        {
            InitializeComponent();

            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);

            BindingContext = new BatteryViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ((BaseViewModel)BindingContext).Appearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            ((BaseViewModel)BindingContext).Disappearing();
        }
    }
}
