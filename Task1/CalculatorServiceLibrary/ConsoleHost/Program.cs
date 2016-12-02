using System;
using System.ServiceModel;

namespace ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
			var host = new ServiceHost(typeof(CalculatorServiceLibrary.CalculatorService));

            host.Open();
            Console.WriteLine("Http Service started");

            Console.ReadLine();

            host.Close();
            Console.WriteLine("Http Service closed");
        }
    }
}
