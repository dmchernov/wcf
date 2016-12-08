using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using NorthwindModel;
using NorthwindModel.Extensions;
using NorthwindModel.Models;
using NorthwindModel.Models.CustomModels;
using NorthwindServiceLibrary.Contracts;
using NorthwindServiceLibrary.Faults;

namespace NorthwindServiceLibrary.Services
{
	public class OrderService : IOrderService
	{
		public IList<BasicOrder> GetOrders ()
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
				if (order != null)
				{
					var context = (db as IObjectContextAdapter).ObjectContext;
					context.LoadProperty(order, p => p.Order_Details);
					context.LoadProperty(order, p => p.Customer);
					context.LoadProperty(order, p => p.Employee);
					context.LoadProperty(order, p => p.Shipper);
					foreach (var od in order.Order_Details)
					{
						context.LoadProperty(od, p => p.Product);
					}
				}
				return order;
			}
		}

		public Order Add (Order newOrder)
		{
			using (var db = new Northwind())
			{
				if (newOrder.Customer != null)
				{
					if (db.Customers.Find(newOrder.Customer.CustomerID) == null)
						newOrder.Customer = db.Customers.Add(newOrder.Customer);
					else
						newOrder.Customer = db.Customers.Find(newOrder.Customer.CustomerID);
				}
				if (newOrder.Employee != null)
				{
					newOrder.Employee = db.Employees.Add(newOrder.Employee);
				}
				if (newOrder.Shipper != null)
				{
					newOrder.Shipper = db.Shippers.Add(newOrder.Shipper);
				}
				var result = db.Orders.Add(newOrder);
				db.SaveChanges();
				return GetOrderEx(result.OrderID);
			}
		}

		public Order SendOrderToProcess(int orderId, DateTime orderDate)
		{
			using (var db = new Northwind())
			{
				db.Configuration.ProxyCreationEnabled = false;
				try
				{
					var order = db.Orders.First(o => o.OrderID == orderId);
					if (order.OrderDate != null)
						throw new FaultException<OrderFault>(new OrderFault()
						{
							Message = "Заказ уже находится в обработке",
							OrderId = orderId
						});

					order.OrderDate = orderDate;
					db.SaveChanges();

					return order;
				}
				catch (FaultException<OrderFault>)
				{
					throw;
				}
				catch (Exception e)
				{
					throw new FaultException<OrderFault>(new OrderFault() {InnerException = e, Message = "Во время обновления данных произошла ошибка", OrderId = orderId});
				}
			}
		}

		public Order SendOrderToCustomer(int orderId, DateTime shippedDate)
		{
			using (var db = new Northwind())
			{
				db.Configuration.ProxyCreationEnabled = false;
				try
				{
					var order = db.Orders.First(o => o.OrderID == orderId);
					if (order.ShippedDate != null)
						throw new FaultException<OrderFault>(new OrderFault() { Message = "Заказ уже отправлен покупателю", OrderId = orderId });

					order.ShippedDate = shippedDate;
					db.SaveChanges();

					return order;
				}
				catch (Exception e)
				{
					throw new FaultException<OrderFault>(new OrderFault() { InnerException = e, Message = "Во время обновления данных произошла ошибка", OrderId = orderId });
				}
			}
		}
	}
}
