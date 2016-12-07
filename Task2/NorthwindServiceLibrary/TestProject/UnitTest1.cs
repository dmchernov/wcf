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
			var service = new NorthwindServiceClient();

			foreach (var order in service.GetOrders())
			{
				Console.WriteLine(order.OrderID);
			}
		}

		[Test]
		public void GetOrderExTest ()
		{
			var service = new NorthwindServiceClient();

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
			var service = new NorthwindServiceClient();

			var newOrder = new Order()
			{
				Customer = new Customer() { CompanyName = "Test", ContactName = "Contact", CustomerID = new Random().Next(1, 99999).ToString(), ContactTitle = "Title"},
				Employee = new Employee() { FirstName = "EmpFirstName", LastName = "EmpLastName" },
				Shipper = new Shipper() { CompanyName = "Test Shipper", Phone = "123-456"},
				ShipAddress = "Address",
				ShipCity = "City",
				ShipCountry = "Country"
			};

			var ord = service.Add(newOrder);
			Console.WriteLine(ord.OrderID);
			Assert.AreEqual(ord.Customer.CompanyName, "Test");
			Assert.AreEqual(ord.Customer.ContactName, "Contact");
			Assert.AreEqual(ord.Shipper.CompanyName, "Test Shipper");
			Assert.AreEqual(ord.Shipper.Phone, "123-456");
		}
	}
}
