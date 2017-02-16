namespace NorthwindUsers
{
	using System.Data.Entity;

	public partial class NorthwindUsers : DbContext
	{
		public NorthwindUsers ()
			: base("name=Northwind")
		{
		}

		public virtual DbSet<Role> Roles { get; set; }
		public virtual DbSet<User> Users { get; set; }

		protected override void OnModelCreating (DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Role>()
				.HasMany(e => e.Users)
				.WithMany(e => e.Roles)
				.Map(m => m.ToTable("UserInRoles").MapLeftKey("RoleId").MapRightKey("UserId"));
		}
	}
}
