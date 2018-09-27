using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Xamarin.Essentials;

namespace AZIoTClient.Services
{



    public class IoTClient
    {
        DeviceClient deviceClient;
        public TransportType Protocol = TransportType.Amqp;

        public bool IsConnectionOpen { get; private set; }

        private static readonly Lazy<IoTClient> _instance = new Lazy<IoTClient>(() => new IoTClient());

        private IoTClient()
        {

        }

        public static IoTClient Instance 
        {
            get => _instance.Value;
        }

        public async Task Start(bool isConnectionTest = false)
        {
            try
            {
                string hostname = await SecureStorage.GetAsync("_hostname");
                string sharedAccessKey = await SecureStorage.GetAsync("_sharedAccessKey");
                string deviceId = await SecureStorage.GetAsync("_deviceId");

                if(string.IsNullOrEmpty(hostname) || string.IsNullOrEmpty(sharedAccessKey) || string.IsNullOrEmpty(deviceId))
                {
                    await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("IoT Hub connection details missing.  Please update your settings.", "Missing Settings");
                    Debug.WriteLine("IoT Hub connection details missing");
                    return;
                }

                // Connection string found in Azure on the IoT Hub --> Settings --> Shared Access policies, Select device, then Copy the connection string.

                this.deviceClient = DeviceClient.CreateFromConnectionString($"HostName={hostname};SharedAccessKeyName=device;SharedAccessKey={sharedAccessKey}", deviceId, this.Protocol);

                await this.deviceClient.OpenAsync();

                IsConnectionOpen = true;

                if(isConnectionTest)
                {
                    await Acr.UserDialogs.UserDialogs.Instance.AlertAsync($"Connection to {hostname} succeeded!", "Success");
                }

            }
            catch (Exception ex)
            {
                string msg = $"Error opening IoT Hub Connection: {ex.Message} {ex.GetType().ToString()}";
                Debug.WriteLine(msg);
                await Acr.UserDialogs.UserDialogs.Instance.AlertAsync(msg,"Connection Error");
                IsConnectionOpen = false;
            }
        }

        public Task CloseAsync()
        {
            IsConnectionOpen = false;
            return this.deviceClient.CloseAsync();
        }

        public async Task SendEvent(string message)
        {
            if (!IsConnectionOpen)
                await Start();

            if(IsConnectionOpen)
            {
                Message eventMessage = new Message(Encoding.UTF8.GetBytes(message));
                Debug.WriteLine(string.Format("Sending message: '{0}'", message));
                await deviceClient.SendEventAsync(eventMessage);
            }
        }
    }
}
