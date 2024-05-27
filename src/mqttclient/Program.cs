using Autofac;
using System;
using System.Threading;
using System.Windows.Forms;


namespace Win2Mqtt.Client
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                FrmMqttMain mainForm = scope.Resolve<FrmMqttMain>();
                Application.ThreadException += new ThreadExceptionEventHandler(mainForm.UnhandledThreadExceptionHandler);
                Application.Run(mainForm);
            }


        }
    }
}