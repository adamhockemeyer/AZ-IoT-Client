using AZIoTClient.Models;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AZIoTClient.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;
        public MenuPage()
        {
            InitializeComponent();

            const string iconSourcePrefix = "resource://AZIoTClient.Resources.Images.";

            menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.Home, Title="Home", IconSource=$"{iconSourcePrefix}home2.svg" },
                new HomeMenuItem {Id = MenuItemType.Barometer, Title="Barometer", IconSource=$"{iconSourcePrefix}barometer.svg" },
                new HomeMenuItem {Id = MenuItemType.Battery, Title="Battery", IconSource=$"{iconSourcePrefix}battery.svg" },
                new HomeMenuItem {Id = MenuItemType.Compass, Title="Compass", IconSource=$"{iconSourcePrefix}compass.svg" },
                new HomeMenuItem {Id = MenuItemType.Geolocation, Title="Geolocation", IconSource=$"{iconSourcePrefix}geolocation.svg" },
                new HomeMenuItem {Id = MenuItemType.Gyroscope, Title="Gryroscope", IconSource=$"{iconSourcePrefix}axis-arrows.svg" },
                new HomeMenuItem {Id = MenuItemType.Magnetometer, Title="Magnetometer", IconSource=$"{iconSourcePrefix}magnet1.svg"},
                new HomeMenuItem {Id = MenuItemType.Settings, Title="Settings", IconSource=$"{iconSourcePrefix}settings.svg" },
                new HomeMenuItem {Id = MenuItemType.About, Title="About", IconSource=$"{iconSourcePrefix}info.svg" }
            };

            ListViewMenu.ItemsSource = menuItems;

            //ListViewMenu.SelectedItem = menuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };
        }

        public void ClearSelectedMenuItem()
        {
            ListViewMenu.SelectedItem = null;
        }
    }
}