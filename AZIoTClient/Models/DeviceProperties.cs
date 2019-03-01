using System;
namespace AZIoTClient.Models
{
    /*
     * This class can be serliaized to JSON and used to update device properties in IoT Central.
     */
    public class DeviceProperties
    {
        public IoTCentralLocation Coordinates { get; set; }
        public string Address { get; set; }
    }

    public class IoTCentralLocation
    {
        public double lat { get; set; }
        public double lon { get; set; }
    }
}
