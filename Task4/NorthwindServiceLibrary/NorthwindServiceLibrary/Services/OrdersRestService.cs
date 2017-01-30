using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using NorthwindModel;
using NorthwindModel.Enums;
using NorthwindModel.Extensions;
using NorthwindModel.Models;
using NorthwindModel.Models.CustomModels;
using NorthwindServiceLibrary.Contracts;
using NorthwindServiceLibrary.Faults.OrderFaults;

namespace NorthwindServiceLibrary.Services
{
	public class OrdersRestService : IOrderRestService
	{
		public IList<BasicOrder> GetOrders()
		{
			using (var db = new Northwind())
			{
				db.Configuration.ProxyCreationEnabled = false;
				db.Configuration.LazyLoadingEnabled = false;
				var orders = db.Orders.ToBasicOrdersList();

				return orders;
			}
		}

		public Order GetOrderEx(string Id)
		{
			using (var db = new Northwind())
			{
				db.Configuration.ProxyCreationEnabled = false;
				db.Configuration.LazyLoadingEnabled = false;

				int intId;
				if (!int.TryParse(Id, out intId))
					throw new WebFaultException(HttpStatusCode.BadRequest);

				var order = db.Orders.FirstOrDefault(o => o.OrderID == intId);
				if (order == null)
					throw new WebFaultException<string>($"Order with number {Id} not found", HttpStatusCode.NotFound);

				var context = (db as IObjectContextAdapter).ObjectContext;
				context.LoadProperty(order, p => p.Order_Details);
				context.LoadProperty(order, p => p.Customer);
				context.LoadProperty(order, p => p.Employee);
				context.LoadProperty(order, p => p.Shipper);

				foreach (var od in order.Order_Details)
				{
					context.LoadProperty(od, p => p.Product);
					context.LoadProperty(od.Product, p => p.Category);
				}
				return order;
			}
		}

		public Order Add(Order newOrder)
		{
			using (var db = new Northwind())
			{
				var result = db.Orders.Add(newOrder);
				db.SaveChanges();
				return GetOrderEx(result.OrderID.ToString());
			}
		}

		public Order SendOrderToProcess(int orderId, DateTime orderDate)
		{
			if (orderDate < DateTime.Today)
				throw new FaultException<InvalidOrderDate>(new InvalidOrderDate {OrderId = orderId});

			using (var db = new Northwind())
			{
				db.Configuration.ProxyCreationEnabled = false;

				var order = db.Orders.First(o => o.OrderID == orderId);
				if (order.Status != OrderStatus.New)
					throw new WebFaultException<string>($"Order №{orderId} is not in required statuses. Required statuses is: {OrderStatus.New}", HttpStatusCode.BadRequest);

				order.OrderDate = orderDate;
				db.SaveChanges();

				return GetOrderEx(order.OrderID.ToString());
			}
		}

		public Order SendOrderToCustomer(int orderId, DateTime shippedDate)
		{
			using (var db = new Northwind())
			{
				db.Configuration.ProxyCreationEnabled = false;
				var order = db.Orders.First(o => o.OrderID == orderId);
				if (order.Status != OrderStatus.InProgress)
					throw new WebFaultException<string>($"Order №{orderId} is not in required statuses. Required statuses is: {OrderStatus.InProgress}", HttpStatusCode.BadRequest);

				order.ShippedDate = shippedDate;
				db.SaveChanges();

				return GetOrderEx(order.OrderID.ToString());
			}
		}

		public Order UpdateOrder(Order orderForUpdate)
		{
			using (var db = new Northwind())
			{
				db.Configuration.ProxyCreationEnabled = false;

				var oldOrder = db.Orders.FirstOrDefault(o => o.OrderID == orderForUpdate.OrderID);
				if (oldOrder == null)
					throw new WebFaultException<string>($"Order with number {orderForUpdate.OrderID} not found", HttpStatusCode.NotFound);

				if (oldOrder.Status != OrderStatus.New)
					throw new WebFaultException<string>($"Order №{orderForUpdate.OrderID} is not in required statuses. Required statuses is: {OrderStatus.New}", HttpStatusCode.BadRequest);

				oldOrder.Customer = db.Customers.Find(orderForUpdate.Customer?.CustomerID) ?? orderForUpdate.Customer;
				oldOrder.Employee = db.Employees.Find(orderForUpdate.Employee?.EmployeeID) ?? orderForUpdate.Employee;
				oldOrder.ShipAddress = orderForUpdate.ShipAddress;

				var oldDetails = db.Order_Details.Where(od => od.OrderID == orderForUpdate.OrderID);

				db.Order_Details.RemoveRange(oldDetails);

				oldOrder.Order_Details = orderForUpdate.Order_Details;
				db.SaveChanges();

				return GetOrderEx(oldOrder.OrderID.ToString());
			}
		}

		public void DeleteOrder(int orderId)
		{
			using (var db = new Northwind())
			{
				db.Configuration.ProxyCreationEnabled = false;

				var orderForDelete = db.Orders.FirstOrDefault(o => o.OrderID == orderId);
				if (orderForDelete == null)
					throw new WebFaultException<string>($"Order with number {orderId} not found", HttpStatusCode.NotFound);

				if (orderForDelete.Status == OrderStatus.Complete)
					throw new WebFaultException<string>($"Order №{orderId} is not in required statuses. Required statuses is: {OrderStatus.New}, {OrderStatus.InProgress}", HttpStatusCode.BadRequest);

				var detailsForDelete = db.Order_Details.Where(od => od.OrderID == orderId);
				db.Order_Details.RemoveRange(detailsForDelete);
				db.Orders.Remove(orderForDelete);
				db.SaveChanges();
			}
		}
	}
}