using System.Collections.Generic;
using System.ServiceModel;
using NorthwindModel.Models;
using NorthwindModel.Models.CustomModels;

namespace NorthwindServiceLibrary.Contracts
{
	[ServiceContract(Namespace = "http://epam.com/dmitrii_chernov/northwind")]
	public interface INorthwindService
	{
		[OperationContract]
		IList<BasicOrder> GetOrders();

		[OperationContract]
		Order GetOrderEx(int orderId);

		[OperationContract]
		Order Add(Order newOrder);

		[OperationContract]
		int TestContract();
	}
}
