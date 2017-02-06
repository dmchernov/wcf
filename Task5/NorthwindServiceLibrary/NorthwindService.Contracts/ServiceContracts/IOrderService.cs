using System;
using System.Collections.Generic;
using System.ServiceModel;
using NorthwindModel.Models;
using NorthwindModel.Models.CustomModels;
using NorthwindService.Contracts.FaultContracts.OrderFaults;
using NorthwindService.Contracts.Subscription;

namespace NorthwindService.Contracts.ServiceContracts
{
	[ServiceContract(CallbackContract = typeof(IOrderSubscription))]
	public interface IOrderService
	{
		[OperationContract]
		IList<BasicOrder> GetOrders ();

		[OperationContract]
		IList<BasicOrder> GetOrdersWithError ();

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
