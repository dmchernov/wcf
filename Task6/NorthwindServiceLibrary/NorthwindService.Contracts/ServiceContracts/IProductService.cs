using System.Collections.Generic;
using System.ServiceModel;
using NorthwindModel.Models;

namespace NorthwindService.Contracts.ServiceContracts
{
	[ServiceContract]
	public interface IProductService
	{
		[OperationContract]
		IList<Product> GetAllProducts();
	}
}
