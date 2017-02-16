using System.Runtime.Serialization;

namespace NorthwindService.Contracts.FaultContracts.CategoryFaults
{
	[DataContract]
	public class CategoryNotFound
	{
		[DataMember]
		public int CategoryId { get; set; }
	}
}