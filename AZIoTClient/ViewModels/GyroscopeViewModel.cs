using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AZIoTClient.ViewModels
{
    public class GyroscopeViewModel : BaseViewModel
    {
        // Set speed delay for monitoring changes.
        SensorSpeed speed = SensorSpeed.UI;

        bool _isSendingToIoTHub;
        public bool IsSendingToIoTHub
        {
            get => _isSendingToIoTHub;
            set => SetProperty(ref _isSendingToIoTHub, value);
        }

        bool _isEnableGyroscope;
        public bool IsEnableGyroscope
        {
            get => _isEnableGyroscope;
            set { SetProperty(ref _isEnableGyroscope, value); ToggleGyroscope(value); }
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


        public GyroscopeViewModel()
        {

        }

        public void ToggleGyroscope(bool isOn)
        {
            try
            {
                if (isOn)
                    Gyroscope.Start(speed);
                else
                    Gyroscope.Stop();
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

        void Gyroscope_ReadingChanged(object sender, GyroscopeChangedEventArgs e)
        {
            // Heading = e.Reading.HeadingMagneticNorth;

            X = e.Reading.AngularVelocity.X;
            Y = e.Reading.AngularVelocity.Y;
            Z = e.Reading.AngularVelocity.Z;

            if (IsSendingToIoTHub)
            {
                string payload = Newtonsoft.Json.JsonConvert.SerializeObject(e);
                Acr.UserDialogs.UserDialogs.Instance.Toast(payload, TimeSpan.FromSeconds(.5));
                Task.Run(async () => await Services.IoTClient.Instance.SendEvent(payload));
            }
        }


        internal override void Appearing()
        {
            Gyroscope.ReadingChanged += Gyroscope_ReadingChanged;
        }

        internal override void Disappearing()
        {
            Gyroscope.ReadingChanged -= Gyroscope_ReadingChanged;
        }
    }
}
