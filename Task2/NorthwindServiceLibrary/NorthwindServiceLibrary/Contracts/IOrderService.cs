using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using NorthwindModel.Models;
using NorthwindModel.Models.CustomModels;

namespace NorthwindServiceLibrary.Contracts
{
	[ServiceContract]
	public interface IOrderService
	{
		[OperationContract]
		IList<BasicOrder> GetOrders ();

		[OperationContract]
		Order GetOrderEx (int orderId);

		[OperationContract]
		Order Add (Order newOrder);

		[OperationContract]
		Order SendOrderToProcess (int orderId, DateTime orderDate);

		[OperationContract]
		Order SendOrderToCustomer (int orderId, DateTime shippedDate);
	}
}
