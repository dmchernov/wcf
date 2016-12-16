using System.Runtime.Serialization;

namespace NorthwindServiceLibrary.Services
{
	[DataContract]
	public class CategoryFault
	{
		[DataMember]
		public string Message { get; set; }
		[DataMember]
		public int CategoryId { get; set; }
	}
}