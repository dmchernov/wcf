using System.Collections.Generic;
using System.Threading.Tasks;
using NorthwindModel.Models;
using NorthwindModel.Models.CustomModels;

namespace NorthwindModel.Extensions
{
	public static class OrderExtensions
	{
		public static IList<BasicOrder> ToBasicOrdersList(this IEnumerable<Order> orders)
		{
			var result = new List<BasicOrder>();

			Parallel.ForEach(orders, order =>
			{
				result.Add(new BasicOrder {OrderID = order.OrderID, OrderDate = order.OrderDate, ShipAddress = order.ShipAddress, ShipCity = order.ShipCity, ShippedDate = order.ShippedDate, ShipRegion = order.ShipRegion});
			});

			return result;
		}
	}
}
