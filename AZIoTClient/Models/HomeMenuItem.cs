using System;
using System.Collections.Generic;
using System.Text;

namespace AZIoTClient.Models
{
    public enum MenuItemType
    {
        Home,
        Barometer,
        Battery,
        Compass,
        Geolocation,
        Gyroscope,
        Magnetometer,
        Settings,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }

        public string IconSource { get; set; }
    }
}
