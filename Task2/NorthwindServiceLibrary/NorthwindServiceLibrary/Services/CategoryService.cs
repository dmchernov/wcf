using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using NorthwindModel;
using NorthwindModel.Extensions;
using NorthwindModel.Models.CustomModels;
using NorthwindServiceLibrary.Contracts;

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
					throw new FaultException<CategoryFault>(new CategoryFault() {CategoryId = id, Message = "Категория с заданным Id не найдена"});

				return new CategoryImage()
				{
					CategoryId = id,
					ImageStream = new MemoryStream(cat.Picture),
					Size = cat.Picture.Length
				};
			}
		}

		public void SetCategoryImage(CategoryImage image)
		{
			/*MemoryStream file = new MemoryStream();
			image.ImageStream.CopyTo(file);
			var a = (int) file.Length;
			byte[] newImage = new byte[a];
			file.Read(newImage, 0, a);
			image.ImageStream.Close();
			file.Close();*/

			//const int bufSize = 1000000;

			MemoryStream file = new MemoryStream();
			//image.ImageStream.CopyTo(file);

			int readed;
			byte[] buffer = new byte[image.Size];

			readed = image.ImageStream.Read(buffer, 0, image.Size);
			while (readed != 0)
			{
				file.Write(buffer, 0, readed);
				readed = image.ImageStream.Read(buffer, 0, image.Size);
			}

			image.ImageStream.Close();
			file.Close();

			using (var db = new Northwind())
			{
				var cat = db.Categories.FirstOrDefault(c => c.CategoryID == image.CategoryId);
				if (cat == null)
					throw new FaultException<CategoryFault>(new CategoryFault() {CategoryId = image.CategoryId, Message = "Категория с заданным Id не найдена" });

				cat.Picture = buffer;

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
