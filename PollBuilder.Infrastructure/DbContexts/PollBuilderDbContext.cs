using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PollBuilder.Application.Interfaces;
using PollBuilder.Domain.Entities.Identity;
using PollBuilder.Domain.Entities.PollBuilder;

namespace PollBuilder.Infrastructure.DbContexts
{
	public class PollBuilderDbContext : DbContext, IPollBuilderDbContext
	{
		public PollBuilderDbContext(
		DbContextOptions<PollBuilderDbContext> options)
		: base(options)
		{

		}

		//khai bao DbsetKhi AppDbContext kế thừa từ DbContext của Entity Framework và thực thi IAppDbContext,
		//nó cần phải định nghĩa lại các thuộc tính đó để:

		//Thỏa mãn "bản hợp đồng" của Interface nhằm tránh lỗi biên dịch.

		//Cho Entity Framework Core biết rõ đây là các bảng cần được lập bản đồ(mapping) và quản lý dưới Database.

		public DbSet<Poll> Polls { get; set; }
		public DbSet<Question> Questions { get; set; }
		public DbSet<Option> Options { get; set; }
		public DbSet<Vote> Votes { get; set; }

		public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
		{
			return Database.BeginTransactionAsync(cancellationToken);
		}
		//Confi attribute for each entities

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(PollBuilderDbContext).Assembly);
		}
	}
}
