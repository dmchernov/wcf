using Topshelf;

namespace NorthwindWindowService
{
	class Program
	{
		static void Main (string[] args)
		{
			HostFactory.Run(x =>
			{
				x.Service<NorthwindWinService>();
				x.SetServiceName("NorthwindWinService");
				x.SetDisplayName("Northwind Windows Service");
				x.RunAsLocalService();
			});
		}
	}
}
