using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AZIoTClient.ViewModels
{
    public class BarometerViewModel : BaseViewModel
    {
        // Set speed delay for monitoring changes.
        SensorSpeed speed = SensorSpeed.UI;


        bool _isMonitoring;
        public bool IsMonitoring
        {
            get => _isMonitoring;
            set { SetProperty(ref _isMonitoring, value); ToggleBarometerCommand?.Execute(null); }
        }
        
        bool _isSendingToIoTHub;
        public bool IsSendingToIoTHub
        {
            get => _isSendingToIoTHub;
            set => SetProperty(ref _isSendingToIoTHub, value);
        }

        double _pressure;
        public double Pressure
        {
            get => _pressure;
            set => SetProperty(ref _pressure, value);
        }

        public string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public Command ToggleBarometerCommand { get; set; }

        public BarometerViewModel()
        {
            ToggleBarometerCommand = new Command(async () => await ToggleBarometer());
        }

        void Barometer_ReadingChanged(object sender, BarometerChangedEventArgs e)
        {
            Pressure = e.Reading.PressureInHectopascals;     

            if(IsSendingToIoTHub)
            {
                try
                {
                    string payload = Newtonsoft.Json.JsonConvert.SerializeObject(e.Reading);
                    Acr.UserDialogs.UserDialogs.Instance.Toast(payload, TimeSpan.FromSeconds(.5));
                    Task.Run(async () => await Services.IoTClient.Instance.SendEvent(payload));
                    // If the connection couldn't be opened, make sure we don't keep trying to send events.
                    IsSendingToIoTHub = Services.IoTClient.Instance.IsConnectionOpen;
                    
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error sending data to IoT Hub: {ex.Message} {ex.GetType().ToString()}");
                    IsSendingToIoTHub = false;
                }
            }
        }

        Task ToggleBarometer()
        {
            return Task.Run(() =>
            {
                try
                {
                    if (IsMonitoring)
                        Barometer.Start(speed);
                    else
                        Barometer.Stop();

                    System.Diagnostics.Debug.WriteLine(Barometer.IsMonitoring);

                    ErrorMessage = string.Empty;
                }
                catch (FeatureNotSupportedException fnsEx)
                {
                    // Feature not supported on device
                    ErrorMessage = fnsEx.Message;
                    Acr.UserDialogs.UserDialogs.Instance.Alert(ErrorMessage,"Error");
                }
                catch (Exception ex)
                {
                    // Other error has occurred.
                    ErrorMessage = ex.Message;
                    Acr.UserDialogs.UserDialogs.Instance.Alert(ErrorMessage, "Error");
                }
            });
        }

        internal override void Appearing()
        {
            Barometer.ReadingChanged += Barometer_ReadingChanged;
        }

        internal override void Disappearing()
        {
            Barometer.ReadingChanged -= Barometer_ReadingChanged;
        }
    }
}
