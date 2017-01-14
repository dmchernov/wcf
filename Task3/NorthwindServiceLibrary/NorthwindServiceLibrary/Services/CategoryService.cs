using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using NorthwindModel;
using NorthwindModel.Extensions;
using NorthwindModel.Models.CustomModels;
using NorthwindServiceLibrary.Contracts;
using NorthwindServiceLibrary.Faults.CategoryFaults;

namespace NorthwindServiceLibrary.Services
{
	public class CategoryService : ICategoryService
	{
		public CategoryImage GetCategoryImage (CategoryImage image)
		{
			using (var db = new Northwind())
			{
				var id = image.CategoryId;
				var cat = db.Categories.FirstOrDefault(c => c.CategoryID == id);
				if (cat == null)
					throw new FaultException<CategoryNotFound>(new CategoryNotFound {CategoryId = id});
				
				var imageStream = new MemoryStream(cat.Picture);
				return new CategoryImage
				{
					CategoryId = id,
					ImageStream = imageStream,
					Size = cat.Picture.Length
				};
			}
		}

		public void SetCategoryImage(CategoryImage image)
		{
			MemoryStream targetMemoryStream;
			Stream sourceStream = image.ImageStream;

			var imageCat = new byte[image.Size];

			using (targetMemoryStream = new MemoryStream(imageCat))
			{
				const int bufferLen = 4096;
				byte[] buffer1 = new byte[bufferLen];
				int count;
				while ((count = sourceStream.Read(buffer1, 0, bufferLen)) > 0)
				{
					targetMemoryStream.Write(buffer1, 0, count);
				}
				targetMemoryStream.Close();
				sourceStream.Close();
			}

			using (var db = new Northwind())
			{
				var cat = db.Categories.FirstOrDefault(c => c.CategoryID == image.CategoryId);
				if (cat == null)
					throw new FaultException<CategoryNotFound>(new CategoryNotFound {CategoryId = image.CategoryId});

				cat.Picture = imageCat;

				db.SaveChanges();
			}
		}

		public IList<BasicCategory> GetCategories()
		{
			using (var db = new Northwind())
			{
				return db.Categories.ToBaseCategories();
			}
		}
	}
}
