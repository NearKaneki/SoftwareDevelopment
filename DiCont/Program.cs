namespace DiCont
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new DiCont();
            container.AddTransient<IA, A>();
            container.AddTransient<IB, B>();
            container.Get<IB>();
        }
    }
}