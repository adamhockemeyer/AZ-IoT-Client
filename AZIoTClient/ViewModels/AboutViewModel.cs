using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace AZIoTClient.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";

            OpenWebCommand = new Command<string>((url) => Device.OpenUri(new Uri(url)));
        }

        public ICommand OpenWebCommand { get; }
    }
}