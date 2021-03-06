﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

using System.Timers;
using AZIoTClient.Models;
using System.Linq;
using AZIoTClient.Helpers;

namespace AZIoTClient.ViewModels
{
    public class GeolocationViewModel : BaseViewModel
    {
        bool _isSendingToIoTHub;
        public bool IsSendingToIoTHub
        {
            get => _isSendingToIoTHub;
            set => SetProperty(ref _isSendingToIoTHub, value);
        }

        bool _isEnableLocationTracking;
        public bool IsEnableLocationTracking
        {
            get => _isEnableLocationTracking;
            set { SetProperty(ref _isEnableLocationTracking, value); _timer.Enabled = value; }
        }

        double _latitude;
        public double Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }

        double _longitude;
        public double Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }

        double? _altitude;
        public double? Altitude
        {
            get => _altitude;
            set => SetProperty(ref _altitude, value);
        }

        string _address;
        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }

        public Command GetLocationCommand { get; set; }

        public Command ForceSendCommand { get; set; }

        Location _location { get; set; }

        System.Timers.Timer _timer;

        public GeolocationViewModel()
        {

            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += _timer_Elapsed;

            GetLocationCommand = new Command(async () => await GetLocation());
            ForceSendCommand = new Command(async () => await ForceSend());
        }

        async Task GetLocation()
        {
            GeolocationRequest req = new GeolocationRequest
            {
                DesiredAccuracy = GeolocationAccuracy.Best
            };

            _location = await Geolocation.GetLocationAsync(req);


            if (_location != null)
            {
                Latitude = _location.Latitude;
                Longitude = _location.Longitude;
                Altitude = _location.Altitude;

                var placemarks = await Geocoding.GetPlacemarksAsync(_location);
                var placemark = placemarks?.FirstOrDefault();

                if (placemark != null)
                {

                    Address = $"{placemark.FeatureName}, {placemark.Locality}, {placemark.AdminArea} {placemark.PostalCode}";
                }

                if (IsSendingToIoTHub)
                {
                    await ForceSend();
                }
            }
        }

        async Task ForceSend()
        {
            var payloadData = new { _location.Latitude, _location.Longitude, _location.Altitude, _location.Speed, _location.Course, Address, Coordinates = $"{_location.Latitude},{_location.Longitude}" };
            string payload = Newtonsoft.Json.JsonConvert.SerializeObject(payloadData);
            Acr.UserDialogs.UserDialogs.Instance.Toast(payload, TimeSpan.FromSeconds(0.5));
            await Services.IoTClient.Instance.SendEvent(payload);

            //await Services.IoTClient.Instance.UpdateProperties(new DeviceProperties() { Address = Address, Coordinates = $"{GeoAngle.FromDouble(Latitude).ToString("NS")}, {GeoAngle.FromDouble(Longitude).ToString("WE")}" });
            await Services.IoTClient.Instance.UpdateProperties(new DeviceProperties() { Address = Address, Coordinates = new IoTCentralLocation { lat = Latitude, lon = Longitude } });
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Task.Run(async () => await GetLocation());
        }

        internal override void Appearing()
        {
            _timer.Enabled = IsEnableLocationTracking;
        }

        internal override void Disappearing()
        {
            _timer.Enabled = false;
        }
    }
}
