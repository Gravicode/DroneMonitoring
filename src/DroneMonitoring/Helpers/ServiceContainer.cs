using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneMonitoring.Helpers
{
    public class ServiceContainer
    {
        Autofac.IContainer container;
        ContainerBuilder containerBuilder;

        public ServiceContainer()
        {
            containerBuilder = new ContainerBuilder();

            
        }

        public static ServiceContainer Instance { get; } = new ServiceContainer();

        public T Resolve<T>() => container.Resolve<T>();

        public object Resolve(Type type) => container.Resolve(type);

        public void Register<TInterface, TImplementation>() where TImplementation : TInterface => containerBuilder.RegisterType<TImplementation>().As<TInterface>();

        public void Build() => container = containerBuilder.Build();
    }
}
