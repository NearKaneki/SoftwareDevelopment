namespace DiCont
{
    public class ServiceDescriptor
    {
        public Type ServiceType { get;}

        public Type ImplementationType { get;}

        public object Implementation { get; internal set; }

        public ServiceLifetime LifeTime { get;}

        public ServiceDescriptor(Type serviceType, Type implementationType, ServiceLifetime lifeTime)
        {
            ServiceType = serviceType;
            ImplementationType = implementationType;
            LifeTime = lifeTime;
        }
    }
}
