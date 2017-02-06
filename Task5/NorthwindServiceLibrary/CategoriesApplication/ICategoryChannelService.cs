using System.ServiceModel;
using NorthwindService.Contracts.ServiceContracts;

namespace CategoriesApplication
{
	interface ICategoryChannelService : ICategoryService, IClientChannel
	{
	}
}
