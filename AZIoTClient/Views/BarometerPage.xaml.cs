using System;
using System.Collections.Generic;

using Xamarin.Forms;

using AZIoTClient.ViewModels;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace AZIoTClient.Views
{
    public partial class BarometerPage : ContentPage
    {
        public BarometerPage()
        {
            InitializeComponent();

            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);

            BindingContext = new BarometerViewModel();
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
