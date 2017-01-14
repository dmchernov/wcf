using System;
using System.Linq;
using System.ServiceModel;
using NUnit.Framework;
using TestProject.Common;
using TestProject.OrderService;
using TestProject.ProductService;
using Product = TestProject.ProductService.Product;

namespace TestProject.OrderServiceTests
{
	public class OrderServiceTest : IOrderServiceCallback
	{
		[Test]
		public void GetOrdersTest ()
		{
			using (var service = new OrderServiceClient(new InstanceContext(this)))
			{
				foreach (var order in service.GetOrders())
				{
					Console.WriteLine(order.OrderID);
				}
			}
		}

		[Test]
		public void GetOrderExTest ()
		{
			Order fullOrderData;
			using (var service = new OrderServiceClient(new InstanceContext(this)))
			{
				int orderId = service.GetOrders().First().OrderID;
				fullOrderData = service.GetOrderEx(orderId);
			}
			Assert.NotNull(fullOrderData);

			OrdersHelper.PrintFullOrderInfo(fullOrderData);
		}

		[Test]
		public void AddOrderTest()
		{
			var ord = new OrdersHelper().AddOrder();

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
			using (var service = new OrderServiceClient(new InstanceContext(this)))
			{
				service.Subscribe();
				var order = new OrdersHelper().AddOrder();

				var currentDateTime = DateTime.Now;
				var processedOrder = service.SendOrderToProcess(order.OrderID, currentDateTime);
				Assert.True(currentDateTime.ToString() == processedOrder.OrderDate.ToString());
				service.UnSubscribe();

				OrdersHelper.PrintFullOrderInfo(order);
				OrdersHelper.PrintFullOrderInfo(processedOrder);
			}
		}

		[Test]
		public void ShipOrderTest()
		{
			Order order;
			DateTime currentDateTime;
			Order shippedOrder;
			using (var service = new OrderServiceClient(new InstanceContext(this)))
			{
				order = new OrdersHelper().AddOrder();

				service.Subscribe();
				order = service.SendOrderToProcess(order.OrderID, DateTime.Now);
				service.UnSubscribe();

				currentDateTime = DateTime.Today.AddDays(7);
				shippedOrder = service.SendOrderToCustomer(order.OrderID, currentDateTime);
			}
			Assert.NotNull(shippedOrder.ShippedDate);
			Assert.AreEqual(currentDateTime, shippedOrder.ShippedDate);

			OrdersHelper.PrintFullOrderInfo(order);
			OrdersHelper.PrintFullOrderInfo(shippedOrder);
		}

		[Test]
		public void UpdateOrderTest()
		{
			var order = new OrdersHelper().AddOrder();
			OrdersHelper.PrintFullOrderInfo(order);

			Product[] products;
			using (var productService = new ProductServiceClient())
			{
				products = productService.GetAllProducts();
			}

			order.ShipAddress = "New Ship Address";
			order.Order_Details = new[]
			{
				new Order_Detail {Discount = 0, ProductID = products[products.Length -1 >= 0 ? products.Length -1 : 0].ProductID, Quantity = 100, UnitPrice = 3}, 
				new Order_Detail {Discount = 0, ProductID = products[products.Length -2 >= 0 ? products.Length -2 : 0].ProductID, Quantity = 200, UnitPrice = 2}, 
				new Order_Detail {Discount = 0, ProductID = products[products.Length -3 >= 0 ? products.Length -3 : 0].ProductID, Quantity = 300, UnitPrice = 1} 
			};

			Order updatedOrder;
			using (var service = new OrderServiceClient(new InstanceContext(this)))
			{
				updatedOrder = service.UpdateOrder(order);
			}

			Assert.AreEqual(updatedOrder.ShipAddress, "New Ship Address");
			Assert.AreEqual(updatedOrder.Order_Details.Length, 3);

			OrdersHelper.PrintFullOrderInfo(updatedOrder);
		}

		[Test]
		public void DeleteOrderTest()
		{
			var orderForDelete = new OrdersHelper().AddOrder();

			using (var service = new OrderServiceClient(new InstanceContext(this)))
			{
				service.DeleteOrder(orderForDelete.OrderID);
			}
		}

		public void SendOrderNotification(OrderNotification notification) {}

		public void SendServiceData(SubscriptionServiceData data) {}
	}
}
