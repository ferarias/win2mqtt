using System.Globalization;
using uPLibrary.Networking.M2Mqtt;

namespace Win2Mqtt.Client
{
    public partial class FrmOptions : Form
    {

        
        public string TriggerFile { get; set; }
        public FrmMqttMain ParentFrm { get; set; }
        public FrmOptions(FrmMqttMain Mainform)
        {

            InitializeComponent();
            ParentFrm = Mainform;
            TriggerFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "triggers.json");
            LoadSettings();
            if (txtmqtttopic.TextLength == 0)
            {
                txtmqtttopic.Text = System.Environment.MachineName;
            }

            if (txtMqttTimerInterval.TextLength == 0)
            {
                txtMqttTimerInterval.Text = "60000";
            }

            if (txtmqtttopic.Text.Contains('#') == true)
            {
                txtmqtttopic.Text = txtmqtttopic.Text.Replace("/#", "");
                Properties.Settings.Default["mqtttopic"] = txtmqtttopic.Text;
                Properties.Settings.Default.Save();
            }

        }
        private void LoadSettings()
        {
            txtmqttserver.Text = MqttSettings.MqttServer;
            txtmqttusername.Text = MqttSettings.MqttUsername;
            txtmqttpassword.Text = MqttSettings.MqttPassword;
            txtmqtttopic.Text = MqttSettings.MqttTopic;
            txtMqttTimerInterval.Text = MqttSettings.MqttTimerInterval.ToString(CultureInfo.CurrentCulture);
            ChkBatterySensor.Checked = MqttSettings.BatterySensor;
            ChkDiskSensor.Checked = MqttSettings.DiskSensor;
            chkCpuSensor.Checked = MqttSettings.CpuSensor;
            chkMemorySensor.Checked = MqttSettings.FreeMemorySensor;
            chkVolumeSensor.Checked = MqttSettings.VolumeSensor;
            ChkComputerUsed.Checked = MqttSettings.IsComputerUsed;
            chkMinimizeToTray.Checked = MqttSettings.MinimizeToTray;
            ChkMonitor.Checked = MqttSettings.Monitor;
            chktoast.Checked = MqttSettings.Toast;
            ChkProcesses.Checked = MqttSettings.App;
            chkHibernate.Checked = MqttSettings.Hibernate;
            chkShutdown.Checked = MqttSettings.Shutdown;
            chkReboot.Checked = MqttSettings.Reboot;
            chkSuspend.Checked = MqttSettings.Suspend;

            chkStartUp.Checked = Convert.ToBoolean(Properties.Settings.Default["RunAtStart"], CultureInfo.CurrentCulture);

        }
        private void SaveSettings()
        {
            MqttSettings.MqttServer = txtmqttserver.Text;
            MqttSettings.MqttUsername = txtmqttusername.Text;
            MqttSettings.MqttPassword = txtmqttpassword.Text;
            MqttSettings.MqttTopic = txtmqtttopic.Text;
            MqttSettings.MqttTimerInterval = txtMqttTimerInterval.Text;
            MqttSettings.MinimizeToTray = chkMinimizeToTray.Checked;
            MqttSettings.CpuSensor = chkCpuSensor.Checked;
            MqttSettings.FreeMemorySensor = chkMemorySensor.Checked;
            MqttSettings.VolumeSensor = chkVolumeSensor.Checked;
            MqttSettings.DiskSensor = (bool)ChkDiskSensor.Checked;
            MqttSettings.IsComputerUsed = ChkComputerUsed.Checked;
            MqttSettings.BatterySensor = ChkBatterySensor.Checked;
            MqttSettings.Monitor = ChkMonitor.Checked;
            MqttSettings.Toast = chktoast.Checked;
            MqttSettings.App = ChkProcesses.Checked;
            MqttSettings.Hibernate = chkHibernate.Checked;
            MqttSettings.Shutdown = chkShutdown.Checked;
            MqttSettings.Reboot = chkReboot.Checked;
            MqttSettings.Suspend = chkSuspend.Checked;
            MqttSettings.Save();
        }
        private void CmdClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void CmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    SaveSettings();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error durring savesettings error:" + ex.Message);
                    throw;
                }

                try
                {
                    ParentFrm.ReloadApp();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error durring ReloadApp error:" + ex.Message);
                    throw;
                }
                try
                {
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error durring Close error:" + ex.Message);

                    throw;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("error" + ex.Message + " details: " + ex.InnerException);
                throw;
            }

        }
        private void ChkStartUp_CheckedChanged(object sender, EventArgs e)
        {
            {
                var rk = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (rk != null)
                {
                    if (chkStartUp.Checked)
                    {
                        rk.SetValue(Constants.AppId, Application.ExecutablePath.ToString(CultureInfo.CurrentCulture));
                    }
                    else
                    {
                        rk.DeleteValue(Constants.AppId, false);
                    }
                }
                Properties.Settings.Default["RunAtStart"] = chkStartUp.Checked;
            }
        }
        private void Button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                var client = new MqttClient(txtmqttserver.Text, Convert.ToInt16(textBox1.Text, CultureInfo.CurrentCulture), false, null, null, MqttSslProtocols.None, null);

                if (txtmqttusername.Text.Length == 0)
                {
                    byte code = client.Connect(Guid.NewGuid().ToString());
                }
                else
                {
                    byte code = client.Connect(Guid.NewGuid().ToString(), txtmqttusername.Text, txtmqttpassword.Text);
                }
                MessageBox.Show($"connection ok id: {client.ClientId}");
            }
            catch (Exception)
            {
                MessageBox.Show("Connection failed");
                //throw;
            }
        }
    }
}
