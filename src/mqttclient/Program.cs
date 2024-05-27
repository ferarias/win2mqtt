using Autofac;
using Serilog;

namespace Win2Mqtt.Client
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("c:\\logs\\log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("Starting engines");

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.ThreadException += new ThreadExceptionEventHandler((sender, e) =>
                {
                    Log.Error(e.Exception, "Something went wrong");

                    if (ShowExceptionMessage(e.Exception))
                    {
                        Application.Exit();
                    }
                });

                var container = ContainerConfig.Configure();
                using var scope = container.BeginLifetimeScope();
                FrmMqttMain mainForm = scope.Resolve<FrmMqttMain>();
                Application.Run(mainForm);

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Something went terribly wrong!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static bool ShowExceptionMessage(Exception e) => MessageBox.Show(
            "An unexpected error has occurred. details:" + e.Message + "innerException:" + e.InnerException + "Continue?",
            "MqttClient" + e.Message + " inner:" + e.InnerException,
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Stop,
            MessageBoxDefaultButton.Button2) == DialogResult.No;
    }
}