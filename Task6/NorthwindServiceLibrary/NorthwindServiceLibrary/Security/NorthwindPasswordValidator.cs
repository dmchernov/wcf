using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Text;

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
				var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
				var user = userStore.Users.SingleOrDefault(u => u.UserName == userName);
				if (user != null && user.Password == base64)
					return;

				throw new SecurityTokenValidationException();
			}
		}
	}
}
