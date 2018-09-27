using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

using System.Timers;

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
            set  { SetProperty(ref _isEnableLocationTracking, value); _timer.Enabled = value; }
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

            if(_location != null)
            {
                Latitude = _location.Latitude;
                Longitude = _location.Longitude;
                Altitude = _location.Altitude;

                if (IsSendingToIoTHub)
                {
                    await ForceSend();
                }
            }  
        }

        async Task ForceSend()
        {
            string payload = Newtonsoft.Json.JsonConvert.SerializeObject(_location);
            Acr.UserDialogs.UserDialogs.Instance.Toast(payload, TimeSpan.FromSeconds(0.5));
            await Services.IoTClient.Instance.SendEvent(payload);
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
