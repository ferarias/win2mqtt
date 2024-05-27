using Autofac;
using Win2Mqtt.Client.Mqtt;
using Win2Mqtt.Sensors.HardwareSensors;

namespace Win2Mqtt.Client
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainFormContainer>().AsSelf().SingleInstance();
            builder.RegisterType<FrmMqttMain>().AsSelf().SingleInstance();
            builder.RegisterType<Logger>().As<ILogger>();
            builder.RegisterType<Mqtt.Mqtt>().As<IMqtt>().SingleInstance();
            builder.RegisterType<MqttPublish>().As<IMqttPublish>();
            builder.RegisterType<Audio>().As<IAudio>().SingleInstance();
            builder.RegisterType<ToastMessage>().As<IToastMessage>();

            builder.RegisterType<FrmOptions>().AsSelf();

            //builder.RegisterAssemblyTypes(Assembly.Load(nameof(mqttclient)))
            //    .Where(t => t.Namespace.Contains("HardwareSensors"))
            //    .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));

            return builder.Build();
        }
    }
}