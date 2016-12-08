using System;
using System.Linq;
using NUnit.Framework;
using TestProject.OrderService;

namespace TestProject
{
	public class UnitTest1
	{
		private Order _order;
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
		public void AddOrderTest()
		{
			var ord = AddOrder();

			Console.WriteLine(ord.OrderID);
			Assert.AreEqual(ord.Customer.CompanyName, "Test");
			Assert.AreEqual(ord.Customer.ContactName, "Contact");

			Assert.AreEqual(ord.Employee.LastName, "EmpLastName");
			Assert.AreEqual(ord.Employee.FirstName, "FirstName");

			Assert.AreEqual(ord.Shipper.CompanyName, "Test Shipper");
			Assert.AreEqual(ord.Shipper.Phone, "123-456");
		}
		public Order AddOrder ()
		{
			var service = new OrderServiceClient();

			_order = new Order()
			{
				Customer = new Customer() { CompanyName = "Test", ContactName = "Contact", CustomerID = new Random().Next(1, 99999).ToString(), ContactTitle = "Title"},
				Employee = new Employee() { FirstName = "FirstName", LastName = "EmpLastName" },
				Shipper = new Shipper() { CompanyName = "Test Shipper", Phone = "123-456" },
				ShipAddress = "Address",
				ShipCity = "City",
				ShipCountry = "Country"
			};

			return service.Add(_order);
		}

		[Test]
		public void ProcessOrderTest()
		{
			var service = new OrderServiceClient();

			_order = AddOrder();

			var currentDateTime = DateTime.Now;
			var processedOrder = service.SendOrderToProcess(_order.OrderID, currentDateTime);
			Assert.AreEqual(currentDateTime, processedOrder.OrderDate);
		}

		[Test]
		public void ShipOrderTest()
		{
			var service = new OrderServiceClient();

			var order = AddOrder();
			order = service.SendOrderToProcess(order.OrderID, DateTime.Now);

			var currentDateTime = DateTime.Today.AddDays(7);
			var shippedOrder = service.SendOrderToCustomer(order.OrderID, currentDateTime);
			Assert.AreEqual(currentDateTime, shippedOrder.ShippedDate.Value.Date);
		}
	}
}
