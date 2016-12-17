using System;
using System.Collections.Generic;
using System.ServiceModel;
using NorthwindModel.Models;
using NorthwindModel.Models.CustomModels;
using NorthwindServiceLibrary.Faults;

namespace NorthwindServiceLibrary.Contracts
{
	[ServiceContract(CallbackContract = typeof(IOrderSubscription))]
	public interface IOrderService
	{
		[OperationContract]
		IList<BasicOrder> GetOrders ();

		[OperationContract]
		[FaultContract(typeof(OrderFault))]
		Order GetOrderEx (int orderId);

		[OperationContract]
		Order Add (Order newOrder);

		[OperationContract]
		[FaultContract(typeof(OrderFault))]
		Order SendOrderToProcess (int orderId, DateTime orderDate);

		[OperationContract]
		[FaultContract(typeof(OrderFault))]
		Order SendOrderToCustomer (int orderId, DateTime shippedDate);

		[OperationContract]
		[FaultContract(typeof(OrderFault))]
		Order UpdateOrder (Order orderForUpdate);

		[OperationContract]
		[FaultContract(typeof(OrderFault))]
		void DeleteOrder (int orderId);

		[OperationContract(IsOneWay = true)]
		void Subscribe();

		[OperationContract(IsOneWay = true)]
		void UnSubscribe();
	}
}
