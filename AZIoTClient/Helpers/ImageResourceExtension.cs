using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AZIoTClient.Helpers
{
    /// <summary>
    /// Use this to be able to use images in the shared project.
    /// Use like:
    /// <Image Source="{imageResource:ImageResource AZIoTClient.Resources.Images.logo.png}" HeightRequest="125"  Aspect="AspectFit" />
    /// </summary>
    [ContentProperty("Source")]
    public class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
            {
                return null;
            }
            // Do your translation lookup here, using whatever method you require
            var imageSource = ImageSource.FromResource(Source);


            return imageSource;
        }
    }
}
