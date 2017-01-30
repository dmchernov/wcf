using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using NorthwindModel.Models;
using NorthwindModel.Models.CustomModels;
using NorthwindServiceLibrary.Faults.OrderFaults;
using NorthwindServiceLibrary.Subscription;

namespace NorthwindServiceLibrary.Contracts
{
	[ServiceContract]
	public interface IOrderRestService
	{
		[WebGet(UriTemplate = "/orders/")]
		IList<BasicOrder> GetOrders ();

		[WebGet(UriTemplate = "/orders/{Id}")]
		Order GetOrderEx (string Id);

		[WebInvoke(UriTemplate = "/orders/", Method = "POST")]
		Order Add (Order newOrder);

		[WebGet(UriTemplate = "/orders/toprocess?orderid={orderId}&date={orderDate}")]
		Order SendOrderToProcess (int orderId, DateTime orderDate);

		[WebGet(UriTemplate = "/orders/tocustomer?orderid={orderId}&date={shippedDate}")]
		Order SendOrderToCustomer (int orderId, DateTime shippedDate);

		[WebInvoke(UriTemplate = "/orders/", Method = "PUT")]
		Order UpdateOrder (Order orderForUpdate);

		[WebInvoke(UriTemplate = "/orders/order?orderId={orderId}", Method = "DELETE")]
		void DeleteOrder (int orderId);
	}
}
