using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AZIoTClient.ViewModels
{
    public class MagnetometerViewModel : BaseViewModel
    {
        // Set speed delay for monitoring changes.
        SensorSpeed speed = SensorSpeed.UI;

        bool _isSendingToIoTHub;
        public bool IsSendingToIoTHub
        {
            get => _isSendingToIoTHub;
            set => SetProperty(ref _isSendingToIoTHub, value);
        }

        bool _isEnableMagnetometer;
        public bool IsEnableMagnetometer
        {
            get => _isEnableMagnetometer;
            set { SetProperty(ref _isEnableMagnetometer, value); ToggleMagnetometer(value); }
        }

        string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }


        float _x;
        public float X
        {
            get => _x;
            set => SetProperty(ref _x, value);
        }

        float _y;
        public float Y
        {
            get => _y;
            set => SetProperty(ref _y, value);
        }

        float _z;
        public float Z
        {
            get => _z;
            set => SetProperty(ref _z, value);
        }

        public MagnetometerViewModel()
        {
        }

        public void ToggleMagnetometer(bool isOn)
        {
            try
            {
                if (isOn)
                    Magnetometer.Start(speed);
                else
                    Magnetometer.Stop();
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
                ErrorMessage = fnsEx.Message;
                Acr.UserDialogs.UserDialogs.Instance.Alert(ErrorMessage, "Error");

            }
            catch (Exception ex)
            {
                // Some other exception has occurred
                ErrorMessage = ex.Message;
                Acr.UserDialogs.UserDialogs.Instance.Alert(ErrorMessage, "Error");
            }
        }

        void Magnetometer_ReadingChanged(object sender, MagnetometerChangedEventArgs e)
        {
            X = e.Reading.MagneticField.X;
            Y = e.Reading.MagneticField.Y;
            Z = e.Reading.MagneticField.Z;

            var magnet = new { MagnetometerX = X, MagnetometerY = Y, MagnetometerZ = Z };

            if (IsSendingToIoTHub)
            {
                string payload = Newtonsoft.Json.JsonConvert.SerializeObject(e);
                Acr.UserDialogs.UserDialogs.Instance.Toast(payload, TimeSpan.FromSeconds(.5));
                Task.Run(async () => await Services.IoTClient.Instance.SendEvent(payload));
            }
        }


        internal override void Appearing()
        {
            Magnetometer.ReadingChanged += Magnetometer_ReadingChanged;
        }

        internal override void Disappearing()
        {
            Magnetometer.ReadingChanged -= Magnetometer_ReadingChanged;
        }
    }
}
