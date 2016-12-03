using System;
using NUnit.Framework;

namespace TestProject
{
	public class UnitTest1
	{
		[Test]
		public void GetOrdersTest ()
		{
			var service = new NorthwindService.NorthwindServiceClient();

			foreach (var order in service.GetOrders())
			{
				Console.WriteLine(order.Customer.CompanyName);
			}
		}
	}
}
