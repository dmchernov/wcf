using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;

namespace NorthwindServiceLibrary.Contracts
{
	[ServiceContract]
	public interface ICategoryRestService
	{
		[WebGet(UriTemplate = "/categories/{id}/image")]
		Message GetImage(string id);

		[WebInvoke(UriTemplate = "/categories/{id}/image", Method = "PUT")]
		void UpdateImage (string id, Stream image);
	}
}
