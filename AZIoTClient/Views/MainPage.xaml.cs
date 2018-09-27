using AZIoTClient.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AZIoTClient.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();

            MasterBehavior = MasterBehavior.Popover;

            MenuPages.Add((int)MenuItemType.Home, (NavigationPage)Detail);
        }

        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.Home:
                        MenuPages.Add(id, new NavigationPage(new HomePage()));
                        break;
                    case (int)MenuItemType.Barometer:
                        MenuPages.Add(id, new NavigationPage(new BarometerPage()));
                        break;
                    case (int)MenuItemType.Battery:
                        MenuPages.Add(id, new NavigationPage(new BatteryPage()));
                        break;
                    case (int)MenuItemType.Compass:
                        MenuPages.Add(id, new NavigationPage(new CompassPage()));
                        break;
                    case (int)MenuItemType.Geolocation:
                        MenuPages.Add(id, new NavigationPage(new GeolocationPage()));
                        break;
                    case (int)MenuItemType.Gyroscope:
                        MenuPages.Add(id, new NavigationPage(new GyroscopePage()));
                        break;
                    case (int)MenuItemType.Magnetometer:
                        MenuPages.Add(id, new NavigationPage(new MagnetometerPage()));
                        break;
                    case (int)MenuItemType.Settings:
                        MenuPages.Add(id, new NavigationPage(new SettingsPage()));
                        break;
                    case (int)MenuItemType.About:
                        MenuPages.Add(id, new NavigationPage(new AboutPage()));
                        break;
                }
            }

            var newPage = MenuPages[id];


            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;

                var master = (Master as MenuPage);

                master.ClearSelectedMenuItem();
            }
        }
    }
}