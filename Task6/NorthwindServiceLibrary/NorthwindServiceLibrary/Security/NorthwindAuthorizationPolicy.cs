using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace NorthwindServiceLibrary.Security
{
	public class NorthwindAuthorizationPolicy : IAuthorizationPolicy
	{
		public class CustomPrincipal : IPrincipal
		{
			private readonly IList<string> _roles;

			public CustomPrincipal (IIdentity identity, IList<string> roles)
			{
				Identity = identity;
				_roles = roles;
			}

			public IIdentity Identity { get; }

			public bool IsInRole (string role)
			{
				return _roles.Contains(role);
			}
		}

		public string Id => "{B90BF06A-E5B5-4EF5-AB27-F94AE5B056AB}";

		public ClaimSet Issuer => ClaimSet.System;

		public bool Evaluate (EvaluationContext evaluationContext, ref object state)
		{
			var operationContext = OperationContext.Current;
			var authContext = AuthorizationContext.CreateDefaultAuthorizationContext(new List<IAuthorizationPolicy>());
			if (operationContext.EndpointDispatcher.ContractName == ServiceMetadataBehavior.MexContractName &&
				operationContext.EndpointDispatcher.ContractNamespace == "http://schemas.microsoft.com/2006/04/mex" &&
				operationContext.IncomingMessageHeaders.Action == "http://schemas.xmlsoap.org/ws/2004/09/transfer/Get")

			{
				var principal = new GenericPrincipal(new GenericIdentity("MexAccount"), null);
				authContext.Properties["Principal"] = principal;
				evaluationContext.Properties["Principal"] = principal;
				return true;
			}

			var user = evaluationContext.ClaimSets
					.SelectMany(c => c.FindClaims(ClaimTypes.Name, Rights.PossessProperty))
					.Select(c1 => c1.Resource.ToString())
					.FirstOrDefault();

			var identity = (evaluationContext.Properties["Identities"] as IEnumerable<IIdentity>)?.First();

			using (var userStore = new NorthwindUsers.NorthwindUsers())
			{
				var roles = userStore.Users.SingleOrDefault(u => u.UserName == user)?.Roles.Select(r => r.RoleName).ToList();
				evaluationContext.Properties["Principal"] = new CustomPrincipal(identity, roles);
			}

			return true;
		}
	}
}
