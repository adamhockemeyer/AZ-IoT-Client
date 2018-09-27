using System;
using System.Threading.Tasks;
using AZIoTClient.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AZIoTClient.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        Color _iotStatusColor;
        public Color IoTStatusColor
        {
            get => _iotStatusColor;
            set => SetProperty(ref _iotStatusColor, value);
        }

        string _iotStatusMessage;
        public string IoTStatusMessage
        {
            get => _iotStatusMessage;
            set => SetProperty(ref _iotStatusMessage, value);
        }

        public Command<string> GoToPageCommand { get; set; }

        public HomeViewModel()
        {
            GoToPageCommand = new Command<string>(async (page) => await GoToPage(page));
        }

        async Task GoToPage(string page)
        {
            var mainPage = Application.Current.MainPage as Views.MainPage;

            MenuItemType pageEnum;

            if(Enum.TryParse<MenuItemType>(page, out pageEnum))
            {
               await mainPage.NavigateFromMenu((int)pageEnum);
            }
        }

        internal override void Appearing()
        {
            base.Appearing();

            Task.Run(async () =>
            {
                string hostname = await SecureStorage.GetAsync("_hostname");
                string sharedAccessKey = await SecureStorage.GetAsync("_sharedAccessKey");
                string deviceId = await SecureStorage.GetAsync("_deviceId");

                if (string.IsNullOrEmpty(hostname) || string.IsNullOrEmpty(sharedAccessKey) || string.IsNullOrEmpty(deviceId))
                {
                    IoTStatusColor = Color.Red;
                    IoTStatusMessage = "Not Configured";
                }
                else
                {
                    IoTStatusColor = Color.DarkGreen;
                    IoTStatusMessage = $"Configured: {hostname}";
                }
            });
        }

        internal override void Disappearing()
        {
            base.Disappearing();
        }
    }
}
