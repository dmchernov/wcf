using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using NorthwindModel.Models;
using NorthwindService.Contracts.Subscription;
using NUnit.Framework;
using TestProject.Common;

namespace TestProject.OrderServiceTests
{
	public class OrderServiceTest : IOrderSubscription
	{
		//TODO: Тест, демонстрирующий обработку Unhandled exception
		[Test]
		public void GetOrdersTest ()
		{
			using (var service = ChannelFactoryHelper.GetOrdersFactory(new InstanceContext(this)).CreateChannel())
			{
				try
				{
					foreach (var order in service.GetOrders())
					{
						Console.WriteLine(order.OrderID);
					}
				}
				catch
				{
					if (service.State == CommunicationState.Faulted)
						service.Abort();
				}
			}
		}
		
		//TODO: Тест, демонстрирующий обработку Unhandled exception
		[Test]
		public void GetOrdersWithExceptionTest ()
		{
			using (var service = ChannelFactoryHelper.GetOrdersFactory(new InstanceContext(this)).CreateChannel())
			{
				try
				{
					foreach (var order in service.GetOrdersWithError())
					{
						Console.WriteLine(order.OrderID);
					}
				}
				catch
				{
					if (service.State == CommunicationState.Faulted)
						service.Abort();
				}
			}
		}

		[Test]
		public void GetOrderEx ()
		{
			Order fullOrderData = null;
			var service = ChannelFactoryHelper.GetOrdersFactory(new InstanceContext(this)).CreateChannel();
			
			try
			{
				var orderId = service.GetOrders().First().OrderID;
				fullOrderData = service.GetOrderEx(orderId);
			}
			catch
			{
				if (service.State == CommunicationState.Faulted)
					service.Abort();
			}

			service.Close();

			Assert.NotNull(fullOrderData);
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

			Assert.AreEqual(ord.Order_Details.Count, 2);
			Assert.AreEqual(ord.Order_Details.ToList()[0].UnitPrice, 10);
			Assert.AreEqual(ord.Order_Details.ToList()[1].UnitPrice, 100);

			OrdersHelper.PrintFullOrderInfo(ord);
		}

		[Test]
		public void ProcessOrderTest()
		{
			using (var service = ChannelFactoryHelper.GetOrdersFactory(new InstanceContext(this)).CreateChannel())
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
			using (var service = ChannelFactoryHelper.GetOrdersFactory(new InstanceContext(this)).CreateChannel())
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

			IList<Product> products;
			using (var productService = ChannelFactoryHelper.ProductsFactory.CreateChannel())
			{
				products = productService.GetAllProducts();
			}

			order.ShipAddress = "New Ship Address";
			order.Order_Details = new[]
			{
				new Order_Detail {Discount = 0, ProductID = products[products.Count -1 >= 0 ? products.Count -1 : 0].ProductID, Quantity = 100, UnitPrice = 3}, 
				new Order_Detail {Discount = 0, ProductID = products[products.Count -2 >= 0 ? products.Count -2 : 0].ProductID, Quantity = 200, UnitPrice = 2}, 
				new Order_Detail {Discount = 0, ProductID = products[products.Count -3 >= 0 ? products.Count -3 : 0].ProductID, Quantity = 300, UnitPrice = 1} 
			};

			Order updatedOrder;
			using (var service = ChannelFactoryHelper.GetOrdersFactory(new InstanceContext(this)).CreateChannel())
			{
				updatedOrder = service.UpdateOrder(order);
			}

			Assert.AreEqual(updatedOrder.ShipAddress, "New Ship Address");
			Assert.AreEqual(updatedOrder.Order_Details.Count, 3);

			OrdersHelper.PrintFullOrderInfo(updatedOrder);
		}

		[Test]
		public void DeleteOrderTest()
		{
			var orderForDelete = new OrdersHelper().AddOrder();

			using (var service = ChannelFactoryHelper.GetOrdersFactory(new InstanceContext(this)).CreateChannel())
			{
				service.DeleteOrder(orderForDelete.OrderID);
			}
		}

		public void SendOrderNotification(OrderNotification notification) {}

		public void SendServiceData(SubscriptionServiceData data) {}
	}
}
