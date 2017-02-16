using System.ServiceModel;
using NorthwindService.Contracts.ServiceContracts;

namespace CallBackClient
{
	interface IOrderChannelService : IOrderService, IClientChannel
	{
	}
}
