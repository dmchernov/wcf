using System.Collections.Generic;
using System.ServiceModel;
using NorthwindModel.Models;

namespace NorthwindServiceLibrary.Contracts
{
	[ServiceContract]
	public interface IProductService
	{
		[OperationContract]
		IList<Product> GetAllProducts();
	}
}
