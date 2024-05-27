using Autofac;
using AutofacSerilogIntegration;
using Win2Mqtt.Client.Mqtt;

namespace Win2Mqtt.Client
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterLogger();
            builder.RegisterType<FrmMqttMain>().AsSelf().SingleInstance();
            builder.RegisterType<Mqtt.Mqtt>().As<IMqtt>().SingleInstance();
            builder.RegisterType<MqttPublish>().As<IMqttPublish>();
            builder.RegisterType<ToastMessage>().As<IToastMessage>();
            builder.RegisterType<FrmOptions>().AsSelf();
            return builder.Build();
        }
    }
}