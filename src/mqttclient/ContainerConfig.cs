using Autofac;
using Win2Mqtt.Client.Mqtt;

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
            builder.RegisterType<ToastMessage>().As<IToastMessage>();
            builder.RegisterType<FrmOptions>().AsSelf();
            return builder.Build();
        }
    }
}