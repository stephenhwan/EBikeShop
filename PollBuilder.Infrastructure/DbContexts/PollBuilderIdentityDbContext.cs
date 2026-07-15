using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PollBuilder.Application.Interfaces;
using PollBuilder.Domain.Entities.Identity;


namespace PollBuilder.Infrastructure.DbContexts
{
	public class PollBuilderIdentityDbContext : IdentityDbContext<User, UserRole, string>, IPollBuilderIdentityDbContext
	{
		public PollBuilderIdentityDbContext(
			DbContextOptions<PollBuilderIdentityDbContext> options)
			: base(options)
		{

		}

		public DbSet<UserRole> UserRole => Roles;
	}
}
