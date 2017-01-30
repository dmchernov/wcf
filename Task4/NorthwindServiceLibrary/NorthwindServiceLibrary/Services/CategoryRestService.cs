using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using NorthwindModel;
using NorthwindServiceLibrary.Contracts;

namespace NorthwindServiceLibrary.Services
{
	public class CategoryRestService : ICategoryRestService
	{
		public Message GetImage(string id)
		{
			using (var db = new Northwind())
			{
				var intId = 0;
				if (!int.TryParse(id, out intId))
					throw new WebFaultException(HttpStatusCode.NotFound);

				var category = db.Categories.FirstOrDefault(c => c.CategoryID == intId);
				if (category == null)
					throw new WebFaultException(HttpStatusCode.NotFound);

				return WebOperationContext.Current.CreateStreamResponse(
					new MemoryStream(category.Picture), "image/bmp");
			}
		}

		public void UpdateImage(string id, Stream image)
		{
			var str = new MemoryStream();
			const int bufferLen = 4096;
			byte[] buffer1 = new byte[bufferLen];
			int count;
			while ((count = image.Read(buffer1, 0, bufferLen)) > 0)
			{
				str.Write(buffer1, 0, count);
			}
			var bytes = str.ToArray();
			image.Close();
			str.Close();

			using (var db = new Northwind())
			{
				int intId;
				if (!int.TryParse(id, out intId))
					throw new WebFaultException(HttpStatusCode.NotFound);

				var cat = db.Categories.FirstOrDefault(c => c.CategoryID == intId);
				if (cat == null)
					throw new WebFaultException(HttpStatusCode.NotFound);

				cat.Picture = bytes;
				db.SaveChanges();
			}
		}
	}
}
