namespace DiCont
{
    public class B : IB
    {
        public B(IA a) { }
        public void checkB()
        {
            Console.WriteLine("B");
        }
    }
}
