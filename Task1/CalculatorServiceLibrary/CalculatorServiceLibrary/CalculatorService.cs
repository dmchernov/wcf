namespace CalculatorServiceLibrary
{
    public class CalculatorService : ICalculatorService
    {
        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Mul(int a, int b)
        {
            return a * b;
        }

	    public int Sub(int a, int b)
	    {
		    return a - b;
	    }

	    public int Div(int a, int b)
	    {
		    return a/b;
	    }
    }
}
