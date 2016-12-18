using System.ServiceModel;

namespace NorthwindServiceLibrary.Contracts
{
	public interface IOrderSubscription
	{
		[OperationContract(IsOneWay = true)]
		// ReSharper disable once OperationContractWithoutServiceContract
		void SendInformationMessage(string message);
	}
}
