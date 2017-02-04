using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NorthwindModel.Models.CustomModels
{
	[DataContract]
	[KnownType(typeof(Category))]
	public class BasicCategory
	{
		[DataMember]
		public int CategoryID { get; set; }
		[Required]
		[StringLength(15)]
		[DataMember]
		public string CategoryName { get; set; }
	}
}
