using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
			var host = new ServiceHost(typeof(CalculatorServiceLibrary.CalculatorService));

            host.Open();
            Console.WriteLine("Service started");

            Console.ReadLine();

            host.Close();
            Console.WriteLine("Service closed");
        }
    }
}
