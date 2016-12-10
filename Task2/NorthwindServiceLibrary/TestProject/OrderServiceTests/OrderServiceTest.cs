using System;
using System.Linq;
using System.ServiceModel;
using NUnit.Framework;
using TestProject.Common;
using TestProject.OrderService;
using TestProject.ProductService;

namespace TestProject.OrderServiceTests
{
	public class OrderServiceTest
	{
		[Test]
		public void GetOrdersTest ()
		{
			var service = new OrderServiceClient();

			foreach (var order in service.GetOrders())
			{
				Console.WriteLine(order.OrderID);
			}
		}

		[Test]
		public void GetOrderExTest ()
		{
			var service = new OrderServiceClient();

			int orderId = service.GetOrders().First().OrderID;
			var fullOrderData = service.GetOrderEx(orderId);
			Assert.NotNull(fullOrderData);

			OrdersHelper.PrintFullOrderInfo(fullOrderData);
		}

		[Test]
		public void AddOrderTest()
		{
			var ord = OrdersHelper.AddOrder();

			Console.WriteLine(ord.OrderID);
			Assert.AreEqual(ord.Customer.CompanyName, "Test");
			Assert.AreEqual(ord.Customer.ContactName, "Contact");

			Assert.AreEqual(ord.Employee.LastName, "EmpLastName");
			Assert.AreEqual(ord.Employee.FirstName, "FirstName");

			Assert.AreEqual(ord.Shipper.CompanyName, "Test Shipper");
			Assert.AreEqual(ord.Shipper.Phone, "123-456");

			Assert.AreEqual(ord.Order_Details.Length, 2);
			Assert.AreEqual(ord.Order_Details[0].UnitPrice, 10);
			Assert.AreEqual(ord.Order_Details[1].UnitPrice, 100);

			OrdersHelper.PrintFullOrderInfo(ord);
		}

		[Test]
		public void ProcessOrderTest()
		{
			var service = new OrderServiceClient();

			var order = OrdersHelper.AddOrder();

			var currentDateTime = DateTime.Now;
			var processedOrder = service.SendOrderToProcess(order.OrderID, currentDateTime);
			Assert.True(currentDateTime.ToString() == processedOrder.OrderDate.ToString());

			OrdersHelper.PrintFullOrderInfo(order);
			OrdersHelper.PrintFullOrderInfo(processedOrder);
		}

		[Test]
		public void ShipOrderTest()
		{
			var service = new OrderServiceClient();

			var order = OrdersHelper.AddOrder();
			order = service.SendOrderToProcess(order.OrderID, DateTime.Now);

			var currentDateTime = DateTime.Today.AddDays(7);
			var shippedOrder = service.SendOrderToCustomer(order.OrderID, currentDateTime);
			Assert.NotNull(shippedOrder.ShippedDate);
			Assert.AreEqual(currentDateTime, shippedOrder.ShippedDate);

			OrdersHelper.PrintFullOrderInfo(order);
			OrdersHelper.PrintFullOrderInfo(shippedOrder);
		}

		[Test]
		public void UpdateOrderTest()
		{
			var order = OrdersHelper.AddOrder();
			OrdersHelper.PrintFullOrderInfo(order);

			var products = new ProductServiceClient().GetAllProducts();

			order.ShipAddress = "New Ship Address";
			order.Order_Details = new[]
			{
				new Order_Detail() {Discount = 0, ProductID = products[products.Length -1 >= 0 ? products.Length -1 : 0].ProductID, Quantity = 100, UnitPrice = 3}, 
				new Order_Detail() {Discount = 0, ProductID = products[products.Length -2 >= 0 ? products.Length -2 : 0].ProductID, Quantity = 200, UnitPrice = 2}, 
				new Order_Detail() {Discount = 0, ProductID = products[products.Length -3 >= 0 ? products.Length -3 : 0].ProductID, Quantity = 300, UnitPrice = 1}, 
			};

			var updatedOrder = new OrderServiceClient().UpdateOrder(order);

			Assert.AreEqual(updatedOrder.ShipAddress, "New Ship Address");
			Assert.AreEqual(updatedOrder.Order_Details.Length, 3);

			OrdersHelper.PrintFullOrderInfo(updatedOrder);
		}

		[Test]
		public void DeleteOrderTest()
		{
			var orderForDelete = OrdersHelper.AddOrder();

			var service = new OrderServiceClient();
			service.DeleteOrder(orderForDelete.OrderID);

			Assert.Catch(typeof(FaultException<OrderFault>), () => service.DeleteOrder(orderForDelete.OrderID));
		}
	}
}
