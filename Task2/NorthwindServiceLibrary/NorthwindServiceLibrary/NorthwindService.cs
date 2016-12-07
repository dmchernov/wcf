using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using NorthwindServiceLibrary.Contracts;
using NorthwindServiceLibrary.Models;

namespace NorthwindServiceLibrary
{
	// ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде и файле конфигурации.
	public class NorthwindService : INorthwindService
	{
		public IList<Order> GetOrders()
		{
			using (var db = new Northwind())
			{
				db.Configuration.ProxyCreationEnabled = false;
				db.Configuration.LazyLoadingEnabled = false;
				var orders = db.Orders.ToList();
				//orders.RemoveRange(1, 820);

				/*var t = (db as IObjectContextAdapter).ObjectContext;
				foreach (var o in orders)
				{
					//t.LoadProperty(o, p => p.Employee);
					//t.LoadProperty(o, p => p.Customer);
					t.LoadProperty(o, p => p.Order_Details);
					foreach (var orderDetail in o.Order_Details)
					{
						t.LoadProperty(orderDetail, p => p.Product);
						t.LoadProperty(orderDetail.Product, p => p.Category);
						t.LoadProperty(orderDetail.Product, p => p.Supplier);
					}
				}*/

				return orders;
			}
		}

		public Order GetOrderEx(int orderId)
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

		public Order Add(Order newOrder)
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

		public int TestContract()
		{
			return 1;
		}
	}
}
