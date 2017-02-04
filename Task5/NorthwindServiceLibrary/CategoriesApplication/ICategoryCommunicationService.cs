using System.ServiceModel;
using NorthwindService.Contracts.ServiceContracts;

namespace CategoriesApplication
{
	interface ICategoryCommunicationService : ICategoryService, IClientChannel
	{
	}
}
