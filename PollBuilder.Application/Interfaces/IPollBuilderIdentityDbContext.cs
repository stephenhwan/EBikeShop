using Microsoft.EntityFrameworkCore;
using PollBuilder.Domain.Entities.Identity;

namespace PollBuilder.Application.Interfaces
{
	public interface IPollBuilderIdentityDbContext
	{
		DbSet<User> Users { get; }
		DbSet<UserRole> UserRole { get; }
	}
}
