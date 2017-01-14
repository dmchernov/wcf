using System.Runtime.Serialization;

namespace NorthwindServiceLibrary.Faults.CategoryFaults
{
	[DataContract]
	public class CategoryNotFound
	{
		[DataMember]
		public int CategoryId { get; set; }
	}
}