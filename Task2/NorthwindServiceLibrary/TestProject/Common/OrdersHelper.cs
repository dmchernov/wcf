using System;
using System.Linq;
using System.ServiceModel;
using TestProject.OrderService;
using TestProject.ProductService;

namespace TestProject.Common
{
	public class OrdersHelper : IOrderServiceCallback
	{
		public Order AddOrder ()
		{
			var service = new OrderServiceClient(new InstanceContext(this));

			var order = new Order()
			{
				Customer = new Customer() { CompanyName = "Test", ContactName = "Contact", CustomerID = new Random().Next(1, 99999).ToString(), ContactTitle = "Title" },
				Employee = new Employee() { FirstName = "FirstName", LastName = "EmpLastName" },
				Shipper = new Shipper() { CompanyName = "Test Shipper", Phone = "123-456" },
				ShipAddress = "Address",
				ShipCity = "City",
				ShipCountry = "Country"
			};

			var productService = new ProductServiceClient();
			var products = productService.GetAllProducts();

			Order_Detail[] details =
			{
				new Order_Detail() {Discount = 0, Quantity = 5, UnitPrice = 10, ProductID = products[0].ProductID},
				new Order_Detail() {Discount = 0, Quantity = 1, UnitPrice = 100, ProductID = products[1].ProductID},
			};

			order.Order_Details = details;

			return service.Add(order);
		}

		public static void PrintFullOrderInfo(Order order)
		{
			Console.WriteLine("---------------------------------------------------------------------------------");
			Console.WriteLine($"OrderID: {order.OrderID}");
			Console.WriteLine($"OrderDate: {order.OrderDate}");
			Console.WriteLine($"ShippedDate: {order.ShippedDate}");
			Console.WriteLine($"Freight: {order.Freight}");
			Console.WriteLine($"RequiredDate: {order.RequiredDate}");
			Console.WriteLine($"ShipName: {order.ShipName}");
			Console.WriteLine($"ShipCountry: {order.ShipCountry}");
			Console.WriteLine($"ShipPostalCode: {order.ShipPostalCode}");
			Console.WriteLine($"ShipRegion: {order.ShipRegion}");
			Console.WriteLine($"ShipCity: {order.ShipCity}");
			Console.WriteLine($"ShipAddress: {order.ShipAddress}");
			Console.WriteLine();

			if (order.Customer != null)
			{
				Console.WriteLine("Customer:");
				Console.WriteLine($"CustomerID: {order.Customer.CustomerID}\nCompanyName: {order.Customer.CompanyName}\nContactName: {order.Customer.ContactName}\nContactTitle: {order.Customer.ContactTitle}");
				Console.WriteLine();
			}

			if (order.Employee != null)
			{
				Console.WriteLine("Employee:");
				Console.WriteLine($"EmployeeID: {order.Employee.EmployeeID}\nFirstName: {order.Employee.FirstName}\nLastName: {order.Employee.LastName}");
				Console.WriteLine();
			}

			if (order.Shipper != null)
			{
				Console.WriteLine("Shipper:");
				Console.WriteLine($"ShipperID: {order.Shipper.ShipperID}\nCompanyName: {order.Shipper.CompanyName}\nPhone: {order.Shipper.Phone}");
				Console.WriteLine();
			}

			if (order.Order_Details != null && order.Order_Details.Length > 0)
			{
				Console.WriteLine("Details:");
				order.Order_Details.ToList().ForEach(detail =>
				{
					Console.WriteLine($"ProductID: {detail.ProductID} Quantity: {detail.Quantity} UnitPrice: {detail.UnitPrice:C} Discount: {detail.Discount}");
				});
			}
		}

		public void SendInformationMessage(string message)
		{
			Console.WriteLine(message);
		}
	}
}
