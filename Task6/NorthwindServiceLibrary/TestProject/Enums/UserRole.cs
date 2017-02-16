using System;

namespace TestProject.Enums
{
	[Flags]
	public enum UserRole
	{
		None = 0,
		Operator = 1,
		Manager = 2
	}
}
