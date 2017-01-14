using System;
using System.Collections.Generic;
using System.ServiceModel;
using NorthwindModel.Models;
using NorthwindModel.Models.CustomModels;
using NorthwindServiceLibrary.Faults.OrderFaults;
using NorthwindServiceLibrary.Subscription;

namespace NorthwindServiceLibrary.Contracts
{
	[ServiceContract(CallbackContract = typeof(IOrderSubscription))]
	public interface IOrderService
	{
		[OperationContract]
		IList<BasicOrder> GetOrders ();

		[OperationContract]
		[FaultContract(typeof(OrderNotFound))]
		Order GetOrderEx (int orderId);

		[OperationContract]
		Order Add (Order newOrder);

		[OperationContract]
		[FaultContract(typeof(OrderNotFound))]
		[FaultContract(typeof(OrderNotInRequiredStatuses))]
		[FaultContract(typeof(InvalidOrderDate))]
		Order SendOrderToProcess (int orderId, DateTime orderDate);

		[OperationContract]
		[FaultContract(typeof(OrderNotFound))]
		[FaultContract(typeof(OrderNotInRequiredStatuses))]
		Order SendOrderToCustomer (int orderId, DateTime shippedDate);

		[OperationContract]
		[FaultContract(typeof(OrderNotFound))]
		[FaultContract(typeof(OrderNotInRequiredStatuses))]
		Order UpdateOrder (Order orderForUpdate);

		[OperationContract]
		[FaultContract(typeof(OrderNotFound))]
		[FaultContract(typeof(OrderNotInRequiredStatuses))]
		void DeleteOrder (int orderId);

		[OperationContract(IsOneWay = true)]
		void Subscribe();

		[OperationContract(IsOneWay = true)]
		void UnSubscribe();
	}
}
