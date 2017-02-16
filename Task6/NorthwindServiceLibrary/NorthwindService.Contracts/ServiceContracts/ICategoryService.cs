using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using NorthwindModel.Models.CustomModels;
using NorthwindService.Contracts.FaultContracts.CategoryFaults;

namespace NorthwindService.Contracts.ServiceContracts
{
	[ServiceContract]
	public interface ICategoryService
	{
		[OperationContract]
		[FaultContract(typeof(CategoryNotFound))]
		CategoryImage GetCategoryImage (CategoryImage image);

		[OperationContract]
		[FaultContract(typeof(CategoryNotFound))]
		void SetCategoryImage(CategoryImage image);

		[OperationContract]
		IList<BasicCategory> GetCategories();
	}

	[MessageContract]
	public class CategoryImage
	{
		[MessageHeader]
		public int CategoryId { get; set; }
		[MessageHeader]
		public int Size { get; set; }

		[MessageBodyMember]
		public Stream ImageStream { get; set; }
	}
}
