namespace DiCont
{
    public class A : IA
    {
        public A(IB b) { }
        public void checkA()
        {
            Console.WriteLine("A");
        }
    }
}
