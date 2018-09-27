using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AZIoTClient.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        string _hostname;
        public string Hostname
        {
            get => _hostname;
            set => SetProperty(ref _hostname, value);
        }

        string _sharedAccessKey;
        public string SharedAccessKey
        {
            get => _sharedAccessKey;
            set => SetProperty(ref _sharedAccessKey, value);
        }

        string _deviceId;
        public string DeviceId 
        {
            get => _deviceId;
            set => SetProperty(ref _deviceId, value);
        }

        public Command TestConnectionCommand { get; set; }

        public SettingsViewModel()
        {
            TestConnectionCommand = new Command(async () => await TestConnection());
        }

        async Task TestConnection()
        {
            await SetProperties();

            // TODO : Call IoTHub and check for error codes.

            if (string.IsNullOrEmpty(Hostname) || string.IsNullOrEmpty(SharedAccessKey) || string.IsNullOrEmpty(DeviceId))
            {
                // Show an alert
                await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Ensure Host Name, Shared Access Key, and Device ID all have values.", "Missing Values");

                return;
            }

            await Services.IoTClient.Instance.Start(true);
      
        }

        async Task SetProperties()
        {
            try
            {
                if (string.IsNullOrEmpty(Hostname))
                    SecureStorage.Remove("_hostname");
                else
                    await SecureStorage.SetAsync("_hostname", Hostname);

                if (string.IsNullOrEmpty(SharedAccessKey))
                    SecureStorage.Remove("_sharedAccessKey");
                else
                    await SecureStorage.SetAsync("_sharedAccessKey", SharedAccessKey);

                if (string.IsNullOrEmpty(DeviceId))
                    SecureStorage.Remove("_deviceId");
                else
                    await SecureStorage.SetAsync("_deviceId", DeviceId);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
                Acr.UserDialogs.UserDialogs.Instance.Alert(ex.Message);
            }
        }

        internal override void Appearing()
        {
            base.Appearing();

            Task.Run(async () =>
            {
                try
                {
                    Hostname = await SecureStorage.GetAsync("_hostname");
                    SharedAccessKey = await SecureStorage.GetAsync("_sharedAccessKey");
                    DeviceId = await SecureStorage.GetAsync("_deviceId");
                }
                catch (Exception ex)
                {
                    // Possible that device doesn't support secure storage on device.
                }
            });
        }

        internal override void Disappearing()
        {
            base.Disappearing();

            Task.Run(async () => await SetProperties());
        }


    }
}
