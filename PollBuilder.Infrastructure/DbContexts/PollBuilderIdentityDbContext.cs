using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PollBuilder.Application.Interfaces;
using PollBuilder.Domain.Entities.Identity;


namespace PollBuilder.Infrastructure.DbContexts
{
	public class PollBuilderIdentityDbContext : IdentityDbContext, IPollBuilderIdentityDbContext
	{
		public PollBuilderIdentityDbContext(
			DbContextOptions<PollBuilderIdentityDbContext> options)
			: base(options)
		{

		}
		public DbSet<User> Users { get; set; }
		public DbSet<UserRole> UserRole { get; set; }

	}
}
