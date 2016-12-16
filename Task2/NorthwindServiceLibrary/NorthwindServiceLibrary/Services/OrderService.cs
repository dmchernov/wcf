using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.ServiceModel;
using NorthwindModel;
using NorthwindModel.Enums;
using NorthwindModel.Extensions;
using NorthwindModel.Models;
using NorthwindModel.Models.CustomModels;
using NorthwindServiceLibrary.Contracts;
using NorthwindServiceLibrary.Faults;

namespace NorthwindServiceLibrary.Services
{
	public class OrderService : IOrderService
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

		public Order GetOrderEx (int orderId)
		{
			using (var db = new Northwind())
			{
				db.Configuration.ProxyCreationEnabled = false;
				db.Configuration.LazyLoadingEnabled = false;
				var order = db.Orders.FirstOrDefault(o => o.OrderID == orderId);
				if (order == null)
					throw new FaultException<OrderFault>(new OrderFault() {Message = $"Заказ с указанным номером не найден.", OrderId = orderId});

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
				return GetOrderEx(result.OrderID);
			}
		}

		public Order SendOrderToProcess(int orderId, DateTime orderDate)
		{
			if (orderDate < DateTime.Today)
				throw new FaultException<OrderFault>(new OrderFault() { Message = "Невозможно отправить заказ задним числом.", OrderId = orderId});

			using (var db = new Northwind())
			{
				db.Configuration.ProxyCreationEnabled = false;
				
				var order = db.Orders.First(o => o.OrderID == orderId);
				if (order.Status != OrderStatus.New)
					throw new FaultException<OrderFault>(new OrderFault()
					{
						Message = "Заказ уже находится в обработке или был отправлен покупателю.",
						OrderId = orderId,
						Status = order.Status
					});

				order.OrderDate = orderDate;
				db.SaveChanges();

				return GetOrderEx(order.OrderID);
			}
		}

		public Order SendOrderToCustomer(int orderId, DateTime shippedDate)
		{
			using (var db = new Northwind())
			{
				db.Configuration.ProxyCreationEnabled = false;
				var order = db.Orders.First(o => o.OrderID == orderId);
				if (order.Status != OrderStatus.InProgress)
					throw new FaultException<OrderFault>(new OrderFault()
					{
						Message = "Невозможно отправить заказ, не находящийся в обработке.",
						OrderId = orderId
					});

				order.ShippedDate = shippedDate;
				db.SaveChanges();

				return GetOrderEx(order.OrderID);
			}
		}

		public Order UpdateOrder(Order orderForUpdate)
		{
			using (var db = new Northwind())
			{
				db.Configuration.ProxyCreationEnabled = false;

				var oldOrder = db.Orders.FirstOrDefault(o => o.OrderID == orderForUpdate.OrderID);
				if (oldOrder == null)
					throw new FaultException<OrderFault>(new OrderFault() {Message = "Заказ для обновления не найден.", OrderId = orderForUpdate.OrderID});

				if (oldOrder.Status != OrderStatus.New)
					throw new FaultException<OrderFault>(new OrderFault() {Message = "Нельзя изменить отправленный или находящийся в обработке заказ.", OrderId = orderForUpdate.OrderID, Status = oldOrder.Status});

				oldOrder.Customer = db.Customers.Find(orderForUpdate.Customer?.CustomerID) ?? orderForUpdate.Customer;
				oldOrder.Employee = db.Employees.Find(orderForUpdate.Employee?.EmployeeID) ?? orderForUpdate.Employee;
				oldOrder.ShipAddress = orderForUpdate.ShipAddress;

				var oldDetails = db.Order_Details.Where(od => od.OrderID == orderForUpdate.OrderID);

				db.Order_Details.RemoveRange(oldDetails);

				oldOrder.Order_Details = orderForUpdate.Order_Details;
				db.SaveChanges();

				return GetOrderEx(oldOrder.OrderID);
			}
		}

		public void DeleteOrder(int orderId)
		{
			using (var db = new Northwind())
			{
				db.Configuration.ProxyCreationEnabled = false;

				var orderForDelete = db.Orders.FirstOrDefault(o => o.OrderID == orderId);
				if (orderForDelete == null)
					throw new FaultException<OrderFault>(new OrderFault() {Message = "Заказ с указанным номером не зарегистрирован.", OrderId = orderId});

				if (orderForDelete.Status == OrderStatus.Complete)
					throw new FaultException<OrderFault>(new OrderFault() {Message = "Невозможно удалить отправленный заказ.", Status = orderForDelete.Status, OrderId = orderForDelete.OrderID});

				var detailsForDelete = db.Order_Details.Where(od => od.OrderID == orderId);
				db.Order_Details.RemoveRange(detailsForDelete);
				db.Orders.Remove(orderForDelete);
				db.SaveChanges();
			}
		}
	}
}