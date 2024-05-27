using System.Globalization;
using Win2Mqtt.Sensors.HardwareSensors;

namespace Win2Mqtt.Client.Mqtt
{
    public class MqttPublish : IMqttPublish
    {
        
        private readonly IMqtt _mqtt;
        public MqttPublish(IMqtt mqtt)
        {
            _mqtt = mqtt;
        }
        public async void PublishSystemData()
        {

            List<Task> task = [];

            if (_mqtt.IsConnected == false)
            {

                _mqtt.Connect(MqttSettings.MqttServer, MqttSettings.MqttPort, MqttSettings.MqttUsername, MqttSettings.MqttPassword);
            }

            if (_mqtt.IsConnected == true)
            {
                if (MqttSettings.IsComputerUsed)
                {
                    task.Add(Task.Run(() => PublishStatus()));
                }
                if (MqttSettings.CpuSensor)
                {
                    task.Add(Task.Run(() => _mqtt.Publish("cpuprosessortime", Processor.GetCpuProcessorTime())));
                }
                if (MqttSettings.FreeMemorySensor)
                {
                    task.Add(Task.Run(() => _mqtt.Publish("freememory", Memory.GetFreeMemory())));
                }
                if (MqttSettings.BatterySensor)
                {
                    task.Add(Task.Run(() => PublishBattery()));
                }
                if (MqttSettings.DiskSensor)
                {
                    task.Add(Task.Run(() => PublishDiskStatus()));
                }
            }
            await Task.WhenAll(task).ConfigureAwait(false);
        }
        private async void PublishStatus()
        {
            if (UsingComputer.IsUsing())
            {
                _mqtt.Publish("binary_sensor/inUse", "on");
            }
            else
            {
                _mqtt.Publish("binary_sensor/inUse", "off");
            }

        }
        private void PublishBattery()
        {
            //TODO
            //_mqtt.Publish("Power/BatteryChargeStatus", Power.BatteryChargeStatus());
            //_mqtt.Publish("Power/BatteryFullLifetime", Power.BatteryFullLifetime());
            //_mqtt.Publish("Power/BatteryLifePercent", Power.BatteryLifePercent());
            //_mqtt.Publish("Power/BatteryLifeRemaining", Power.BatteryLifeRemaining());
            //_mqtt.Publish("Power/PowerLineStatus", Power.PowerLineStatus());
        }
        private void PublishDiskStatus()
        {
            try
            {
                foreach (var drive in DriveInfo.GetDrives())
                {
                    double freeSpace = drive.TotalFreeSpace;
                    double totalSpace = drive.TotalSize;
                    double percentFree = freeSpace / totalSpace * 100;
                    float num = (float)percentFree;

                    string rawdrivename = drive.Name.Replace(":\\", "");

                    _mqtt.Publish("drive/" + rawdrivename + "/totalsize", drive.TotalSize.ToString(CultureInfo.CurrentCulture));
                    _mqtt.Publish("drive/" + rawdrivename + "/percentfree", Convert.ToString(Math.Round(Convert.ToDouble(percentFree.ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture), 0), CultureInfo.CurrentCulture));
                    _mqtt.Publish("drive/" + rawdrivename + "/availablefreespace", drive.AvailableFreeSpace.ToString(CultureInfo.CurrentCulture));
                }
            }
            catch (Exception)
            {
                // throw;
            }

        }
        private static bool NetworkUp()
        {

            try
            {
                return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            }
            catch (Exception)
            {
                return false;
            }

        }
        
        
    }
}