using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AZIoTClient.ViewModels
{
    public class CompassViewModel : BaseViewModel
    {
        // Set speed delay for monitoring changes.
        SensorSpeed speed = SensorSpeed.UI;

        bool _isSendingToIoTHub;
        public bool IsSendingToIoTHub
        {
            get => _isSendingToIoTHub;
            set => SetProperty(ref _isSendingToIoTHub, value);
        }

        bool _isEnableCompass;
        public bool IsEnableCompass
        {
            get => _isEnableCompass;
            set { SetProperty(ref _isEnableCompass, value); ToggleCompass(value); }
        }

        double _heading;
        public double Heading
        {
            get => _heading;
            set => SetProperty(ref _heading, value);
        }

        string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public CompassViewModel()
        {

        }

        void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
        {
            Heading = e.Reading.HeadingMagneticNorth;

            if (IsSendingToIoTHub)
            {
                string payload = Newtonsoft.Json.JsonConvert.SerializeObject(e.Reading.HeadingMagneticNorth);
                Acr.UserDialogs.UserDialogs.Instance.Toast(payload, TimeSpan.FromSeconds(.5));
                Task.Run(async () => await Services.IoTClient.Instance.SendEvent(payload));
            }
        }

        public void ToggleCompass(bool isOn)
        {
            try
            {
                if (isOn)
                    Compass.Start(speed);
                else
                    Compass.Stop();
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


        internal override void Appearing()
        {
            Compass.ReadingChanged += Compass_ReadingChanged;
        }

        internal override void Disappearing()
        {
            Compass.ReadingChanged -= Compass_ReadingChanged;
        }
    }
}
