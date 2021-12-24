namespace DiCont
{
    public class DiCont
    {
        private List<ServiceDescriptor> dependencies;
        public DiCont()
        {
            dependencies = new List<ServiceDescriptor>();
        }
        public void AddTransient<TService, TImplementation>()
        {
            dependencies.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient));
        }
        public void AddSingleton<TService, TImplementation>()
        {
            dependencies.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton));
        }
        public T Get<T>() => (T)Get(typeof(T));
        public object Get(Type serviceType)
        {
            var descriptor = dependencies.SingleOrDefault(x => x.ServiceType == serviceType);
            if (descriptor == null)
            {
                throw new Exception("Сервис не найден");
            }
            if (descriptor.Implementation != null)
            {
                return descriptor.Implementation;
            }
            var actualType = descriptor.ImplementationType;
            var constructor = actualType.GetConstructors().First();
            if (constructor.GetParameters().Any(x => IsItCycled(serviceType, x.ParameterType)))
            {
                throw new Exception("Цикл");
            }
            var parameters = constructor.GetParameters().Select(x => Get(x.ParameterType)).ToArray();
            var implementation = Activator.CreateInstance(actualType, parameters);
            if (descriptor.LifeTime == ServiceLifetime.Singleton)
            {
                descriptor.Implementation = implementation;
            }
            return implementation;
        }
        public bool IsItCycled(Type serviceType, Type parametrType)
        {
            var descriptor = dependencies.SingleOrDefault(x => x.ServiceType == parametrType);
            var actualType = descriptor.ImplementationType;
            var constructorType = actualType.GetConstructors().First();
            return constructorType.GetParameters().Any(x => Equals(serviceType, x.ParameterType));
        }
    }
}
