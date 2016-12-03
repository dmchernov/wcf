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
				db.Configuration.LazyLoadingEnabled = true;
				var orders = db.Orders.ToList();
				orders.RemoveRange(1, 820);

				var t = (db as IObjectContextAdapter).ObjectContext;
				foreach (var o in orders)
				{
					t.LoadProperty(o, p => p.Employee);
					t.LoadProperty(o, p => p.Customer);
					t.LoadProperty(o, p => p.Order_Details);
					foreach (var orderDetail in o.Order_Details)
					{
						t.LoadProperty(orderDetail, p => p.Product);
						t.LoadProperty(orderDetail.Product, p => p.Category);
						t.LoadProperty(orderDetail.Product, p => p.Supplier);
					}
				}

				return orders;
			}
		}
	}
}
