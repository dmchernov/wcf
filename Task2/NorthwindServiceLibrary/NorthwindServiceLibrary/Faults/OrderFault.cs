using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindServiceLibrary.Faults
{
	[DataContract]
	public class OrderFault
	{
		[DataMember]
		public string Message { get; set; }

		[DataMember]
		public int OrderId { get; set; }

		[DataMember]
		public Exception InnerException { get; set; }
	}
}
