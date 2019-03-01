using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AZIoTClient.ViewModels
{
    public class BatteryViewModel : BaseViewModel
    {
        bool _isSendingToIoTHub;
        public bool IsSendingToIoTHub
        {
            get => _isSendingToIoTHub;
            set => SetProperty(ref _isSendingToIoTHub, value);
        }

        double _chargeLevel;
        public double ChargeLevel
        {
            get => _chargeLevel;
            set => SetProperty(ref _chargeLevel, value);
        }

        string _powerSource;
        public string PowerSource
        {
            get => _powerSource;
            set => SetProperty(ref _powerSource, value);
        }

        string _chargeState;
        public string ChargeState
        {
            get => _chargeState;
            set => SetProperty(ref _chargeState, value);
        }

        public Command ForceSendCommand { get; set; }

        public BatteryViewModel()
        {
            ChargeLevel = Battery.ChargeLevel;
            PowerSource = Battery.PowerSource.ToString();
            ChargeState = Battery.State.ToString();

            ForceSendCommand = new Command(async () => await ForceSendToIotHub());
        }

        async Task ForceSendToIotHub()
        {
            var battery = new { ChargeLevel, PowerSource, ChargeState };

            string payload = Newtonsoft.Json.JsonConvert.SerializeObject(battery);
            Acr.UserDialogs.UserDialogs.Instance.Toast(payload, TimeSpan.FromSeconds(0.5));
            await Services.IoTClient.Instance.SendEvent(payload);
        }

        void Battery_BatteryChanged(object sender, BatteryInfoChangedEventArgs e)
        {
            ChargeLevel = Battery.ChargeLevel;
            PowerSource = Battery.PowerSource.ToString();
            ChargeState = Battery.State.ToString();

            if (IsSendingToIoTHub)
            {
                Task.Run(async () => await ForceSendToIotHub());
            }
        }


        internal override void Appearing()
        {
            Battery.BatteryInfoChanged += Battery_BatteryChanged;
            
        }

        internal override void Disappearing()
        {
            Battery.BatteryInfoChanged -= Battery_BatteryChanged;
        }
    }
}
