using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestClient
{
	[TestClass]
	public class TcpServiceTest
	{
		[TestMethod]
		public void AddTest ()
		{
			using (var client = new TcpServiceReference.CalculatorServiceClient())
			{
				Console.WriteLine(client.Add(5, 71));
				Assert.AreEqual(client.Add(5, 5), 10);
			}
		}

		[TestMethod]
		public void MulTest ()
		{
			using (var client = new TcpServiceReference.CalculatorServiceClient())
			{
				Console.WriteLine(client.Mul(5, 5));
				Assert.AreEqual(client.Mul(5, 5), 25);
			}
		}

		[TestMethod]
		public void SubTest ()
		{
			using (var client = new TcpServiceReference.CalculatorServiceClient())
			{
				Console.WriteLine(client.Sub(10, 5));
				Assert.AreEqual(client.Sub(10, 1), 9);
			}
		}

		[TestMethod]
		public void DivTest ()
		{
			using (var client = new TcpServiceReference.CalculatorServiceClient())
			{
				Console.WriteLine(client.Div(10, 5));
				Assert.AreEqual(client.Div(10, 5), 2);
			}
		}

		[TestMethod]
		public void DivByZeroTest ()
		{
			using (var client = new TcpServiceReference.CalculatorServiceClient())
			{
				Console.WriteLine(client.Div(10, 5));
				client.Div(10, 0);
			}
		}
	}
}
