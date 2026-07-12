using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PollBuilder.Application.Interfaces;
using PollBuilder.Domain.Entities;

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

		public DbSet<User> Users { get; set; }
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
			modelBuilder.Entity<User>(builder =>
			{
				// Dòng mã này sẽ tự động tìm tất cả các lớp triển khai IEntityTypeConfiguration 
				// nằm trong cùng Assembly (chính là project Infrastructure này) và áp dụng chúng.
				modelBuilder.ApplyConfigurationsFromAssembly(typeof(PollBuilderDbContext).Assembly);
			});
		}
	}
}
