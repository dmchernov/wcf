using System;
using System.Linq;
using NUnit.Framework;
using TestProject.NorthwindService;

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
				Console.WriteLine(order.OrderID);
			}
		}

		[Test]
		public void GetOrderExTest ()
		{
			var service = new NorthwindService.NorthwindServiceClient();

			int orderId = service.GetOrders().First().OrderID;
			var fullOrderData = service.GetOrderEx(orderId);
			if (fullOrderData == null) return;

			Console.WriteLine($"Order Id: {fullOrderData.OrderID}");
			Console.WriteLine($"Order date: {fullOrderData.OrderDate:d}");
			if (fullOrderData.Order_Details != null && fullOrderData.Order_Details.Length > 0)
			foreach (var od in fullOrderData.Order_Details)
			{
				Console.WriteLine($"Product: {od.Product.ProductName}; Quantity: {od.Quantity}; UnitPrice: {od.UnitPrice:C}; Discount: {od.Discount}");
			}
		}

		[Test]
		public void AddOrderTest ()
		{
			var service = new NorthwindService.NorthwindServiceClient();

			var newOrder = new Order()
			{};
		}
	}
}
