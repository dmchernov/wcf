using System.Collections.Generic;
using System.ServiceModel;
using NorthwindServiceLibrary.Contracts;

namespace NorthwindServiceLibrary.Services
{
	class OrderSubscription// : IOrderSubscription
	{
		static List<IOrderSubscription> callBacks = new List<IOrderSubscription>();

		public void Subscribe ()
		{
			var callback = OperationContext.Current.GetCallbackChannel<IOrderSubscription>();
			if (!callBacks.Contains(callback))
				callBacks.Add(callback);
		}

		public void UnSubscribe ()
		{
			var callback = OperationContext.Current.GetCallbackChannel<IOrderSubscription>();
			if (callBacks.Contains(callback))
				callBacks.Remove(callback);
		}
	}
}
