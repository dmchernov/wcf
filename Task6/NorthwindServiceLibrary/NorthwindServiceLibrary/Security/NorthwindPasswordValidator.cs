using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Linq;

namespace NorthwindServiceLibrary.Security
{
	public class NorthwindPasswordValidator : UserNamePasswordValidator
	{
		public override void Validate(string userName, string password)
		{
			if (userName == "Guest" && String.IsNullOrEmpty(password))
				return;

			using (var userStore = new NorthwindUsers.NorthwindUsers())
			{
				//1793703675 - Хэш для IIS
				//1849190365 - Хэш для WindowsService
				var hash = password.GetHashCode().ToString();
				var user = userStore.Users.SingleOrDefault(u => u.UserName == userName);
				if (user != null && user.Password.Split('|').Contains(hash))
					return;

				throw new SecurityTokenValidationException();
			}
		}
	}
}
