using System.ServiceModel;
using NorthwindService.Contracts.ServiceContracts;

namespace TestProject.Contracts
{
	public interface IProductChannelService : IProductService, IClientChannel
	{
	}
}
