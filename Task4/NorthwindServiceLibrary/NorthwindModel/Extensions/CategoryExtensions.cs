using System.Collections.Generic;
using System.Linq;
using NorthwindModel.Models;
using NorthwindModel.Models.CustomModels;

namespace NorthwindModel.Extensions
{
	public static class CategoryExtensions
	{
		public static IList<BasicCategory> ToBaseCategories(this IEnumerable<Category> categories)
		{
			var result = new List<BasicCategory>();

			categories.ToList().ForEach(category =>
			{
				result.Add(new BasicCategory {CategoryID = category.CategoryID, CategoryName = category.CategoryName});
			});

			return result;
		}
	}
}
