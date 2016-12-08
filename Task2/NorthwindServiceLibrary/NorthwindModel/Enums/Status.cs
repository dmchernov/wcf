using System.Runtime.Serialization;

namespace NorthwindModel.Enums
{
	[DataContract]
	public enum Status
	{
		[EnumMember]
		None = 0,
		[EnumMember]
		New = 1,
		[EnumMember]
		InProgress = 2,
		[EnumMember]
		Complete = 3
	}
}
