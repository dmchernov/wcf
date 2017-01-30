using System.ServiceModel;

namespace NorthwindServiceLibrary.Subscription
{
	public interface IOrderSubscription
	{
		[OperationContract(IsOneWay = true)]
		// ReSharper disable once OperationContractWithoutServiceContract
		void SendOrderNotification(OrderNotification notification);

		[OperationContract(IsOneWay = true)]
		void SendServiceData (SubscriptionServiceData data);
	}
}
